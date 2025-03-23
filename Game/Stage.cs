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
    public bool show_boxs = false;
    public bool block_after_hit = false;
    public bool refil_life = true;
    public bool refil_super = true;
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
    public Dictionary<string, Animation> animations;
    private Dictionary<string, Sprite> spriteImages;
    private Dictionary<string, Sound> stageSounds;
    public string CurrentSprite = "";
    public string CurrentSound = "";
    public Animation CurrentAnimation => animations[CurrentState];
    public int CurrentFrameIndex => animations[CurrentState].currentFrameIndex;

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
        this.spriteImages = new Dictionary<string, Sprite>();
        this.stageSounds = new Dictionary<string, Sound>();

        this.spark = new Hitspark("Default", 0, 0, 1, this);
        this.fireball = new Fireball("Default", 0, 0, 0, 1, this);
        this.particle = new Particle("Default", 0, 0, 1, this);

        this.thumb = new Sprite(new Texture(thumbPath));

        this.timer = new Stopwatch();
        this.matchTimer = new Stopwatch();
    }

    // Behaviour
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

        // Pause
        if (InputManager.Instance.Key_down("Start") && Program.sub_state == Program.Battling) {
            this.Pause();
        }

        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentSimpleFrame();

        // Render stage sprite
        if (this.spriteImages.ContainsKey(this.CurrentSprite)) {
            Sprite temp_sprite = this.spriteImages[this.CurrentSprite];
            temp_sprite.Scale = new Vector2f(this.size_ratio, this.size_ratio);
            temp_sprite.Position = new Vector2f (0, 0);
            window.Draw(temp_sprite);
        }

        // Keep music playing
        if (!this.pause) this.PlayMusic();

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

        // Render chars, UI, shadows and particles
        foreach (Character char_object in this.OnSceneCharacters) {
            this.DrawShadow(window, char_object);
            char_object.DoRender(window, this.show_boxs);
        }
        UI.Instance.DrawBattleUI(window, this);
        foreach (Character part_object in this.OnSceneParticles) {
            part_object.DoRender(window, this.show_boxs);
        }
        
        // Render Pause menu and Traning assets
        if (this.debug_mode) this.DebugMode(window);
        if (this.pause) this.PauseScreen(window);
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

        this.CheckColisions();
        this.DoSpecialBehaviour();
    }
    public virtual void DoSpecialBehaviour() {}
    public void DebugMode(RenderWindow window) {
        UI.Instance.ShowFramerate(window, "default small white");
        UI.Instance.DrawText(window, "training mode", 0, 70, spacing: Config.spacing_small, size: 1f, textureName: "default small white");
        
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
    public void PauseScreen(RenderWindow window) {
        fade90.Position = new Vector2f(Program.camera.X - Config.RenderWidth/2, Program.camera.Y - Config.RenderHeight/2);
        window.Draw(fade90);

        // Draw options
        UI.Instance.DrawText(window, "Pause", 0, -75, size: 1f, spacing: Config.spacing_medium, textureName: "default medium");
        UI.Instance.DrawText(window, "Settings", 0, -40, spacing: Config.spacing_medium, textureName: this.pause_pointer == 0 ? "default medium hover" : "default medium");
        UI.Instance.DrawText(window, "Training mode", 0, -20, spacing: Config.spacing_medium, textureName: this.pause_pointer == 1 ? "default medium hover" : "default medium");
        if (debug_mode) UI.Instance.DrawText(window, "Show hitboxes", 0, 0, spacing: Config.spacing_small, textureName: this.pause_pointer == 2 ? "default small hover" : "default small");
        if (debug_mode) UI.Instance.DrawText(window, block_after_hit ? "Block: after hit" : "Block: never", 0, 10, spacing: Config.spacing_small, textureName: this.pause_pointer == 3 ? "default small hover" : "default small");
        if (debug_mode) UI.Instance.DrawText(window, refil_life ? "Life: refil" : "Life: keep", 0, 20, spacing: Config.spacing_small, textureName: this.pause_pointer == 4 ? "default small hover" : "default small");
        if (debug_mode) UI.Instance.DrawText(window, refil_super ? "Super: refil" : "Super: keep", 0, 30, spacing: Config.spacing_small, textureName: this.pause_pointer == 5 ? "default small hover" : "default small");
        UI.Instance.DrawText(window, "End match", 0, 70, spacing: Config.spacing_medium, textureName: this.pause_pointer == 6 ? "default medium red" : "default medium");

        // Change option 
        if (InputManager.Instance.Key_down("Up") && this.pause_pointer > 0) {
            this.pause_pointer -= 1;
            if (!debug_mode && this.pause_pointer < 5 && this.pause_pointer > 1) this.pause_pointer = 1;
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
        par.animations = this.particle.animations;
        par.spriteImages = this.particle.spriteImages;
        par.characterSounds = this.particle.characterSounds;
        this.newParticles.Add(par);
    }
    public void spawnHitspark(int hit, float X, float Y, int facing, int X_offset = 0, int Y_offset = 0) {
        var hs = new Hitspark("default", X + X_offset * facing, Y + Y_offset, facing);
        if (hit == 2) {
            hs.CurrentState = "Parry";
        } else if (hit == 1) {
            hs.CurrentState = "Hit" + Program.random.Next(1, 4);
        } else if (hit == 0){
            hs.CurrentState = "Block";
        } else {
            return;
        }
        hs.animations = this.spark.animations;
        hs.spriteImages = this.spark.spriteImages;
        hs.characterSounds = this.spark.characterSounds;
        this.newParticles.Add(hs);
    }
    public Fireball spawnFireball(string state, float X, float Y, int facing, int team, int X_offset = 10, int Y_offset = 0) {
        var fb = new Fireball(state, X + X_offset * facing, Y + Y_offset, team, facing, this);
        fb.animations = this.fireball.animations;
        fb.spriteImages = this.fireball.spriteImages;
        fb.characterSounds = this.fireball.characterSounds;
        this.newCharacters.Add(fb);
        return fb;
    }

    // Visuals
    public void DrawShadow(RenderWindow window, Character char_obj) {
        if (char_obj.shadow_size != -1) {
            this.shadow.Texture = this.shadows[char_obj.shadow_size];
            this.shadow.Position = new Vector2f(char_obj.body.Position.X - this.shadow.GetLocalBounds().Width/2, this.floorLine - this.shadow.GetLocalBounds().Height/2 - 55 );
            this.shadow.Color = this.AmbientLight;
            window.Draw(this.shadow);
        }
    }

    // Auxiliary
    public void CheckColisions() {
        // Aleatoriza a lista para não dar vantagem sempre a um mesmo char
        var OnSceneCharactersRandom = this.OnSceneCharacters.OrderBy(x => Program.random.Next()).ToList();

        for (int i = 0; i < OnSceneCharactersRandom.Count(); i++) {
            for (int j = 0; j < OnSceneCharactersRandom.Count(); j++) {
                if (i == j) continue;
                var charA = OnSceneCharactersRandom[i];
                var charB = OnSceneCharactersRandom[j];
                foreach (GenericBox boxA in charA.CurrentBoxes) {
                    foreach (GenericBox boxB in charB.CurrentBoxes) {
                        if (boxA.type == 2 && boxB.type == 2 && GenericBox.Intersects(boxA, boxB, charA, charB)) {
                            // Colisão física
                            GenericBox.Colide(boxA, boxB, charA, charB);

                        } else if (!charA.hasHit && boxA.type == 0 && boxB.type == 1 && charA.team != charB.team && charA.type >= charB.type && GenericBox.Intersects(boxA, boxB, charA, charB)) { // A hit B
                            this.hitstopCounter = Config.hitStopTime;

                            charA.hasHit = true; // isso tava abaixo do behaviour, não sei se tava certo. 
                            var hit = charA.ImposeBehavior(charB);

                            // Soma o contador de combo do time
                            if (charA.team == 0) this.character_A.comboCounter += hit == 1 ? 1 : 0;
                            else this.character_B.comboCounter += hit == 1 ? 1 : 0;

                            // spawna a particula de hit
                            this.spawnHitspark(hit, (boxA.getRealA(charA).X + boxA.getRealB(charA).X) / 2, (boxA.getRealA(charA).Y + boxA.getRealB(charA).Y) / 2 + 125, charA.facing);
                        }
                    }
                }
            }
        }
    }
    public void SetChars(Character char_A, Character char_B) {
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

        this.character_A.LightTint = this.AmbientLight;
        this.character_B.LightTint = this.AmbientLight;

        this.OnSceneCharacters = new List<Character> {this.character_A, this.character_B};
        this.LockPlayers();
    }
    public bool CheckRoundEnd() {
        if (this.hitstopCounter != 0) return false;
        
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
        this.ToggleMusicVolume(this.pause, volume_A: 10f);
        foreach (Character char_object in this.OnSceneCharacters) char_object.animate = !char_object.animate;
        foreach (Character part_object in this.OnSceneParticles) part_object.animate = ! part_object.animate;
    }
    public void SetHitstop(int amount) {
        this.hitstopCounter = amount;
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
        if (this.stageSounds.Keys.Contains("music")) {
            this.stageSounds["music"].Volume = amount * (Config.Main_Volume / 100);
        }
    }
    public void StopMusic() {
        if (this.stageSounds.Keys.Contains("music")) {
            this.stageSounds["music"].Stop();
        }
    }
    public void PauseMusic() {
        if (this.stageSounds.Keys.Contains("music")) {
            this.stageSounds["music"].Pause();
        }
    }
    public void PlayMusic() {
        if (this.stageSounds.Keys.Contains("music") && (this.stageSounds["music"].Status == SoundStatus.Stopped || this.stageSounds["music"].Status == SoundStatus.Paused)){
            this.stageSounds["music"].Play();
        }
    }
    public void ToggleMusic() {
        if (this.stageSounds["music"].Status == SoundStatus.Paused) this.PlayMusic();
        else this.PauseMusic();
    }
    public void ToggleMusicVolume(bool control, float volume_A = -1, float volume_B = -1) {
        if (control) this.SetMusicVolume(volume_A);
        else this.SetMusicVolume(volume_B);
    }
    
    // Loads
    public void LoadCharacters(int charA_index, int charB_index) {        
        var charA = Character.SelectCharacter(charA_index, this);
        var charB = Character.SelectCharacter(charB_index, this);

        this.SetChars(charA, charB);

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

        return true;
    }
    public void UnloadSpriteImages() {
        foreach (var image in this.spriteImages.Values)
        {
            image.Dispose(); // Free the memory used by the image
        }
        this.spriteImages.Clear(); // Clear the dictionary
    }
    public bool LoadSounds() {
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullSoundPath = Path.Combine(currentDirectory, this.soundFolderPath);

        // Verifica se o diretório existe
        if (!System.IO.Directory.Exists(fullSoundPath)) {
            return false;
        }

        // Obtém todos os arquivos no diretório especificado
        try {
            string[] files = System.IO.Directory.GetFiles(fullSoundPath);

            foreach (string file in files) {
                // Tenta carregar a textura
                var buffer = new SoundBuffer(file);
                
                // Obtém o nome do arquivo sem a extensão
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);

                // Adiciona no dicionário
                this.stageSounds[fileNameWithoutExtension] = new Sound(buffer);
            } 
        } catch {}

        return true;
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