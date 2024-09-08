using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Aux_Space;
using Input_Space;

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
public class Character {
    // Infos
    public string name;
    public int facing = 1;
    public float size_ratio = 1.0f;
    public string folderPath;
    public string soundFolderPath;
    public int floorLine;

    // Controls
    public int player { get; set; }

    // Statistics 
    public Vector2i LifePoints = new Vector2i(1000, 1000);
    public Vector2i StunPoints = new Vector2i(50, 50);
    public int move_speed = 0;
    public int dash_speed = 0;
    public int jump_hight = 80;
    public int push_box_width = 0;

    // Object infos
    public Physics physics = new Physics();
    public Vector2i Position = new Vector2i(0, 0);
    public Vector3f Velocity = new Vector3f(0, 0, 0);
    public string CurrentState { get; set; }
    private string LastState { get; set; }

    // Combat logic infos
    public bool notActing => this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward" || this.CurrentState == "Crouching" || this.CurrentState == "CrouchingIn" || this.CurrentState == "CrouchingOut";
    public bool notActingAir => this.CurrentState == "Jump" || this.CurrentState == "JumpForward" || this.CurrentState == "JumpBackward";
    public bool onHitStun => this.CurrentState == "OnHit" || this.CurrentState == "OnHitCrouching" || this.CurrentState == "Airboned";
    public bool onBlockStun => this.CurrentState == "OnBlock" || this.CurrentState == "OnBlockCrouching";
    public bool canAct = false;
    public bool hasHit = false; // Colidiu com algo nesse frame
    private bool blockingHigh = false;
    private bool blockingLow = false;

    // Data structs
    public Dictionary<string, Animation> animations;
    private Dictionary<int, Sprite> spriteImages;
    private Dictionary<string, Sound> characterSounds;
    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    // Gets
    public int CurrentSprite;
    public string CurrentSound;
    public Animation CurrentAnimation => animations[CurrentState];
    public int CurrentFrameIndex => animations[CurrentState].currentFrameIndex;

    public Character(string name, string initialState, int startX, int startY, string folderPath, string soundFolderPath) {   
        this.folderPath = folderPath;
        this.soundFolderPath = soundFolderPath;
        this.name = name;
        this.CurrentState = initialState;
        this.LastState = initialState;
        this.Position.X = startX;
        this.Position.Y = startY;
        this.floorLine = startY;
        this.spriteImages = new Dictionary<int, Sprite>();
        this.characterSounds = new Dictionary<string, Sound>();
    }

    public void Update() {
        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentFrame().Sprite_index;
        this.CurrentSound = CurrentAnimation.GetCurrentFrame().Sound_index;

        // Update position
        Position.X += CurrentAnimation.GetCurrentFrame().DeltaX;
        Position.Y += CurrentAnimation.GetCurrentFrame().DeltaY;
        this.physics.Update(this);

        // Advance to the next frame
        if (CurrentAnimation.AdvanceFrame()) this.hasHit = false;
        if (this.CurrentAnimation.onLastFrame) {
            this.CurrentAnimation.Reset();
            if (CurrentAnimation.doChangeState) {
                this.ChangeState(this.CurrentAnimation.post_state);
            }
        }

        // Do Behaviour
        if (this.canAct) this.DoBehavior();
    }
    public void Render(RenderWindow window, bool drawHitboxes = false) {
        // Get onScreen position
        var realPosition = new Vector2f(this.Position.X - 125, this.Position.Y - 250);

        // Render sprite
        Sprite temp_sprite = this.GetCurrentSpriteImage();
        temp_sprite.Position = realPosition;
        temp_sprite.Scale = new Vector2f(this.size_ratio, this.size_ratio);
        window.Draw(temp_sprite);

        // Play sounds
        if (characterSounds.ContainsKey(this.CurrentSound)) {
            if (!(this.characterSounds[this.CurrentSound].Status == SoundStatus.Playing)) this.characterSounds[this.CurrentSound].Play();
        }
        
        // Draw Hitboxes
        if (drawHitboxes) {
            foreach (GenericBox box in this.CurrentBoxes) {
                // Calcula as coordenadas absolutas da hitbox
                int x1 = (int)(realPosition.X + box.pA.X * size_ratio);
                int y1 = (int)(realPosition.Y + box.pA.Y * size_ratio);
                int x2 = (int)(realPosition.X + box.pB.X * size_ratio);
                int y2 = (int)(realPosition.Y + box.pB.Y * size_ratio);

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

    // Battle methods
    public virtual void DoBehavior() {}
    public virtual void ImposeBehavior(Character target, bool isblockingHigh = false, bool isblockingLow = false) {}
    public bool isBlockingHigh() {
        if (this.notActing && InputManager.Instance.Key_hold("Left", player: this.player) && this.facing == 1) return true;
        else if (this.notActing && InputManager.Instance.Key_hold("Right", player: this.player) && this.facing == -1) return true;
        return false;
    }
    public bool isBlockingLow() {
        if (this.notActing && InputManager.Instance.Key_hold("Left", player: this.player) && InputManager.Instance.Key_hold("Down", player: this.player) && this.facing == 1) return true;
        else if (this.notActing && InputManager.Instance.Key_hold("Right", player: this.player) && InputManager.Instance.Key_hold("Down", player: this.player) && this.facing == -1) return true;
        return false;
    }

    // Auxiliar methods
    public void SetVelocity(int X = 0, int Y = 0, int T = 0) {
        this.Velocity.X = X;
        this.Velocity.Y = Y;
        this.Velocity.Z = T;

        this.physics.reset();
    }
    public void ChangeState(string newState, int index = 0, bool reset = false) {
        if (animations.ContainsKey(newState)) {
            this.LastState = CurrentState;
            this.CurrentState = newState;
        }

        if (CurrentState != LastState || reset) {
            this.animations[CurrentState].Reset();
            this.LastState = CurrentState;
            this.CurrentAnimation.currentFrameIndex = index;
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
        this.StunPoints.X = this.StunPoints.Y;
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
    public virtual void Load() {}
    public void Unload() {
        this.UnloadSounds();
        this.UnloadSpriteImages();
    }

}


}
