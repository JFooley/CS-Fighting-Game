using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Character_Space;

namespace Stage_Space {

public class Stage {
    // Basic Infos
    public string name = "Empty";
    public float size_ratio = 1.0f;
    public string spritesFolderPath;
    public string soundFolderPath;

    // Battle Info
    public Character character_A;
    public Character character_B;
    public int rounds_A;
    public int rounds_B;

    // Technical infos
    public int floorLine;
    public int length;
    public int height;
    public int start_point_A;
    public int start_point_B;
    public Vector2i center_point => new Vector2i((int) (length / 2), (int) (height / 2));

    // Animation infos
    public string CurrentState { get; set; }
    public string LastState { get; set; }
    public Dictionary<string, Animation> animations;
    private Dictionary<int, Sprite> spriteImages;
    private Dictionary<string, Sound> stageSounds;
    public int CurrentSprite;
    public string CurrentSound;
    public Animation CurrentAnimation => animations[CurrentState];
    public int CurrentFrameIndex => animations[CurrentState].currentFrameIndex;

    public Stage(string name, int floorLine, int length, int height, string spritesFolderPath, string soundFolderPath) {
        this.name = name;
        this.floorLine = floorLine;
        this.length = length;
        this.height = height;
        this.start_point_A = (int) ((length / 2) - 100);
        this.start_point_B = (int) ((length / 2) + 100);
        this.spritesFolderPath = spritesFolderPath;
        this.soundFolderPath = soundFolderPath;
        this.rounds_A = 0;
        this.rounds_B = 0;
        this.CurrentState = "Default";
        this.spriteImages = new Dictionary<int, Sprite>();
        this.stageSounds = new Dictionary<string, Sound>();
    }

    public void Update(RenderWindow window) {
        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentSimpleFrame();

        // Render sprite
        if (this.spriteImages.ContainsKey(this.CurrentSprite)) {
            Sprite temp_sprite = this.spriteImages[this.CurrentSprite];
            temp_sprite.Scale = new Vector2f(this.size_ratio, this.size_ratio);
            temp_sprite.Position = new Vector2f (0, 0);
            window.Draw(temp_sprite);
        }

        // Play background music
        if (this.stageSounds["music"].Status == SoundStatus.Stopped){
            this.stageSounds["music"].Volume = Config.Music_Volume;
            this.stageSounds["music"].Play();
        }

        // Advance to the next frame
        CurrentAnimation.AdvanceFrame();
        if (this.CurrentAnimation.onLastFrame) {
            this.CurrentAnimation.Reset();
            if (CurrentAnimation.doChangeState) {
                if (animations.ContainsKey(this.CurrentAnimation.post_state)) {
                    this.LastState = this.CurrentState;
                    this.CurrentState = this.CurrentAnimation.post_state;
                }
            }
        }

        // Do Behaviour
        this.DoBehavior();
    }

    private void DoBehavior() {
        // Move characters away from border
        character_A.PositionX = (int) Math.Max(0, Math.Min(character_A.PositionX, this.length));
        character_B.PositionX = (int) Math.Max(0, Math.Min(character_B.PositionX, this.length));

    }

    public void setChars(Character char_A, Character char_B) {
        this.character_A = char_A;
        this.character_B = char_B;
    }

    public void LoadSpriteImages() {
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullPath = Path.Combine(currentDirectory, this.spritesFolderPath);

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
                this.spriteImages[int.Parse(fileNameWithoutExtension)] = sprite;
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
        foreach (var image in this.spriteImages.Values)
        {
            image.Dispose(); // Free the memory used by the image
        }
        this.spriteImages.Clear(); // Clear the dictionary
    }

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
                this.stageSounds[fileNameWithoutExtension] = new Sound(buffer);
            } catch (SFML.LoadingFailedException ex) {
                Console.WriteLine($"Falha ao carregar o som {file}: {ex.Message}");
            }
        }
    }
    public void UnloadSounds() {
        foreach (var sound in this.stageSounds.Values)
        {
            sound.Dispose(); // Free the memory used by the image
        }
        this.stageSounds.Clear(); 
    }

    public virtual void LoadStage() {}

    public virtual void UnloadStage() {}

}

}