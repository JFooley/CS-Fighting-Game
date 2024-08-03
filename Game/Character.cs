using System.Collections.Generic;
using System.Drawing;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using Animation_Space;

// ----- Default States -------
// Idle
// WalkingForward
// WalkingBackward
// DashForward
// DashBackward
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
public class Character
{
    public string name;
    public bool facingRight = true;
    public float size_ratio = 2.0f;

    public int LifePoints = 0;
    public int StunPoints = 0;
    public int BlockStunFrames = 0;
    public int HitStunFrames = 0;

    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public string CurrentState { get; private set; }
    private string LastState { get; set; }

    public bool canNormalAtack => this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward" || this.CurrentState == "Crouching" || this.CurrentState == "CrouchingIn" || this.CurrentState == "CrouchingOut";
    public bool onGround => !(this.CurrentState == "Jumping") || !(this.CurrentState == "JumpingForward") || !(this.CurrentState == "JumpingBackward") || !(this.CurrentState == "Airboned");
    public bool onHitStun => this.CurrentState == "OnHit" || this.CurrentState == "OnHitCrouching";

    public Dictionary<string, Animation> animations;
    public Animation CurrentAnimation => animations[CurrentState];
    private Dictionary<int, Sprite> spriteImages;
    private Dictionary<string, Sound> characterSounds;

    public string folderPath;
    public string soundFolderPath;

    public int CurrentSprite;
    public string CurrentSound;

    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    public Character(string name, string initialState, int startX, int startY, string folderPath, string soundFolderPath) {   
        this.folderPath = folderPath;
        this.soundFolderPath = soundFolderPath;
        this.name = name;
        this.CurrentState = initialState;
        this.LastState = initialState;
        this.PositionX = startX;
        this.PositionY = startY;
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

    public void Render(RenderWindow window) {
        // Render sprite
        Sprite temp_sprite = this.GetCurrentSpriteImage();
        temp_sprite.Position = new Vector2f(this.PositionX, this.PositionY);
        temp_sprite.Scale = new Vector2f(this.size_ratio, this.size_ratio);
        window.Draw(temp_sprite);

        // Play sounds
        if (characterSounds.ContainsKey(this.CurrentSound)) {
            this.characterSounds[this.CurrentSound].Play();
        }
    }
    
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
    
    public void LoadSpriteImages() {
        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(this.folderPath)) {
            throw new System.IO.DirectoryNotFoundException($"O diretório {this.folderPath} não foi encontrado.");
        }

        // Obtém todos os arquivos no diretório especificado
        string[] files = System.IO.Directory.GetFiles(this.folderPath);

        foreach (string file in files) {
            try
            {
                // Tenta carregar a textura
                Texture texture = new Texture(file);
                Sprite sprite = new Sprite(texture);
                
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

    public void LoadSounds() {
        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(this.soundFolderPath)) {
            throw new System.IO.DirectoryNotFoundException($"O diretório {this.soundFolderPath} não foi encontrado.");
        }

        // Obtém todos os arquivos no diretório especificado
        string[] files = System.IO.Directory.GetFiles(this.soundFolderPath);

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

    public virtual void Load() {}
    public virtual void DoBehavior() {}
}


}
