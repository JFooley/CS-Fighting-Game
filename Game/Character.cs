using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Aux_Space;
using Input_Space;
using Stage_Space;

// ----- Default States -------
// Intro
// Idle
// WalkingForward
// WalkingBackward
// DashForward
// DashBackward
// JumpForward
// Jump
// JumpBackward
// CrouchingIn
// Crouching
// CrouchingOut
// OnHit
// OnHitCrouching
// OnBlock
// OnBlockCrouching
// Airboned
// Fallen
// Wakeup

namespace Character_Space {
public class Character : Object_Space.Object {
    // Infos
    public string name;
    public string folderPath;
    public string soundFolderPath;
    public float floorLine;
    public int team;
    public Stage stage;

    // Controls
    public int playerIndex { get; set; }

    // Statistics 
    public Vector2i LifePoints = new Vector2i(1000, 1000);
    public Vector2i DizzyPoints = new Vector2i(1000, 1000);
    public int move_speed = 0;
    public int dash_speed = 0;
    public int jump_hight = 80;
    public int push_box_width = 0;

    // Object infos
    public Physics physics = new Physics();
    public Vector2f VisualPosition => new Vector2f(this.Position.X - 125, this.Position.Y - 250);
    public Vector3f Velocity = new Vector3f(0, 0, 0);
    public string CurrentState { get; set; }
    private string LastState { get; set; }

    // Combat logic infos
    public bool notActing => this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward" || this.CurrentState == "Crouching" || this.CurrentState == "CrouchingIn" || this.CurrentState == "CrouchingOut" || (this.CurrentState == "DashForward" && this.CurrentAnimation.onLastFrame) || (this.CurrentState == "DashBackward" && this.CurrentAnimation.onLastFrame);
    public bool notActingAir => this.CurrentState == "Jump" || this.CurrentState == "JumpForward" || this.CurrentState == "JumpBackward";
    public bool onHitStun => this.CurrentState == "OnHit" || this.CurrentState == "OnHitCrouching" || this.CurrentState == "Airboned";
    public bool onBlockStun => this.CurrentState == "OnBlock" || this.CurrentState == "OnBlockCrouching";
    public bool hasHit = false; 
    private bool blockingHigh = false;
    private bool blockingLow = false;

    // Data
    public Dictionary<string, Animation> animations = new Dictionary<string, Animation>{};
    public Dictionary<int, Sprite> spriteImages = new Dictionary<int, Sprite>{};
    public Dictionary<string, Sound> characterSounds = new Dictionary<string, Sound>{};
    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    // Gets
    public int CurrentSprite;
    public string CurrentSound = "";
    public Animation CurrentAnimation => animations[CurrentState];
    public int CurrentFrameIndex => animations[CurrentState].currentFrameIndex;

    public Character(string name, string initialState, float startX, float startY, string folderPath, string soundFolderPath, Stage stage) : base() {
        this.folderPath = folderPath;
        this.soundFolderPath = soundFolderPath;
        this.name = name;
        this.CurrentState = initialState;
        this.LastState = initialState;
        this.stage = stage;
        this.Position = new Vector2f(startX, startY);
        this.floorLine = startY;
        this.spriteImages = new Dictionary<int, Sprite>();
        this.characterSounds = new Dictionary<string, Sound>();
    }

