using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Aux_Space;
using Input_Space;
using Stage_Space;
using UI_space;
using System.Security.Cryptography.X509Certificates;

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
// OnHitLow
// OnBlock
// OnBlockLow
// Airboned
// OnGround
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
    public int StunFrames = 0;
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
    public bool isCrounching = false;
    public bool onAir => this.Position.Y < this.floorLine ? true : false;
    public bool hasHit = false; 
    public bool onHit => this.CurrentState.Contains("Airboned") || this.CurrentState.Contains("OnHit");

    private bool blockingHigh = false;
    private bool blockingLow = false;
    private bool blocking = false;

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
    public int lastFrameIndex = -1;

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
        temp_sprite.Position = new Vector2f(this.Position.X - (temp_sprite.GetLocalBounds().Width / 2 * this.facing), this.Position.Y - temp_sprite.GetLocalBounds().Height);
        temp_sprite.Scale = new Vector2f(this.size_ratio * this.facing, this.size_ratio);
        window.Draw(temp_sprite);

        // Play sounds
        this.PlaySound();
        
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
                Color color;
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
                    OutlineColor = color, 
                    OutlineThickness = 1.0f 
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

        // Change state, if necessary
        if (this.CurrentAnimation.onLastFrame && CurrentAnimation.doChangeState) {
            this.ChangeState(this.CurrentAnimation.post_state);
        }

        // Advance to the next frame and reset hit if necessary
        this.lastFrameIndex = this.CurrentFrameIndex;
        if (CurrentAnimation.AdvanceFrame() && CurrentAnimation.GetCurrentFrame().hasHit == false) this.hasHit = false;
    }
    public void PlaySound() {
        if (this.CurrentFrameIndex != this.lastFrameIndex && this.CurrentSound != null && characterSounds.ContainsKey(this.CurrentSound)) {
            this.characterSounds[this.CurrentSound].Volume = Config.Character_Volume;
            this.characterSounds[this.CurrentSound].Play();
        }
    }
    
    // Battle methods
    public virtual int ImposeBehavior(Character target) {
        return -1;
    }
    public bool isBlocking() {
        return this.isBlockingHigh() || this.isBlockingLow();
    }
    public bool isBlockingHigh() {
        if (this.blockingHigh || this.blocking) return true;
        return (this.notActing || this.CurrentState == "OnBlock") && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Down", player: this.playerIndex, facing: this.facing);
    }
    public bool isBlockingLow() {
        if (this.blockingLow || this.blocking) return true;
        return (this.notActing || this.CurrentState == "OnBlockCrouching") && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Down", player: this.playerIndex);
    }
    public void HitStun(Character enemy, int advantage, bool airbone = false, int airbone_height = 50, bool force = false) {
        if (airbone || this.LifePoints.X <= 0 || this.onAir) {
            this.ChangeState("Airboned", reset: true);
            this.SetVelocity(
                X: -5, 
                Y: airbone_height, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
            this.StunFrames = 0;
            return;
        }
        else if (this.isCrounching) this.ChangeState("OnHitLow", reset: true);
        else this.ChangeState("OnHit", reset: true);

        if (force) {
            this.StunFrames = Math.Max(advantage, 1);
        } else {
            this.StunFrames = Math.Max(60 / enemy.CurrentAnimation.framerate * (enemy.CurrentAnimation.animSize - enemy.CurrentFrameIndex) + advantage, 1);
        }
    }
    public void BlockStun(Character enemy, int advantage, bool force = false) {
        if (this.isCrounching) this.ChangeState("OnBlockLow", reset: true);
        else this.ChangeState("OnBlock", reset: true);

        if (force) {
            this.StunFrames = Math.Max(advantage, 0);
        } else {
            this.StunFrames = Math.Max(60 / enemy.CurrentAnimation.framerate * (enemy.CurrentAnimation.animSize - enemy.CurrentFrameIndex) + advantage, 0);
        }
    }

    // Static Methods 
    public static void Pushback(Character target, Character self, string amount, float Y_amount = 0, bool force_push = false) {
        if ((target.Position.X <= Camera.Instance.X - ((Config.maxDistance - 20) / 2) || target.Position.X >= Camera.Instance.X + ((Config.maxDistance - 20) / 2)) && !force_push) {
            if (amount == "Light") {
                self.SetVelocity(-Config.light_pushback, Y_amount, Config.light_pushback_frames);
            } else if (amount == "Medium") {
                self.SetVelocity(-Config.medium_pushback, Y_amount, Config.medium_pushback_frames);
            } else if (amount == "Heavy"){
                self.SetVelocity(-Config.heavy_pushback, Y_amount, Config.heavy_pushback_frames);
            }
        } else {
            if (amount == "Light") {
                target.SetVelocity(-Config.light_pushback, Y_amount, Config.light_pushback_frames);
            } else if (amount == "Medium") {
                target.SetVelocity(-Config.medium_pushback, Y_amount, Config.medium_pushback_frames);
            } else if (amount == "Heavy"){
                target.SetVelocity(-Config.heavy_pushback, Y_amount, Config.heavy_pushback_frames);
            }
        }
    }
    public static void Damage(Character target, int damage, int dizzy_damage) {
        target.LifePoints.X -= damage;
        target.DizzyPoints.X -= dizzy_damage;
    }
    public void Stun(Character enemy, bool hit, int advantage, bool airbone = false, int airbone_height = 50, bool force = false) {
        if (hit) {
            if (airbone || this.LifePoints.X <= 0) {
                this.ChangeState("Airboned", reset: true);
                this.SetVelocity(
                    X: -5, 
                    Y: airbone_height, 
                    T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
                this.StunFrames = 0;
                return;
            }
            else if (this.isCrounching) this.ChangeState("OnHitLow", reset: true);
            else this.ChangeState("OnHit", reset: true);
            
        } else {
            if (this.isCrounching) this.ChangeState("OnBlockLow", reset: true);
            else this.ChangeState("OnBlock", reset: true);
        }

        if (force) {
            this.StunFrames = Math.Max(advantage, 1);
        } else {
            this.StunFrames = Math.Max(60 / enemy.CurrentAnimation.framerate * (enemy.CurrentAnimation.animSize - enemy.CurrentFrameIndex) + advantage, 1);
        }

    }

    // Auxiliar methods
    public void SetVelocity(float X = 0, float Y = 0, int T = 0) {
        this.Velocity.X = X;
        this.Velocity.Y = Y;
        this.Velocity.Z = T - 1;

        this.physics.reset();
    }
    public void AddVelocity(float X = 0, float Y = 0, int T = 0) {
        this.Velocity.X += X;
        this.Velocity.Y += Y;
        this.Velocity.Z += T;

        this.physics.reset();
    }
    public void ChangeState(string newState, int index = 0, bool reset = false) {
        this.LastState = CurrentState;

        if (animations.ContainsKey(newState)) {
            this.CurrentState = newState;
            
        }

        if (CurrentState != LastState || reset) {
            this.animations[LastState].Reset();
            this.CurrentAnimation.currentFrameIndex = index;
        }

        this.hasHit = false;

        if (this.CurrentState == "Crouching" || this.CurrentState.Contains("Low")) {
            this.isCrounching = true;
        } else {
            this.isCrounching = false;
        }
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
    public static Character SelectCharacter(int index, Stage stage) {
        switch (index) {
            case 0:
                var Ken_object = new Ken("Intro", 0, stage.floorLine, stage);
                Ken_object.Load();
                return Ken_object;
            
            case 1:
                var Psylock_object = new Psylock("Intro", 0, stage.floorLine, stage);
                Psylock_object.Load();
                return Psylock_object;

            default:
                var Default_object = new Ken("Intro", 0, stage.floorLine, stage);
                Default_object.Load();
                return Default_object;
        }
    }
    public virtual void Load() {}
    public override void Unload() {
        this.UnloadSounds();
        this.UnloadSpriteImages();
    }
    public Character Copy(){
        Character copy = new Character(this.name, this.CurrentState, this.Position.X, this.Position.Y, this.folderPath, this.soundFolderPath, this.stage);

        copy.spriteImages = this.spriteImages;
        copy.characterSounds = this.characterSounds;
        copy.animations = this.animations;

        return copy;
    }
}

}
