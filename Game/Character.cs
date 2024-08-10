using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using Animation_Space;
using Aux_Space;

// ----- Default States -------
// Idle
// WalkingForward
// WalkingBackward
// DashForward
// DashBackward
// JumpIn
// Jump
// JumpOut
// CrouchingIn
// Crouching
// CrouchingOut
// OnHit
// OnHitCrouching
// OnBlock
// OnBlockCrouching
// Airboned
// Fallen

namespace Character_Space {
public class Character {
    // Infos
    public string name;
    public bool facingRight = true;
    public float size_ratio = 1.0f;
    public string folderPath;
    public string soundFolderPath;
    public int floorLine;

    // Statistics 
    public int LifePoints = 0;
    public int StunPoints = 0;
    public int BlockStunFrames = 0;
    public int HitStunFrames = 0;
    public int move_speed = 0;

    // Object infos
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public string CurrentState { get; set; }
    private string LastState { get; set; }

    // Combat logic infos
    public bool canNormalAtack => this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward" || this.CurrentState == "Crouching" || this.CurrentState == "CrouchingIn" || this.CurrentState == "CrouchingOut";
    public bool onGround => !(this.PositionY > this.floorLine);
    public bool onHitStun => this.CurrentState == "OnHit" || this.CurrentState == "OnHitCrouching";

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
        this.PositionX = startX;
        this.PositionY = startY;
        this.floorLine = startY;
        this.spriteImages = new Dictionary<int, Sprite>();
        this.characterSounds = new Dictionary<string, Sound>();
    }

    public void Update() {
        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentFrame().Sprite_index;
        this.CurrentSound = CurrentAnimation.GetCurrentFrame().Sound_index;

        // Update position
        PositionX += CurrentAnimation.GetCurrentFrame().DeltaX;
        PositionY += CurrentAnimation.GetCurrentFrame().DeltaY;

        // Check Push Box

        // Check agressive colisions

        // Advance to the next frame
        CurrentAnimation.AdvanceFrame();
        if (this.CurrentAnimation.onLastFrame) {
            this.CurrentAnimation.Reset();
            if (CurrentAnimation.doChangeState) {
                this.ChangeState(this.CurrentAnimation.post_state);
            }
        }

        // Do Behaviour
        this.DoBehavior();
    }

    public void Render(RenderWindow window, bool drawHitboxes = false) {
        // Get onScreen position
        var realPosition = new Vector2f(this.PositionX - 125, this.PositionY);

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
                int x1 = (int)(realPosition.X + box.x1 * size_ratio);
                int y1 = (int)(realPosition.Y + box.y1 * size_ratio);
                int x2 = (int)(realPosition.X + box.x2 * size_ratio);
                int y2 = (int)(realPosition.Y + box.y2 * size_ratio);

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
                    OutlineThickness = 2.0f // Espessura do contorno
                };

                // Desenha o retângulo da hitbox na janela
                window.Draw(hitboxRect);
            }
        }
    }
    
    public virtual void DoBehavior() {}

    // Auxiliar instructions
    public void ChangeState(string newState) {
        if (animations.ContainsKey(newState)) {
            LastState = CurrentState;
            CurrentState = newState;
        }

        if (CurrentState != LastState)
        {
            animations[CurrentState].Reset();
            LastState = CurrentState;
        }
    }

    public Sprite GetCurrentSpriteImage() {
        if (spriteImages.ContainsKey(this.CurrentSprite))
        {
            return spriteImages[this.CurrentSprite];
        }
        return new Sprite(); 
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
