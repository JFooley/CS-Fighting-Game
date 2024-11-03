using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Character_Space;
using UI_space;

namespace Stage_Space {

public class Stage {
    // Basic Infos
    public string name = "Empty";
    public float size_ratio = 1.0f;
    public string spritesFolderPath;
    public string soundFolderPath;

    // Battle Info
    private int hitstopCounter = 0;
    public List<Character> OnSceneCharacters = new List<Character> {};
    public List<Character> OnSceneParticles = new List<Character> {};
    public List<Character> newCharacters = new List<Character> {};
    public List<Character> newParticles = new List<Character> {};

    public Character character_A;
    public Character character_B;
    public Vector2f last_pos_A;
    public Vector2f last_pos_B;
    public int rounds_A;
    public int rounds_B;
    public int round_length = Config.RoundLength;
    public DateTime round_start_time;
    public int elapsed_time => (int) (DateTime.Now - this.round_start_time).TotalSeconds;
    public int round_time => elapse_time ? this.round_length - this.elapsed_time : Config.RoundLength;
    public bool elapse_time = true;

    // Technical infos
    public int floorLine;
    public int length;
    public int height;
    public int start_point_A;
    public int start_point_B;
    public Vector2f center_point => new Vector2f(length / 2, height / 2);

    // Aux
    private DateTime timer;
    private double current_time => (DateTime.Now - this.timer).TotalSeconds;

    // Pre-renders
    private Hitspark spark; 
    private Fireball fireball;
    private Particle particle;

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

        this.spark = new Hitspark("Default", 0, 0, 1, this);
        this.fireball = new Fireball("Default", 0, 0, 0, 1, this);
        this.particle = new Particle("Default", 0, 0, 1, this);
    }

    // Behaviour
    public void Update(RenderWindow window, bool showBoxs) {
        if (hitstopCounter > 0) {
            hitstopCounter--;
        }

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
            if (CurrentAnimation.doChangeState) {
                if (animations.ContainsKey(this.CurrentAnimation.post_state)) {
                    this.LastState = this.CurrentState;
                    this.CurrentState = this.CurrentAnimation.post_state;
                }
            }
        }

        // Update chars
        if (hitstopCounter == 0) {
            this.DoBehavior();
            foreach (Character char_object in this.OnSceneCharacters) char_object.Update();
            this.OnSceneCharacters.RemoveAll(obj => obj.remove);
            this.OnSceneCharacters.AddRange(this.newCharacters);
            this.newCharacters.Clear();
        }

        // Update particles
        foreach (Character part_object in this.OnSceneParticles) part_object.Update();
        this.OnSceneParticles.RemoveAll(obj => obj.remove);
        this.OnSceneParticles.AddRange(this.newParticles);
        this.newParticles.Clear();
        
