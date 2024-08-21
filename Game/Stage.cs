using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Character_Space;
using Microsoft.SqlServer.Server;

namespace Stage_Space {

public class Stage {
    // Basic Infos
    public string name = "Empty";
    public float size_ratio = 1.0f;
    public string spritesFolderPath;
    public string soundFolderPath;

    // Battle Info
    public List<Character> OnSceneCharacters { get; set; }
    public Character character_A;
    public Character character_B;
    public int rounds_A;
    public int rounds_B;
    public int round_length = Config.RoundLength;
    public DateTime round_start_time;
    public int elapsed_time => (int) (DateTime.Now - this.round_start_time).TotalSeconds;


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

    // Behaviour
    public void Update(RenderWindow window, bool showBoxs) {
        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentSimpleFrame();

        // Update Chars
        foreach (Character char_object in this.OnSceneCharacters) char_object.Update();

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

        // Render Chars
        foreach (Character char_object in this.OnSceneCharacters) char_object.Render(window, showBoxs);

        // Do Behaviour
        this.DoBehavior();
    }
    private void DoBehavior() {
        int maxDistance = 350;

        // Move characters away from border
        character_A.Position.X = (int) Math.Max(0, Math.Min(character_A.Position.X, this.length));
        character_B.Position.X = (int) Math.Max(0, Math.Min(character_B.Position.X, this.length));

        // Keep characters close
        if (this.character_A.Position.X > character_B.Position.X + maxDistance) this.character_A.Position.X = character_B.Position.X + maxDistance;
        if (this.character_A.Position.X < character_B.Position.X - maxDistance) this.character_A.Position.X = character_B.Position.X - maxDistance;
        if (this.character_B.Position.X > character_A.Position.X + maxDistance) this.character_B.Position.X = character_A.Position.X + maxDistance;
        if (this.character_B.Position.X < character_A.Position.X - maxDistance) this.character_B.Position.X = character_A.Position.X - maxDistance;

        // Keep characters facing each other
        if (this.character_A.Position.X < this.character_B.Position.X) {
            this.character_A.facing = 1;
            this.character_B.facing = -1;
        } else {
            this.character_A.facing = -1;
            this.character_B.facing = 1;
        }

        this.checkColisions();
        this.doSpecialBehaviour();
    }
    public virtual void doSpecialBehaviour() {}

    // Auxiliary
    public void checkColisions() {
        foreach (GenericBox boxA in character_A.CurrentBoxes) {
            foreach (GenericBox boxB in character_B.CurrentBoxes) {
                if (boxA.type == 2 && boxB.type == 2 && boxA.Intersects(boxB, character_A.Position, character_B.Position)) { // Push A e Push B
                    Console.WriteLine("Caso Push");

                } else if (!character_A.hasHit && boxA.type == 0 && boxB.type == 1 && boxA.Intersects(boxB, character_A.Position, character_B.Position)) { // A hit B
                    Console.WriteLine("A hit B");
                    character_A.hasHit = true;
                    character_A.ImposeBehavior(character_B, false);

                } else if (!character_B.hasHit && boxA.type == 1 && boxB.type == 0 && boxB.Intersects(boxA, character_B.Position, character_A.Position)) { // B hit A
                    Console.WriteLine("B hit A");
                    character_B.hasHit = true;
                    character_B.ImposeBehavior(character_A, false);
                }
            }
        }
  
    }
    public void setChars(Character char_A, Character char_B) {
        this.character_A = char_A;
        this.character_A.facing = 1;
        this.character_A.player = 1;

        this.character_B = char_B;
        this.character_B.facing = -1;
        this.character_B.player = 2;

    }
    public bool CheckRoundEnd() {
        bool doEnd = false;

        if (character_A.LifePoints.X <= 0) {
            this.rounds_B += 1;
            doEnd = true;
        }
        if (character_B.LifePoints.X <= 0) {
            this.rounds_A += 1;
            doEnd = true;
        }

        if (this.elapsed_time >= this.round_length) {
            if (character_A.LifePoints.X <= character_B.LifePoints.X) {
                this.rounds_B += 1;
                doEnd = true;
            } 
            if (character_A.LifePoints.X >= character_B.LifePoints.X) {
                this.rounds_A += 1;
                doEnd = true;
            } 
        }

        return doEnd;
    }
    public bool CheckMatchEnd() {       
        if (this.rounds_A == 2 || this.rounds_B == 2) return true;
        return false;
    }
    public void ResetRoundTime() {
        this.round_start_time = DateTime.Now;
    }
    public void ResetMatch() {
        this.rounds_A = 0;
        this.rounds_B = 0;
    }
    public void ResetPlayers(bool force = false) {
        if (force) {
            this.character_A.Reset(this.start_point_A, facing: 1, state: "Intro");
            this.character_B.Reset(this.start_point_B, facing: -1, state: "Intro");
        } else {
            this.character_A.Reset(this.start_point_A, facing: 1);
            this.character_B.Reset(this.start_point_B, facing: -1);
        }
    }
    public void TogglePlayers() {
        this.character_A.canAct = !this.character_A.canAct;
        this.character_B.canAct = !this.character_B.canAct;
    }

    // All loads
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