using System.Collections.Generic;
using System.Drawing;
using SFML.Graphics;
using Animation_Space;

public class Character
{
    public string name;

    public int LifePoints = 1000;
    public int StunPoints = 0;
    public int onBlockStun = 0;
    public int onHitStun = 0;

    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public string CurrentState { get; private set; }
    public string LastState { get; private set; }

    private Dictionary<string, Animation> animations;
    public Animation CurrentAnimation => animations[CurrentState];
    private Dictionary<int, Texture> spriteImages;
    private string folderPath;

    public int CurrentSprite => CurrentAnimation.GetCurrentFrame().Sprite_index;
    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    public Character(string name, Dictionary<string, Animation> animations, string initialState, int startX, int startY, string folderPath)
    {   
        this.folderPath = folderPath;
        this.animations = animations;
        this.name = name;
        CurrentState = initialState;
        LastState = initialState;
        PositionX = startX;
        PositionY = startY;
        spriteImages = new Dictionary<int, Texture>();
    }

    public void Update()
    {
        // Check if state has changed
        if (CurrentState != LastState)
        {
            animations[CurrentState].Reset();
            LastState = CurrentState;
        }

        FrameData frameData = CurrentAnimation.GetCurrentFrame();

        // Update character position
        PositionX += frameData.DeltaX;
        PositionY += frameData.DeltaY;

        // Advance to the next frame
        CurrentAnimation.AdvanceFrame();
        if (this.CurrentAnimation.onLastFrame) {
            this.CurrentAnimation.Reset();
            this.CurrentState = this.CurrentAnimation.post_state;
        }
    }
    
    public void ChangeState(string newState)
    {
        if (animations.ContainsKey(newState))
        {
            LastState = CurrentState;
            CurrentState = newState;
        }
    }

    public Texture GetCurrentSpriteImage()
    {
        int spriteIndex = this.CurrentSprite;
        if (spriteImages.ContainsKey(spriteIndex))
        {
            return spriteImages[spriteIndex];
        }
        return null; 
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
                
                // Obtém o nome do arquivo sem a extensão
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);
                
                // Usa o nome do arquivo sem extensão como chave no dicionário
                spriteImages[int.Parse(fileNameWithoutExtension)] = texture;
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
}
