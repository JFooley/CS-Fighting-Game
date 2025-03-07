using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Character_Space;
using UI_space;
using Input_Space;
using System.Diagnostics;

namespace Stage_Space {

public class Stage {
    // Basic Infos
    public string name = "Empty";
    public float size_ratio = 1.0f;
    public string spritesFolderPath;
    public string soundFolderPath;
    public Sprite thumb;

    // Debug infos
    public bool debug_mode = false;
    public bool pause = false;
    public bool showBoxs = false;
    public bool block_after_hit = false;
    public int reset_frames = 0;

    // Battle Info
    private int hitstopCounter = 0;
    public bool onHitstop => hitstopCounter > 0 ? true : false;
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
    public int elapsed_time => this.stopwatch.Elapsed.Seconds;
    public int round_time => elapse_time ? Config.RoundLength - (int) stopwatch.Elapsed.TotalSeconds : Config.RoundLength;
    public bool elapse_time = true;

    // Technical infos
    public int floorLine;
    public int length;
    public int height;
    public int start_point_A;
    public int start_point_B;
    public Vector2f center_point => new Vector2f(length / 2, height / 2);

    // Aux
    private Stopwatch timer;
    private Stopwatch stopwatch;
    private int pause_pointer = 0;

    // Pre-renders
    private Hitspark spark; 
    private Fireball fireball;
    private Particle particle;

    // Animation infos
    public string CurrentState { get; set; }
    public string LastState { get; set; }
    public Dictionary<string, Animation> animations;
    private Dictionary<string, Sprite> spriteImages;
    private Dictionary<string, Sound> stageSounds;
    public string CurrentSprite = "";
    public string CurrentSound = "";
    public Animation CurrentAnimation => animations[CurrentState];
    public int CurrentFrameIndex => animations[CurrentState].currentFrameIndex;

    Sprite fade90 = new Sprite(new Texture("Assets/ui/90fade.png"));

    public Stage(string name, int floorLine, int length, int height, string spritesFolderPath, string soundFolderPath, string thumbPath) {
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
        this.spriteImages = new Dictionary<string, Sprite>();
        this.stageSounds = new Dictionary<string, Sound>();

        this.spark = new Hitspark("Default", 0, 0, 1, this);
        this.fireball = new Fireball("Default", 0, 0, 0, 1, this);
        this.particle = new Particle("Default", 0, 0, 1, this);

        this.thumb = new Sprite(new Texture(thumbPath));

        this.timer = new Stopwatch();
        this.stopwatch = new Stopwatch();
    }

