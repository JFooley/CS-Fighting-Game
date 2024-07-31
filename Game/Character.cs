using System.Collections.Generic;
using System.Drawing;
using SFML.Graphics;
using Animation_Space;

namespace Character_Space {
public class Character
{
    public string name;
    public float size_ratio = 1;

    public int LifePoints = 0;
    public int StunPoints = 0;
    public int onBlockStun = 0;
    public int onHitStun = 0;

    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public string CurrentState { get; private set; }
    private string LastState { get; set; }

    public bool canNormalAtack => this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward";
    public bool onGround => !(this.CurrentState == "Jumping") || !(this.CurrentState == "JumpingForward") || !(this.CurrentState == "JumpingBackward") || !(this.CurrentState == "Airboned");

    public Dictionary<string, Animation> animations;
    public Animation CurrentAnimation => animations[CurrentState];
    private Dictionary<int, Sprite> spriteImages;
    public string folderPath;

    public int CurrentSprite;
    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    public Character(string name, string initialState, int startX, int startY, string folderPath) {   
        this.folderPath = folderPath;
        this.name = name;
        CurrentState = initialState;
        LastState = initialState;
        PositionX = startX;
        PositionY = startY;
        spriteImages = new Dictionary<int, Sprite>();
    }

    public void Update() {
        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentFrame().Sprite_index;

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

    public void UnloadSpriteImages()
    {
        foreach (var image in spriteImages.Values)
        {
            image.Dispose(); // Free the memory used by the image
        }
        spriteImages.Clear(); // Clear the dictionary
    }

    public virtual void Load() {
        var animations = new Dictionary<string, Animation> {};
        this.animations = animations;
    }

    public virtual void DoBehavior() {}
}


}