    // Every Frame methods
    public override void Update() {
        base.Update();
        this.DoBehave();
        this.DoAnimate();
    }
    public override void DoRender(RenderWindow window, bool drawHitboxes = false) {
        base.DoRender(window, drawHitboxes);
        // Render sprite
        Sprite temp_sprite = this.GetCurrentSpriteImage();
        temp_sprite.Position = new Vector2f(this.Position.X - (this.quadsize / 2 * this.facing), this.Position.Y - this.quadsize);
        temp_sprite.Scale = new Vector2f(this.size_ratio * this.facing, this.size_ratio);
        window.Draw(temp_sprite);

        // Play sounds
        if (this.CurrentSound != null && characterSounds.ContainsKey(this.CurrentSound)) {
            // if (!(this.characterSounds[this.CurrentSound].Status == SoundStatus.Playing)) this.characterSounds[this.CurrentSound].Play();
            this.characterSounds[this.CurrentSound].Volume = Config.Character_Volume;
            this.characterSounds[this.CurrentSound].Play();
        }
        
        // Draw Hitboxes
        if (drawHitboxes) {  
            // Desenha o ponto central
            RectangleShape anchor = new RectangleShape(new Vector2f(1, 10)) {
                Position = new Vector2f(this.Position.X, this.Position.Y - 55),
                FillColor = SFML.Graphics.Color.Transparent,
                OutlineColor = Color.White, 
                OutlineThickness = 1.0f 
            };
            
            window.Draw(anchor);

            foreach (GenericBox box in this.CurrentBoxes) {
                // Calcula as coordenadas absolutas da hitbox
                float x1 = box.getRealA(this).X * size_ratio;
                float y1 = box.getRealA(this).Y * size_ratio;
                float x2 = box.getRealB(this).X * size_ratio;
                float y2 = box.getRealB(this).Y * size_ratio;

                // Cria o retângulo da hitbox
                var color = SFML.Graphics.Color.Transparent;
                switch (box.type) {
                    case 0:
                        color = SFML.Graphics.Color.Red;
                        break;
                    case 1:
                        color = SFML.Graphics.Color.Blue;
                        break;
                    default:
                        color = SFML.Graphics.Color.White;
                        break;
                }

                RectangleShape hitboxRect = new RectangleShape(new Vector2f(x2 - x1, y2 - y1)) {
                    Position = new Vector2f(x1, y1),
                    FillColor = SFML.Graphics.Color.Transparent,
                    OutlineColor = color, // Cor de contorno para a hitbox
                    OutlineThickness = 1.0f // Espessura do contorno
                };

                // Desenha o retângulo da hitbox na janela
                window.Draw(hitboxRect);
            }
        }
    }
    public override void DoAnimate() {
        base.DoAnimate();
        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentFrame().Sprite_index;
        this.CurrentSound = CurrentAnimation.GetCurrentFrame().Sound_index;

        // Update position
        Position.X += CurrentAnimation.GetCurrentFrame().DeltaX * this.facing;
        Position.Y += CurrentAnimation.GetCurrentFrame().DeltaY * this.facing;
        this.physics.Update(this);

        // Advance to the next frame
        if (this.CurrentAnimation.onLastFrame && CurrentAnimation.doChangeState) {
            this.ChangeState(this.CurrentAnimation.post_state);
        }
        CurrentAnimation.AdvanceFrame();
    }

    // Battle methods
    public virtual bool ImposeBehavior(Character target) {
        return true;
    }
    public bool isBlocking() {
        return this.isBlockingHigh() || this.isBlockingLow();
    }
    public bool isBlockingHigh() {
        return this.notActing && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Down", player: this.playerIndex, facing: this.facing);
    }
    public bool isBlockingLow() {
        return this.notActing && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Down", player: this.playerIndex);
    }

    // Auxiliar methods
    public void SetVelocity(float X = 0, float Y = 0, int T = 0) {
        this.Velocity.X = X;
        this.Velocity.Y = Y;
        this.Velocity.Z = T;

        this.physics.reset();
    }
    public void ChangeState(string newState, int index = 0, bool reset = false) {
        this.LastState = CurrentState;

        if (animations.ContainsKey(newState)) {
            this.CurrentState = newState;
        }

        if (CurrentState != LastState || reset) {
            this.animations[CurrentState].Reset();
            this.CurrentAnimation.currentFrameIndex = index;
        }

        this.hasHit = false;
    }
    public Sprite GetCurrentSpriteImage() {
        if (spriteImages.ContainsKey(this.CurrentSprite))
        {
            return spriteImages[this.CurrentSprite];
        }
        return new Sprite(); 
    }
    public void Reset(int start_point, int facing, String state = "Idle") {
        this.ChangeState(state);
        this.LifePoints.X = this.LifePoints.Y;
        this.DizzyPoints.X = this.DizzyPoints.Y;
        this.Position.X = start_point;
        this.Position.Y = this.floorLine;
        this.facing = facing;
        this.physics.reset();
        this.Velocity = new Vector3f(0, 0, 0);
    }
   
