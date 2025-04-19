using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Character_Space;
using UI_space;
using Input_Space;
using System.Diagnostics;
using Data_space;

namespace Stage_Space {

public class Stage {
    // Basic Infos
    public string name = "";
    public string spritesFolderPath;
    public string soundFolderPath;
    public Sprite thumb;

    // Debug infos
    public bool debug_mode = false;
    public bool pause = false;
    public bool show_boxs = false;
    public bool block_after_hit = false;
    public bool refil_life = true;
    public bool refil_super = true;
    public int reset_frames = 0;

    // Battle Info
    public List<Character> OnSceneCharacters = new List<Character> {};
    public List<Character> OnSceneCharactersSorted => this.OnSceneCharacters
            .OrderByDescending(x => x.State.priority)
            .ThenBy(x => Program.random.Next())
            .ToList();
    public List<Character> OnSceneCharactersRender => this.OnSceneCharacters
            .OrderBy(x => x.State.priority)
            .ToList();
    public List<Character> OnSceneParticles = new List<Character> {};
    public List<Character> newCharacters = new List<Character> {};
    public List<Character> newParticles = new List<Character> {};

    public Character character_A;
    public Character character_B;
    public Vector2f last_pos_A => character_A.body.LastPosition;
    public Vector2f last_pos_B => character_B.body.LastPosition;
    public int rounds_A;
    public int rounds_B;
    public int elapsed_time => this.matchTimer.Elapsed.Seconds;
    public int round_time => elapse_time ? Config.RoundLength - (int) matchTimer.Elapsed.TotalSeconds : Config.RoundLength;
    public double raw_round_time => elapse_time ? Config.RoundLength - this.matchTimer.Elapsed.TotalMilliseconds/1000 : Config.RoundLength;
    public bool elapse_time = true;

    // Technical infos
    public int floorLine;
    public int length;
    public int height;
    public int start_point_A;
    public int start_point_B;
    public Vector2f center_point => new Vector2f(length / 2, height / 2);

    // Timers
    private Stopwatch timer;
    private Stopwatch matchTimer;

    // Aux
    private int pause_pointer = 0;

    // Pre-renders
    private Hitspark spark; 
    private Fireball fireball;
    private Particle particle;

    // Animation infos
    public string CurrentState { get; set; }
    public string LastState { get; set; }
    public Dictionary<string, State> states;
    private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
    private Dictionary<string, SoundBuffer> sounds = new Dictionary<string, SoundBuffer>();
    public string CurrentSprite => CurrentAnimation.GetCurrentSimpleFrame();
    public Sound music;
    public State state => states[CurrentState];
    public Animation CurrentAnimation => states[CurrentState].animation;
    public int CurrentFrameIndex => states[CurrentState].animation.current_frame_index;

    // Visual info
    public Color AmbientLight = new Color(255, 255, 255, 255);
    private Sprite fade90 = new Sprite(new Texture("Assets/ui/90fade.png"));
    private Texture[] shadows;
    private Sprite shadow;

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

        this.spark = new Hitspark("Default", 0, 0, 1, this);
        this.fireball = new Fireball("Default", 1, 0, 0, 0, 1, this);
        this.particle = new Particle("Default", 0, 0, 1, this);

        this.thumb = new Sprite(new Texture(thumbPath));

        this.timer = new Stopwatch();
        this.matchTimer = new Stopwatch();
    }
    public Stage(string name, string thumbPath) {
        this.name = name;
        this.thumb = new Sprite(new Texture(thumbPath));
    }