    // Behaviour
    public void PauseScreen(RenderWindow window) {
        fade90.Position = new Vector2f(Program.camera.X - Config.RenderWidth/2, Program.camera.Y - Config.RenderHeight/2);
        window.Draw(fade90);

        UI.Instance.DrawText(window, "Pause", 0, -75, size: 1.5f, spacing: -30, textureName: "default");
        UI.Instance.DrawText(window, "Show hitboxes",0, 0, spacing: -22, textureName: this.pause_pointer == 0 ? "default black" : "default");
        UI.Instance.DrawText(window, "Training mode", 0, 20, spacing: -22, textureName: this.pause_pointer == 1 ? "default black" : "default");
        if (debug_mode) UI.Instance.DrawText(window, block_after_hit ? "Block after hit" : "Never Block", 0, 40, spacing: -22, textureName: this.pause_pointer == 2 ? "default black" : "default");
        else UI.Instance.DrawText(window, block_after_hit ? "Block after hit" : "Never Block", 0, 40, spacing: -22, textureName: "default grad");
        UI.Instance.DrawText(window, "End match", 0, 70, spacing: -22, textureName: this.pause_pointer == 3 ? "default purple" : "default");

        // Change option 
        if (InputManager.Instance.Key_down("Up") && this.pause_pointer > 0) {
            this.pause_pointer -= 1;
        } else if (InputManager.Instance.Key_down("Down") && this.pause_pointer < 3) {
            this.pause_pointer += 1;
        }

        // Do option
        if (this.pause_pointer == 0 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { // rematch
            this.showBoxs = !this.showBoxs;
        } else if (this.pause_pointer == 1 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { // MENU 
            this.debug_mode = !this.debug_mode;
        } else if (this.pause_pointer == 2 && this.debug_mode && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { // MENU 
            this.block_after_hit = !this.block_after_hit;
        } else if (this.pause_pointer == 3 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) {
            this.Pause();
            Program.winner = Program.Drawn;
            Program.sub_state = Program.MatchEnd;
            this.showBoxs = false;
            this.debug_mode = false;
            this.block_after_hit = false;
            this.pause_pointer = 0;
        }
    }
    public void Update(RenderWindow window) {
        if (hitstopCounter > 0) {
            hitstopCounter--;
        }
        if (!this.character_A.onHit) {
            this.character_B.comboCounter = 0;
        }
        if (!this.character_B.onHit) {
            this.character_A.comboCounter = 0;
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

        // Advance to the next frame
        CurrentAnimation.AdvanceFrame();
        if (this.CurrentAnimation.onLastFrame) {
            if (CurrentAnimation.changeOnLastframe) {
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
        foreach (Character char_object in this.OnSceneCharacters) char_object.DoRender(window, this.showBoxs);
        UI.Instance.DrawBattleUI(window, this);
        foreach (Character part_object in this.OnSceneParticles) part_object.DoRender(window, this.showBoxs);

        if (InputManager.Instance.Key_down("Start") && Program.sub_state == Program.Battling) {
            this.Pause();
        }

        if (debug_mode) this.DebugMode(window);
        if (pause) this.PauseScreen(window);
    }
    private void DoBehavior() {
        // Move characters away from border
        character_A.body.Position.X = Math.Max(character_A.push_box_width, Math.Min(character_A.body.Position.X, this.length - character_A.push_box_width));
        character_B.body.Position.X = Math.Max(character_B.push_box_width, Math.Min(character_B.body.Position.X, this.length - character_B.push_box_width));

        // Keep characters close 
        float deltaS = Math.Abs(character_A.body.Position.X - character_B.body.Position.X);
        if (deltaS >= Config.maxDistance) {
            if ((character_A.facing == 1 && character_A.body.Position.X < last_pos_A.X) || (character_A.facing == -1 && character_A.body.Position.X > last_pos_A.X)) {
                character_A.body.Position.X = this.last_pos_A.X;
            }
            if ((character_B.facing == 1 && character_B.body.Position.X < last_pos_B.X) || (character_B.facing == -1 && character_B.body.Position.X > last_pos_B.X)) {
                character_B.body.Position.X = this.last_pos_B.X;
            }
        }
        this.last_pos_A = this.character_A.body.Position;
        this.last_pos_B = this.character_B.body.Position;
        
        // Keep characters facing each other
        if (this.character_A.body.Position.X < this.character_B.body.Position.X) {
            if (this.character_A.notActing) this.character_A.facing = 1;
            if (this.character_B.notActing) this.character_B.facing = -1;
        } else {
            if (this.character_A.notActing) this.character_A.facing = -1;
            if (this.character_B.notActing) this.character_B.facing = 1;
        }

        this.checkColisions();
        this.doSpecialBehaviour();
    }
    public virtual void doSpecialBehaviour() {}
    public void DebugMode(RenderWindow window) {
        UI.Instance.ShowFramerate(window, "default");
        UI.Instance.DrawText(window, "training mode", 0, 70, spacing: -10, size: 0.5f, textureName: "default white");
        
        this.ResetRoundTime();
        
        if(hitstopCounter == 0) this.reset_frames += 1;

        // Block after hit
        if (this.character_B.StunFrames > 0) {
            if (this.block_after_hit) this.character_B.blocking = true;
            this.reset_frames = 0;
        } else if (this.character_A.StunFrames > 0) {
            if (this.block_after_hit) this.character_A.blocking = true;
            this.reset_frames = 0;
        }

        // Reset chars life, stun and super bar
        if (this.reset_frames >= Config.resetFrames) {
            if (this.character_B.notActing) {
                if (this.block_after_hit) this.character_B.blocking = false;
                this.character_B.LifePoints.X = this.character_B.LifePoints.Y;
                this.character_B.DizzyPoints.X = this.character_B.DizzyPoints.Y;
                this.character_B.SuperPoints.X = this.character_B.SuperPoints.Y;
            }
            if (this.character_A.notActing) {
                if (this.block_after_hit) this.character_A.blocking = false;
                this.character_A.LifePoints.X = this.character_A.LifePoints.Y;
                this.character_A.DizzyPoints.X = this.character_A.DizzyPoints.Y;
                this.character_A.SuperPoints.X = this.character_A.SuperPoints.Y;
            }
            this.reset_frames = Config.resetFrames;
        }
    }

    // Spawns
    public void spawnParticle(String state, float X, float Y, int facing = 1, int X_offset = 0, int Y_offset = 0) {
        var par = new Particle(state, X + X_offset * facing, Y + Y_offset, facing);
        par.animations = this.particle.animations;
        par.spriteImages = this.particle.spriteImages;
        par.characterSounds = this.particle.characterSounds;
        this.newParticles.Add(par);
    }
    public void spawnHitspark(int hit, float X, float Y, int facing, int X_offset = 0, int Y_offset = 0) {
        var hs = new Hitspark("default", X + X_offset * facing, Y + Y_offset, facing);
        if (hit == 1) {
            hs.CurrentState = "OnHit" + Program.random.Next(1, 4);
        } else if (hit == 0){
            hs.CurrentState = "OnBlock";
        } else {
            return;
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

                        } else if (!charA.hasHit && boxA.type == 0 && boxB.type == 1 && charA.team != charB.team && charA.type >= charB.type && GenericBox.Intersects(boxA, boxB, charA, charB)) { // A hit B
                            this.hitstopCounter = Config.hitStopTime;
                            var hit = charA.ImposeBehavior(charB);
                            charA.hasHit = true;
                            charA.comboCounter += hit == 1 ? 1 : 0;

                            // spawna a particula de hit
                            float x1 = boxA.getRealA(charA).X;
                            float y1 = boxA.getRealA(charA).Y;
                            float x2 = boxA.getRealB(charA).X;
                            float y2 = boxA.getRealB(charA).Y;
                            this.spawnHitspark(hit, (x1 + x2) / 2, (y1 + y2) / 2 + 125, charA.facing);
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
        this.character_A.body.Position.X = this.start_point_A;
        this.character_B.body.Position.X = this.start_point_B;
        this.character_A.stage = this;
        this.character_B.stage = this;

        this.OnSceneCharacters = new List<Character> {this.character_A, this.character_B};
        this.TogglePlayers();
    }
    public bool CheckRoundEnd() {
        if (this.hitstopCounter != 0) return false;
        
        bool doEnd = false;

        if (character_A.LifePoints.X <= 0 && character_A.CurrentState == "OnGround") {
            this.rounds_B += 1;
            doEnd = true;
        }
        if (character_B.LifePoints.X <= 0 && character_B.CurrentState == "OnGround") {
            this.rounds_A += 1;
            doEnd = true;
        }

        if (this.round_time == 0) {
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
        if (this.rounds_A == this.rounds_B && this.rounds_A >= Config.max_rounds) {
            Program.winner = Program.Drawn;
            return true;
        }
        else if (this.rounds_A >= Config.max_rounds) {
            Program.winner = Program.Player1;
            Program.player1_wins += 1;
            return true;
        }
        else if (this.rounds_B >= Config.max_rounds) {
            Program.winner = Program.Player2;
            Program.player2_wins += 1;
            return true;
        }
        
        return false;
    }
    public void ResetRoundTime() {
        this.elapse_time = true;
        this.stopwatch.Reset();
    }
    public void StartRoundTime() {
        this.elapse_time = true;
        this.stopwatch.Start();
    }
    public void StopRoundTime() {
        this.elapse_time = false;
        this.stopwatch.Stop();
    }
    public void PauseRoundTime() {
        if (stopwatch.IsRunning) stopwatch.Stop();
        else stopwatch.Start();
    }
    public void ResetMatch() {
        this.rounds_A = 0;
        this.rounds_B = 0;
    }
    public void ResetPlayers(bool force = false, bool total_reset = false) {
        if (force) {
            this.character_A.Reset(this.start_point_A, facing: 1, state: "Intro", total_reset: total_reset);
            this.character_B.Reset(this.start_point_B, facing: -1, state: "Intro", total_reset: total_reset);
        } else {
            this.character_A.Reset(this.start_point_A, facing: 1, total_reset: total_reset);
            this.character_B.Reset(this.start_point_B, facing: -1, total_reset: total_reset);
        }
    }
    public void TogglePlayers() {
        this.character_A.behave = !this.character_A.behave;
        this.character_B.behave = !this.character_B.behave;
    }
    public void Pause() {
        this.pause = !this.pause;
        this.TogglePlayers();
        this.PauseRoundTime();
        this.PauseTimer();
        foreach (Character char_object in this.OnSceneCharacters) char_object.animate = !char_object.animate;
        foreach (Character part_object in this.OnSceneParticles) part_object.animate = ! part_object.animate;
    }
    public void ResetTimer() {
        this.timer.Restart();
    }
    public bool CheckTimer(double elapsed_time) {
        return elapsed_time <= this.timer.Elapsed.Seconds;
    }
    public void PauseTimer() {
        if (this.timer.IsRunning) this.timer.Stop();
        else this.timer.Start();
    }
    public void SetHitstop(int amount) {
        this.hitstopCounter = amount;
    }
    public void StopMusic() {
        if (this.stageSounds.Keys.Contains("music")) {
            this.stageSounds["music"].Stop();
        }
    }
    public void PlayMusic() {
        if (this.stageSounds.Keys.Contains("music") && this.stageSounds["music"].Status == SoundStatus.Stopped){
            this.stageSounds["music"].Volume = Config.Music_Volume;
            this.stageSounds["music"].Play();
        }
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
    public void UnloadCharacters() {
        this.character_A.Unload();
        this.character_B.Unload();

        this.spark.Unload();
        this.fireball.Unload();
        this.particle.Unload();
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
                this.spriteImages[fileNameWithoutExtension] = sprite;
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
            } catch {
                // Console.WriteLine($"Falha ao carregar o som {file}: {ex.Message}");
            }
        }
    }
    public void UnloadSounds() {
        this.StopMusic();
        foreach (var sound in this.stageSounds.Values)
        {
            sound.Dispose(); // Free the memory used by the image
        }
        this.stageSounds.Clear(); 
    }

    public virtual void LoadStage() {}
    public void UnloadStage() {
        this.ResetMatch();
        this.ResetRoundTime();
        this.ResetPlayers();
        this.ResetTimer();
        this.UnloadSounds();
        this.UnloadSpriteImages();
        this.UnloadCharacters();

    }

}

}