    // Static Methods 
    public static void Pushback(Character target, string amount) {
        if (amount == "Light") {
            target.SetVelocity(-Config.light_pushback, 0, Config.light_pushback_frames);
        } else if (amount == "Medium") {
            target.SetVelocity(-Config.medium_pushback, 0, Config.medium_pushback_frames);
        } else if (amount == "Heavy"){
            target.SetVelocity(-Config.heavy_pushback, 0, Config.heavy_pushback_frames);
        }
    }
    public static void Damage(Character target, int damage, int dizzy_damage) {
        target.LifePoints.X -= damage;
        target.DizzyPoints.X -= dizzy_damage;
    }
    public static void Stun(Character target, String type, int frame_amount) {
        
    }
    // Visuals load
    public void LoadSpriteImages() {
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullPath = Path.Combine(currentDirectory, this.folderPath);

        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(fullPath)) {
            throw new System.IO.DirectoryNotFoundException($"O diretório {fullPath} não foi encontrado.");
        }

        // Obtém todos os arquivos no diretório especificado
        string[] files = System.IO.Directory.GetFiles(fullPath);

        foreach (string file in files) {
            try
            {
                // Tenta carregar a textura
                Texture texture = new Texture(file);
                Sprite sprite = new Sprite(texture);
                sprite.Texture.Smooth = false;
                
                // Obtém o nome do arquivo sem a extensão
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);
                
                // Usa o nome do arquivo sem extensão como chave no dicionário
                spriteImages[int.Parse(fileNameWithoutExtension)] = sprite;
            }
            catch (SFML.LoadingFailedException)
            {
                // Se falhar ao carregar a imagem, simplesmente ignore
                Console.WriteLine($"Falha ao carregar o arquivo {file} como textura. Ignorando...");
            }
            catch (FormatException)
            {
                // Caso o nome do arquivo não seja um número, ignora
                Console.WriteLine($"O nome do arquivo {file} não é um número válido. Ignorando...");
            }
        }
    }
    public void UnloadSpriteImages() {
        foreach (var image in spriteImages.Values)
        {
            image.Dispose(); // Free the memory used by the image
        }
        spriteImages.Clear(); // Clear the dictionary
    }

    // Sounds load
    public void LoadSounds() {
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullSoundPath = Path.Combine(currentDirectory, this.soundFolderPath);

        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(fullSoundPath)) {
            throw new System.IO.DirectoryNotFoundException($"O diretório {fullSoundPath} não foi encontrado.");
        }

        // Obtém todos os arquivos no diretório especificado
        string[] files = System.IO.Directory.GetFiles(fullSoundPath);

        foreach (string file in files) {
            try
            {
                // Tenta carregar a textura
                var buffer = new SoundBuffer(file);
                
                // Obtém o nome do arquivo sem a extensão
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);

                // Adiciona no dicionário
                this.characterSounds[fileNameWithoutExtension] = new Sound(buffer);
            } catch (SFML.LoadingFailedException ex) {
                Console.WriteLine($"Falha ao carregar o som {file}: {ex.Message}");
            }
        }
    }
    public void UnloadSounds() {
        foreach (var sound in characterSounds.Values)
        {
            sound.Dispose(); // Free the memory used by the image
        }
        characterSounds.Clear(); 
    }

    // General Load
    public virtual void Load() {}
    public override void Unload() {
        this.UnloadSounds();
        this.UnloadSpriteImages();
    }
    public override void Copy(){
        
    }
    }


}