    // Behaviour
    public void Update() {
        if (!this.character_A.onHit) this.character_B.comboCounter = 0;
        if (!this.character_B.onHit) this.character_A.comboCounter = 0;

        // Pause
        if (InputManager.Instance.Key_down("Start") && Program.sub_state == Program.Battling) this.Pause();

        // Render stage sprite
        if (this.textures.ContainsKey(this.CurrentSprite)) {
            Sprite temp_sprite = new Sprite(this.textures[this.CurrentSprite]);
            temp_sprite.Position = new Vector2f (0, 0);
            Program.window.Draw(temp_sprite);
        }

        // Advance to the next frame
        CurrentAnimation.AdvanceFrame();
        if (this.CurrentAnimation.ended && state.change_on_end) {
            if (states.ContainsKey(this.state.post_state)) {
                this.LastState = this.CurrentState;
                this.CurrentState = this.state.post_state;
                if (CurrentState != LastState) this.states[LastState].animation.Reset();
            }
        }

        // Keep music playing
        if (!this.pause) this.PlayMusic();

        // Render 
        foreach (Character char_object in this.OnSceneCharactersRender) {
            this.DrawShadow(char_object);
            char_object.Render(this.show_boxs);
        }
        UI.Instance.DrawBattleUI(this);
        foreach (Character part_object in this.OnSceneParticles) part_object.Render(this.show_boxs);

        // Update chars
        foreach (Character char_object in this.OnSceneCharactersSorted) char_object.Update();
        this.OnSceneCharacters.RemoveAll(obj => obj.remove);
        this.OnSceneCharacters.AddRange(this.newCharacters);
        this.newCharacters.Clear();
        this.DoBehavior();

        // Update particles
        foreach (Character part_object in this.OnSceneParticles) part_object.Update();
        this.OnSceneParticles.RemoveAll(obj => obj.remove);
        this.OnSceneParticles.AddRange(this.newParticles);
        this.newParticles.Clear();
        
        // Render Pause menu and Traning assets
        if (this.debug_mode) this.DebugMode();
        if (this.pause) this.PauseScreen();
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
        
        // Keep characters facing each other
        if (this.character_A.body.Position.X < this.character_B.body.Position.X) {
            if (this.character_A.notActing) this.character_A.facing = 1;
            if (this.character_B.notActing) this.character_B.facing = -1;
        } else {
            if (this.character_A.notActing) this.character_A.facing = -1;
            if (this.character_B.notActing) this.character_B.facing = 1;
        }
        
        this.DoSpecialBehaviour();
    }
    public virtual void DoSpecialBehaviour() {}
    public void DebugMode() {
        UI.Instance.ShowFramerate("default small white");
        UI.Instance.DrawText("training mode", 0, 70, spacing: Config.spacing_small, size: 1f, textureName: "default small white");
        
        this.ResetRoundTime();
        
        if(this.character_A.hitstopCounter == 0 && this.character_B.hitstopCounter == 0) this.reset_frames += 1;

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
                if (this.refil_life){
                    this.character_B.LifePoints.X = this.character_B.LifePoints.Y;
                    this.character_B.DizzyPoints.X = this.character_B.DizzyPoints.Y;
                } 
                if (this.refil_super) this.character_B.SuperPoints.X = this.character_B.SuperPoints.Y;
            }
            if (this.character_A.notActing) {
                if (this.block_after_hit) this.character_A.blocking = false;
                if (this.refil_life){
                    this.character_A.LifePoints.X = this.character_A.LifePoints.Y;
                    this.character_A.DizzyPoints.X = this.character_A.DizzyPoints.Y;
                } 
                if (this.refil_super) this.character_A.SuperPoints.X = this.character_A.SuperPoints.Y;
            }
            this.reset_frames = Config.resetFrames;
        }
    }
    public void PauseScreen() {
        fade90.Position = new Vector2f(Program.camera.X - Config.RenderWidth/2, Program.camera.Y - Config.RenderHeight/2);
        Program.window.Draw(fade90);

        // Draw options
        UI.Instance.DrawText("Pause", 0, -75, size: 1f, spacing: Config.spacing_medium, textureName: "default medium");
        UI.Instance.DrawText("Settings", 0, -40, spacing: Config.spacing_medium, textureName: this.pause_pointer == 0 ? "default medium hover" : "default medium");
        UI.Instance.DrawText("Training mode", 0, -20, spacing: Config.spacing_medium, textureName: this.pause_pointer == 1 ? "default medium hover" : "default medium");
        if (debug_mode) UI.Instance.DrawText("Show hitboxes", 0, 0, spacing: Config.spacing_small, textureName: this.pause_pointer == 2 ? "default small hover" : "default small");
        if (debug_mode) UI.Instance.DrawText(block_after_hit ? "Block: after hit" : "Block: never", 0, 10, spacing: Config.spacing_small, textureName: this.pause_pointer == 3 ? "default small hover" : "default small");
        if (debug_mode) UI.Instance.DrawText(refil_life ? "Life: refil" : "Life: keep", 0, 20, spacing: Config.spacing_small, textureName: this.pause_pointer == 4 ? "default small hover" : "default small");
        if (debug_mode) UI.Instance.DrawText(refil_super ? "Super: refil" : "Super: keep", 0, 30, spacing: Config.spacing_small, textureName: this.pause_pointer == 5 ? "default small hover" : "default small");
        UI.Instance.DrawText("End match", 0, 70, spacing: Config.spacing_medium, textureName: this.pause_pointer == 6 ? "default medium red" : "default medium");

        // Change option 
        if (InputManager.Instance.Key_down("Up") && this.pause_pointer > 0) {
            this.pause_pointer -= 1;
            if (!debug_mode && this.pause_pointer < 6 && this.pause_pointer > 1) this.pause_pointer = 1;
        } else if (InputManager.Instance.Key_down("Down") && this.pause_pointer < 6) {
            this.pause_pointer += 1;
            if (!debug_mode && this.pause_pointer < 5 && this.pause_pointer > 1) this.pause_pointer = 6;
        }

        // Do option
        if (this.pause_pointer == 0 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { 
            Program.return_state = Program.game_state;
            Program.game_state = Program.Settings;

        } else if (this.pause_pointer == 1 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { 
            this.debug_mode = !this.debug_mode;

        }  else if (this.pause_pointer == 2 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { 
            this.show_boxs = !this.show_boxs;

        } else if (this.pause_pointer == 3 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { 
            this.block_after_hit = !this.block_after_hit;

        } else if (this.pause_pointer == 4 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) {
            this.refil_life = !this.refil_life;

        } else if (this.pause_pointer == 5 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { 
            this.refil_super = !this.refil_super;

        } else if (this.pause_pointer == 6 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) {  
            this.Pause();
            Program.winner = Program.Drawn;
            Program.sub_state = Program.MatchEnd;
            this.show_boxs = false;
            this.debug_mode = false;
            this.block_after_hit = false;
            this.refil_life = true;
            this.refil_super = true;
            this.pause_pointer = 0;
        } 
    }

    // Spawns
    public void spawnParticle(String state, float X, float Y, int facing = 1, int X_offset = 0, int Y_offset = 0) {
        var par = new Particle(state, X + X_offset * facing, Y + Y_offset, facing);
        par.states = this.particle.states;
        this.newParticles.Add(par);
    }
    public void spawnHitspark(int hit, float X, float Y, int facing, int X_offset = 0, int Y_offset = 0) {
        string state;
        if (hit == Character.PARRY) {
            state = "Parry";
        } else if (hit == Character.HIT) {
            state = "Hit" + Program.random.Next(1, 4);
        } else if (hit == Character.BLOCK){
            state = "Block";
        } else {
            return;
        }
        var hs = new Hitspark(state, X + X_offset * facing, Y + Y_offset, facing);
        hs.states = this.spark.states;
        this.newParticles.Add(hs);
    }
    public Fireball spawnFireball(string state, float X, float Y, int facing, int team, int life_points = 1, int X_offset = 10, int Y_offset = 0) {        
        var fb = new Fireball(state, life_points, X + X_offset * facing, Y + Y_offset, team, facing, this);
        fb.states = this.fireball.states;
        this.newCharacters.Add(fb);
        return fb;
    }

    // Visuals
    public void DrawShadow(Character char_obj) {
        if (char_obj.shadow_size != -1) {
            this.shadow.Texture = this.shadows[char_obj.shadow_size];
            this.shadow.Position = new Vector2f(char_obj.body.Position.X - this.shadow.GetLocalBounds().Width/2, this.floorLine - this.shadow.GetLocalBounds().Height/2 - 55 );
            this.shadow.Color = this.AmbientLight;
            Program.window.Draw(this.shadow);
        }
    }

    // Auxiliary
    public bool CheckRoundEnd() {
        if (this.debug_mode || this.pause ) return false;
        
        bool doEnd = false;

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

        if (character_A.LifePoints.X <= 0 && character_A.CurrentState == "OnGround") {
            this.rounds_B += 1;
            doEnd = true;
        }
        if (character_B.LifePoints.X <= 0 && character_B.CurrentState == "OnGround") {
            this.rounds_A += 1;
            doEnd = true;
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
    public void ResetMatch() {
        this.rounds_A = 0;
        this.rounds_B = 0;
    }
    public void Pause() {
        this.pause = !this.pause;
        this.TogglePlayers();
        this.PauseRoundTime();
        this.PauseTimer();
        this.ToggleMusicVolume(this.pause, volume_A: 20f);
        foreach (Character char_object in this.OnSceneCharacters) char_object.animate = !char_object.animate;
        foreach (Character part_object in this.OnSceneParticles) part_object.animate = ! part_object.animate;
    }
    public void Hitstop(string amount, bool parry, Character character) {
        if (parry) {
            this.StopFor(Config.hitStopTime + Config.parry_advantage);

            switch (amount) {
                case "Light":
                    character.hitstopCounter = Config.hitStopTime * 1/2;
                    break;

                case "Medium":
                    character.hitstopCounter = Config.hitStopTime * 2/3;
                    break;

                case "Heavy":
                    character.hitstopCounter = Config.hitStopTime;
                    break;

                default:
                    break;
            }
        } else {
            switch (amount) {
                case "Light":
                    this.StopFor(Config.hitStopTime * 1/2);
                    break;

                case "Medium":
                    this.StopFor(Config.hitStopTime * 2/3);
                    break;

                case "Heavy":
                    this.StopFor(Config.hitStopTime);
                    break;

                default:
                    break;
            }
        }
    }
    public void StopFor(int frame_amount) {
        foreach (var entity in this.OnSceneCharacters) entity.hitstopCounter = frame_amount;
    }

    // Round Time
    public void ResetRoundTime() {
        this.elapse_time = true;
        this.matchTimer.Reset();
    }
    public void StartRoundTime() {
        this.elapse_time = true;
        this.matchTimer.Start();
    }
    public void StopRoundTime() {
        this.elapse_time = false;
        this.matchTimer.Stop();
    }
    public void PauseRoundTime() {
        if (matchTimer.IsRunning) matchTimer.Stop();
        else matchTimer.Start();
    }

    // Players
    public void SetChars(Character char_A, Character char_B) {
        this.character_A = char_A;
        this.character_A.facing = 1;
        this.character_A.playerIndex = 1;

        this.character_B = char_B;
        this.character_B.facing = -1;
        this.character_B.playerIndex = 2;

        this.character_A.floorLine = this.floorLine;
        this.character_B.floorLine = this.floorLine;
        this.character_A.body.Position.X = this.start_point_A;
        this.character_B.body.Position.X = this.start_point_B;
        this.character_A.stage = this;
        this.character_B.stage = this;

        this.character_A.LightTint = this.AmbientLight;
        this.character_B.LightTint = this.AmbientLight;

        this.OnSceneCharacters = new List<Character> {this.character_A, this.character_B};
        this.LockPlayers();
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
    public void ReleasePlayers() {
        this.character_A.behave = true;
        this.character_B.behave = true;
    }
    public void LockPlayers() {
        this.character_A.behave = false;
        this.character_B.behave = false;
    }

    // Timer
    public void ResetTimer() {
        this.timer.Restart();
    }
    public bool CheckTimer(double elapsed_time) {
        return elapsed_time <= this.timer.Elapsed.TotalMilliseconds/1000;
    }
    public void PauseTimer() {
        if (this.timer.IsRunning) this.timer.Stop();
        else this.timer.Start();
    }
    public double GetTimerValue() {
        return this.timer.Elapsed.TotalMilliseconds/1000;
    }
    
    // Music
    public void SetMusicVolume(float amount = -1) {
        if (amount == -1) amount = Config.Music_Volume;
        this.music.Volume = amount * (Config.Main_Volume / 100);
    }
    public void StopMusic() {
        this.music.Stop();
    }
    public void PauseMusic() {
        this.music.Pause();
    }
    public void PlayMusic() {
        if (this.music.Status == SoundStatus.Stopped || this.music.Status == SoundStatus.Paused){
            this.music.Play();
        }
    }
    public void ToggleMusic() {
        if (this.music.Status == SoundStatus.Paused) this.PlayMusic();
        else this.PauseMusic();
    }
    public void ToggleMusicVolume(bool control, float volume_A = -1, float volume_B = -1) {
        if (control) this.SetMusicVolume(volume_A);
        else this.SetMusicVolume(volume_B);
    }
    
    // Loads
    public void LoadCharacters(string charA_name, string charB_name) {        
        var charA = Character.SelectCharacter(charA_name, this);
        var charB = Character.SelectCharacter(charB_name, this);

        this.SetChars(charA, charB);
        
        this.spark.Load();
        this.fireball.Load();
        this.particle.Load();
    }
    public void UnloadCharacters() {
        this.character_A = null;
        this.character_B = null;

        this.spark = null;
        this.fireball = null;
        this.particle = null;
    }
    public bool LoadSpriteImages() {
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullPath = Path.Combine(currentDirectory, this.spritesFolderPath);

        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(fullPath)) {
            return false;
        }

        // Load shadows textures
        this.shadows = new Texture[3];
        for (int i = 0; i < 3; i++) {
            this.shadows[i] = new Texture("Assets/characters/Default/Sprites/shadow" + i + ".png");
        }
        this.shadow = new Sprite(this.shadows[0]);

        // Verifica se o arquivo binário existe, senão, carrega as texturas e cria ele
        string datpath = Path.Combine(fullPath, "visuals.dat");
        if (System.IO.File.Exists(datpath)) {
            this.textures = DataManagement.LoadTextures(datpath);
            
        } else {
            // Obtém todos os arquivos no diretório especificado
            string[] files = System.IO.Directory.GetFiles(fullPath);
            foreach (string file in files) {
                // Tenta carregar a textura
                Texture texture = new Texture(file);
                
                // Obtém o nome do arquivo sem a extensão
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);
                
                // Usa o nome do arquivo sem extensão como chave no dicionário
                this.textures[fileNameWithoutExtension] = texture;
            }

            // Salva o arquivo binário
            DataManagement.SaveTextures(datpath, this.textures);
        }

        return true;
    }
    public void UnloadSpriteImages() {
        foreach (var image in this.textures.Values)
        {
            image.Dispose(); // Free the memory used by the image
        }
        this.textures.Clear(); // Clear the dictionary
    }
    public bool LoadSounds() {
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullSoundPath = Path.Combine(currentDirectory, this.soundFolderPath);

        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(fullSoundPath)) {
            return false;
        }

        // Verifica se o arquivo binário existe, senão, carrega as texturas e cria ele
        string datpath = Path.Combine(fullSoundPath, "sounds.dat");
        if (System.IO.File.Exists(datpath)) {
            this.sounds = DataManagement.LoadSounds(datpath);
            
        } else {
            // Obtém todos os arquivos no diretório especificado
            string[] files = System.IO.Directory.GetFiles(fullSoundPath);
            foreach (string file in files) {
                // Tenta carregar a textura
                var buffer = new SoundBuffer(file);
                
                // Obtém o nome do arquivo sem a extensão
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);

                // Adiciona no dicionário
                this.sounds[fileNameWithoutExtension] = buffer;
            } 

            // Salva o arquivo binário
            DataManagement.SaveSounds(datpath, this.sounds);
        }

        // setta a musica
        this.music = new Sound(sounds["music"]);
        
        return true;
    }
    public void UnloadSounds() {
        this.StopMusic();
        foreach (var sound in this.sounds.Values)
        {
            sound.Dispose(); // Free the memory used by the image
        }
        this.sounds.Clear(); 
    }

    public virtual void LoadStage() {}
    public void UnloadStage() {
        this.ResetMatch();
        this.ResetRoundTime();
        this.ResetPlayers();
        this.ResetTimer();
        this.UnloadCharacters();
    }

}

}