        // Render chars, UI and particles
        foreach (Character char_object in this.OnSceneCharacters) char_object.DoRender(window, showBoxs);
        UI.Instance.DrawBattleUI(window, this);
        foreach (Character part_object in this.OnSceneParticles) part_object.DoRender(window, showBoxs);


    }
    private void DoBehavior() {
        // Move characters away from border
        character_A.Position.X = Math.Max(character_A.push_box_width, Math.Min(character_A.Position.X, this.length - character_A.push_box_width));
        character_B.Position.X = Math.Max(character_B.push_box_width, Math.Min(character_B.Position.X, this.length - character_B.push_box_width));

        // Keep characters close 
        float deltaS = Math.Abs(character_A.Position.X - character_B.Position.X);
        if (deltaS >= Config.maxDistance) {
            if ((character_A.facing == 1 && character_A.Position.X < last_pos_A.X) || (character_A.facing == -1 && character_A.Position.X > last_pos_A.X)) {
                character_A.Position.X = this.last_pos_A.X;
            }
            if ((character_B.facing == 1 && character_B.Position.X < last_pos_B.X) || (character_B.facing == -1 && character_B.Position.X > last_pos_B.X)) {
                character_B.Position.X = this.last_pos_B.X;
            }
        }
        this.last_pos_A = this.character_A.Position;
        this.last_pos_B = this.character_B.Position;
        
        // Keep characters facing each other
        if (this.character_A.Position.X < this.character_B.Position.X) {
            if (this.character_A.CurrentAnimation.currentFrameIndex == 0 || this.character_A.notActing) this.character_A.facing = 1;
            if (this.character_B.CurrentAnimation.currentFrameIndex == 0 || this.character_B.notActing) this.character_B.facing = -1;
        } else {
            if (this.character_A.CurrentAnimation.currentFrameIndex == 0 || this.character_A.notActing) this.character_A.facing = -1;
            if (this.character_B.CurrentAnimation.currentFrameIndex == 0 || this.character_B.notActing) this.character_B.facing = 1;
        }

        this.checkColisions();
        this.doSpecialBehaviour();
    }
    public virtual void doSpecialBehaviour() {}

    // Spawns
    public void spawnParticle(String state,  float X, float Y, int facing = 1, int X_offset = 0, int Y_offset = 0) {
        var par = new Particle(state, X + X_offset * facing, Y + Y_offset, facing);
        par.animations = this.particle.animations;
        par.spriteImages = this.particle.spriteImages;
        par.characterSounds = this.particle.characterSounds;
        this.newParticles.Add(par);
    }
    public void spawnHitspark(bool hit, Vector2f position, int facing, int X_offset = 0, int Y_offset = 0) {
        var hs = new Hitspark("default", position.X + X_offset * facing, position.Y + Y_offset, facing);
        Random random = new Random();
        if (hit) {
            hs.CurrentState = "OnHit" + random.Next(1, 4);
        } else {
            hs.CurrentState = "OnBlock";
        }
        hs.animations = this.spark.animations;
        hs.spriteImages = this.spark.spriteImages;
        hs.characterSounds = this.spark.characterSounds;
        this.newParticles.Add(hs);
    }
    public void spawnFireball(string state, float X, float Y, int facing, int team, int X_offset = 10, int Y_offset = 0) {
        var fb = new Fireball(state, X + X_offset * facing, Y + Y_offset, team, facing, this);
        fb.animations = this.fireball.animations;
        fb.spriteImages = this.fireball.spriteImages;
        fb.characterSounds = this.fireball.characterSounds;
        this.newCharacters.Add(fb);
    }

    // Auxiliary
    public void checkColisions() {
        for (int i = 0; i < this.OnSceneCharacters.Count(); i++) {
            for (int j = 0; j < this.OnSceneCharacters.Count(); j++) {
                if (i == j) continue;
                var charA = this.OnSceneCharacters[i];
                var charB = this.OnSceneCharacters[j];
                foreach (GenericBox boxA in charA.CurrentBoxes) {
                    foreach (GenericBox boxB in charB.CurrentBoxes) {
                        if (boxA.type == 2 && boxB.type == 2 && GenericBox.Intersects(boxA, boxB, charA, charB)) { // Push A e Push B
                            GenericBox.Colide(boxA, boxB, charA, charB);

                        } else if (!charA.hasHit && boxA.type == 0 && boxB.type == 1 && charA.team != charB.team && GenericBox.Intersects(boxA, boxB, charA, charB)) { // A hit B
                            this.hitstopCounter = Config.hitStopTime;
                            charA.hasHit = true;
                            var hit = charA.ImposeBehavior(charB);
                            this.spawnHitspark(hit, charB.Position, charA.facing, -10);

                        }
                    }
                }
            }
        }
    }
    public void setChars(Character char_A, Character char_B) {
        this.character_A = char_A;
        this.character_A.facing = 1;
        this.character_A.playerIndex = 1;
        this.character_A.team = 0;

        this.character_B = char_B;
        this.character_B.facing = -1;
        this.character_B.playerIndex = 2;
        this.character_B.team = 1;

        this.character_A.floorLine = this.floorLine;
        this.character_B.floorLine = this.floorLine;
        this.character_A.Position.X = this.start_point_A;
        this.character_B.Position.X = this.start_point_B;
        this.character_A.stage = this;
        this.character_B.stage = this;

        this.OnSceneCharacters = new List<Character> {this.character_A, this.character_B};
        this.TogglePlayers();
    }
    public bool CheckRoundEnd() {
        if (this.hitstopCounter != 0) return false;
        
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
        if (this.rounds_A == Config.max_rounds || this.rounds_B == Config.max_rounds) return true;
        return false;
    }
    public void ResetRoundTime() {
        this.round_start_time = DateTime.Now;
        this.elapse_time = true;
    }
    public void StopRoundTime() {
        this.elapse_time = false;
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
        this.character_A.behave = !this.character_A.behave;
        this.character_B.behave = !this.character_B.behave;
    }
    public void ResetTimer() {
        this.timer = DateTime.Now;
    }
    public bool CheckTimer(double elapsed_time) {
        return elapsed_time <= this.current_time;
    }
    public void SetHitstop(int amount) {
        this.hitstopCounter = amount;
    }
    
    // All loads
    public void LoadCharacters(int charA_index, int charB_index) {        
        var charA = Character.SelectCharacter(charA_index, this);
        var charB = Character.SelectCharacter(charB_index, this);

        this.setChars(charA, charB);

        this.spark.Load();
        this.fireball.Load();
        this.particle.Load();
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
                // Console.WriteLine($"Falha ao carregar o arquivo {file} como textura. Ignorando...");
            }
            catch (FormatException)
            {
                // Caso o nome do arquivo não seja um número, ignora
                // Console.WriteLine($"O nome do arquivo {file} não é um número válido. Ignorando...");
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
                // Console.WriteLine($"Falha ao carregar o som {file}: {ex.Message}");
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