using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using Animation_Space;
using Input_Space;
using Stage_Space;
using UI_space;

// ----- Default States -------
// Intro
// Idle
// WalkingForward
// WalkingBackward
// JumpForward
// Jump
// JumpBackward
// JumpFalling
// Crouching
// OnHit
// OnHitLow
// OnBlock
// OnBlockLow
// Airboned
// Falling
// OnGround
// Wakeup

namespace Character_Space {
public class Character : Object_Space.Object {
    // Infos
    public string name;
    public int type;
    public string folderPath;
    public string soundFolderPath;
    public float floorLine;
    public int team;
    public Stage stage;

    // Controls
    public int playerIndex { get; set; }

    // Statistics 
    public Vector2i LifePoints = new Vector2i(1000, 1000);
    public Vector2i DizzyPoints = new Vector2i(500, 500);
    public Vector2i SuperPoints = new Vector2i(0, 100);
    public int StunFrames = 0;
    public int move_speed = 0;
    public int dash_speed = 0;
    public int jump_hight = 79;
    public int push_box_width = 0;

    // Object infos
    public Vector2f VisualPosition => new Vector2f(this.body.Position.X - 125, this.body.Position.Y - 250);
    public string CurrentState { get; set; }
    private string LastState { get; set; }

    // Combat logic infos
    public bool notActing => this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward" || this.CurrentState == "Crouching" || this.CurrentState == "CrouchingIn" || this.CurrentState == "CrouchingOut" || (this.CurrentState == "DashForward" && this.CurrentAnimation.onLastFrame) || (this.CurrentState == "DashBackward" && this.CurrentAnimation.onLastFrame);
    public bool notActingAir => (this.CurrentState == "Jump" || this.CurrentState == "JumpForward" || this.CurrentState == "JumpBackward") && this.body.Position.Y < this.floorLine;
    public bool isCrounching = false;
    public bool onAir => this.body.Position.Y < this.floorLine;
    public bool hasHit = false; 
    public bool onHit => this.CurrentState.Contains("Airboned") || this.CurrentState.Contains("OnHit");

    public bool blockingHigh = false;
    public bool blockingLow = false;
    public bool blocking = false;

    public int comboCounter = 0;
    public float damageScaling => Math.Max(0.1f, 1 - comboCounter * 0.1f);

    // Data
    public Dictionary<string, Animation> animations = new Dictionary<string, Animation>{};
    public Dictionary<int, Sprite> spriteImages = new Dictionary<int, Sprite>{};
    public Dictionary<string, Sound> characterSounds = new Dictionary<string, Sound>{};
    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    // Gets
    public int CurrentSprite;
    public string CurrentSound = "";
    public Animation CurrentAnimation => animations[CurrentState];
    public int CurrentFrameIndex => animations[CurrentState].currentFrameIndex;
    public bool hasFrameChange => animations[CurrentState].hasFrameChange;
    public int lastFrameIndex = -1;

    public Character(string name, string initialState, float startX, float startY, string folderPath, string soundFolderPath, Stage stage, int type = 0) : base() {
        this.folderPath = folderPath;
        this.soundFolderPath = soundFolderPath;
        this.name = name;
        this.type = type;
        this.CurrentState = initialState;
        this.LastState = initialState;
        this.stage = stage;
        base.body.Position.X = startX; 
        base.body.Position.Y = startY;
        this.floorLine = startY;
        this.spriteImages = new Dictionary<int, Sprite>();
        this.characterSounds = new Dictionary<string, Sound>();
    }

    // Every Frame methods
    public override void Update() {
        base.Update();
        this.DoBehave();
        this.DoAnimate();
    }
    public override void DoRender(RenderWindow window, bool drawHitboxes = false) {
        base.DoRender(window, drawHitboxes);
        // Render sprite
        Sprite temp_sprite = this.GetCurrentSpriteImage();
        temp_sprite.Position = new Vector2f(this.body.Position.X - (temp_sprite.GetLocalBounds().Width / 2 * this.facing), this.body.Position.Y - temp_sprite.GetLocalBounds().Height);
        temp_sprite.Scale = new Vector2f(this.size_ratio * this.facing, this.size_ratio);
        window.Draw(temp_sprite);

        // Play sounds
        this.PlaySound();
        
        // Draw Hitboxes
        if (drawHitboxes) {  
            // Desenha o ponto central
            RectangleShape anchorY = new RectangleShape(new Vector2f(0, 10)) {
                Position = new Vector2f(this.body.Position.X, this.body.Position.Y - 60),
                FillColor = SFML.Graphics.Color.Transparent,
                OutlineColor = Color.White, 
                OutlineThickness = 1.0f
            };
            RectangleShape anchorX = new RectangleShape(new Vector2f(10, 0)) {
                Position = new Vector2f(this.body.Position.X - 5, this.body.Position.Y - 55),
                FillColor = SFML.Graphics.Color.Transparent,
                OutlineColor = Color.White, 
                OutlineThickness = 1.0f 
            };
            
            window.Draw(anchorX);
            window.Draw(anchorY);

            foreach (GenericBox box in this.CurrentBoxes) {
                // Calcula as coordenadas absolutas da hitbox
                float x1 = box.getRealA(this).X * size_ratio;
                float y1 = box.getRealA(this).Y * size_ratio;
                float x2 = box.getRealB(this).X * size_ratio;
                float y2 = box.getRealB(this).Y * size_ratio;

                // Cria o retângulo da hitbox
                Color color;
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
                    OutlineColor = color, 
                    OutlineThickness = 1.0f 
                };

                // Desenha o retângulo da hitbox na janela
                window.Draw(hitboxRect);
            }

            UI.Instance.DrawText(window, this.CurrentState, this.body.Position.X - Camera.Instance.X, this.body.Position.Y - Camera.Instance.Y - 50, spacing: -10, size: 0.5f, alignment: "center");
        }
    }
    public override void DoAnimate() {
        base.DoAnimate();

        // Update Current Sprite
        this.CurrentSprite = CurrentAnimation.GetCurrentFrame().Sprite_index;
        this.CurrentSound = CurrentAnimation.GetCurrentFrame().Sound_index;

        // Update body.Position
        this.body.Update(this);
        this.body.Position.X += CurrentAnimation.GetCurrentFrame().DeltaX * this.facing;
        this.body.Position.Y += CurrentAnimation.GetCurrentFrame().DeltaY * this.facing;

        // Change state, if necessary
        if (CurrentAnimation.changeOnLastframe && this.CurrentAnimation.onLastFrame) {
            this.ChangeState(this.CurrentAnimation.post_state);
        } else if (CurrentAnimation.changeOnGround && this.body.Position.Y >= this.floorLine && this.body.LastPosition.Y < this.floorLine) {
            this.ChangeState(this.CurrentAnimation.post_state);
        }

        // Advance to the next frame and reset hit if necessary
        this.lastFrameIndex = this.CurrentFrameIndex;
        if (CurrentAnimation.AdvanceFrame() && CurrentAnimation.GetCurrentFrame().hasHit == false) this.hasHit = false;
    }
    public void PlaySound() {
        if (this.CurrentFrameIndex != this.lastFrameIndex && this.CurrentSound != null && characterSounds.ContainsKey(this.CurrentSound)) {
            this.characterSounds[this.CurrentSound].Volume = Config.Character_Volume;
            this.characterSounds[this.CurrentSound].Play();
        }
    }
    
    // Battle methods
    public virtual int ImposeBehavior(Character target) {
        return -1;
    }
    public bool isBlocking() {
        return this.isBlockingHigh() || this.isBlockingLow();
    }
    public bool isBlockingHigh() {
        if ((this.notActing || this.CurrentState == "OnBlock") && (this.blockingHigh || this.blocking)) return true;
        return (this.notActing || this.CurrentState == "OnBlock") && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Down", player: this.playerIndex, facing: this.facing);
    }
    public bool isBlockingLow() {
        if ((this.notActing || this.CurrentState == "OnBlockLow") && (this.blockingLow || this.blocking)) return true;
        return (this.notActing || this.CurrentState == "OnBlockLow") && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Down", player: this.playerIndex);
    }
    public void HitStun(Character enemy, int advantage, bool airbone = false, float airbone_height = 0, float airbone_X = 5, bool force = false) {
        this.facing = -enemy.facing;
        
        if (airbone || this.LifePoints.X <= 0 || this.onAir && airbone_height > 0) {
            if (airbone_height == 0) airbone_height = 10;
            this.ChangeState("Airboned", reset: true);
            this.facing = -enemy.facing;
            this.SetVelocity(
                X: airbone_X * (enemy.facing * this.facing), 
                Y: airbone_height, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
            this.StunFrames = 0;
            return;
        } else if (this.isCrounching) this.ChangeState("OnHitLow", reset: true);
        else this.ChangeState("OnHit", reset: true);

        if (force) {
            this.StunFrames = Math.Max(advantage, 1);
        } else {
            this.StunFrames = Math.Max(60 / enemy.CurrentAnimation.framerate * (enemy.CurrentAnimation.animSize - enemy.CurrentFrameIndex) + advantage, 1);
        }
    }
    public void BlockStun(Character enemy, int advantage, bool force = false) {
        if (this.isCrounching) this.ChangeState("OnBlockLow", reset: true);
        else this.ChangeState("OnBlock", reset: true);

        if (force) {
            this.StunFrames = Math.Max(advantage, 0);
        } else {
            this.StunFrames = Math.Max((60 / enemy.CurrentAnimation.framerate) * (enemy.CurrentAnimation.animSize - enemy.CurrentFrameIndex) + advantage, 0);
        }
    }
    public void CheckStun() {
        if (this.StunFrames > 0) {
            this.StunFrames -= 1;
            if (this.StunFrames == 0) {
                if (this.isCrounching) this.ChangeState("Crouching");
                else this.ChangeState("Idle");
            }
        }
    }

    // Static Methods 
    public static void Pushback(Character target, Character self, string amount, float X_amount = 0, bool force_push = false) {
        if ((target.body.Position.X <= Camera.Instance.X - ((Config.maxDistance - 20) / 2) || target.body.Position.X >= Camera.Instance.X + ((Config.maxDistance - 20) / 2)) && !force_push) {
            if (X_amount != 0) {
                self.SetVelocity(X: -X_amount, Y: -self.body.Velocity.Y, raw_set: true);
            } else if (amount == "Light") {
                self.SetVelocity(X: -Config.light_pushback, Y: -self.body.Velocity.Y, raw_set: true);
            } else if (amount == "Medium") {
                self.SetVelocity(X: -Config.medium_pushback, Y: -self.body.Velocity.Y, raw_set: true);
            } else if (amount == "Heavy"){
                self.SetVelocity(X: -Config.heavy_pushback, Y: -self.body.Velocity.Y, raw_set: true);
            }
        } else {
            if (X_amount != 0) {
                target.SetVelocity(X: -X_amount, Y: -target.body.Velocity.Y, raw_set: true);
            } else if (amount == "Light") {
                target.SetVelocity(X: -Config.light_pushback, Y: -target.body.Velocity.Y, raw_set: true);
            } else if (amount == "Medium") {
                target.SetVelocity(X: -Config.medium_pushback, Y: -target.body.Velocity.Y, raw_set: true);
            } else if (amount == "Heavy"){
                target.SetVelocity(X: -Config.heavy_pushback, Y: -target.body.Velocity.Y, raw_set: true);
            }
        }
    }
    public static void Damage(Character target, Character self, int damage, int dizzy_damage) {
        target.LifePoints.X -= (int) (damage * self.damageScaling);
        target.DizzyPoints.X -= (int) (dizzy_damage * self.damageScaling);
    }

    // Auxiliar methods
    public void SetVelocity(float X = 0, float Y = 0, int T = 0, bool raw_set = false) {
        this.body.SetVelocity(this, X, Y, raw_set: raw_set);
    }
    public void AddVelocity(float X = 0, float Y = 0, int T = 0, bool raw_set = false) {
        this.body.AddVelocity(this, X, Y, raw_set: raw_set);
    }
    public void ChangeState(string newState, int index = 0, bool reset = false) {
        this.LastState = CurrentState;

        if (animations.ContainsKey(newState)) {
            this.CurrentState = newState;
            
        }

        if (CurrentState != LastState || reset) {
            this.animations[LastState].Reset();
            this.CurrentAnimation.currentFrameIndex = index;
        }

        this.hasHit = false;

        if (this.CurrentState.Contains("Crouching") || this.CurrentState.Contains("Low")) {
            this.isCrounching = true;
        } else {
            this.isCrounching = false;
        }

        if (this.CurrentState.Contains("Falling")) {
            this.StunFrames = 0;
        }
    }
    public Sprite GetCurrentSpriteImage() {
        if (spriteImages.ContainsKey(this.CurrentSprite))
        {
            return spriteImages[this.CurrentSprite];
        }
        return new Sprite(); 
    }
    public void Reset(int start_point, int facing, String state = "Idle") {
        this.ChangeState(state);
        this.LifePoints.X = this.LifePoints.Y;
        this.DizzyPoints.X = this.DizzyPoints.Y;
        this.body.Position.X = start_point;
        this.body.Position.Y = this.floorLine;
        this.body.SetVelocity(this, 0, 0, raw_set: true);
        this.facing = facing;
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
    public static Character SelectCharacter(int index, Stage stage) {
        switch (index) {
            case 0:
                var Ken_object = new Ken("Intro", 0, stage.floorLine, stage);
                Ken_object.Load();
                return Ken_object;
            
            case 1:
                var Psylock_object = new Psylock("Intro", 0, stage.floorLine, stage);
                Psylock_object.Load();
                return Psylock_object;

            default:
                var Default_object = new Ken("Intro", 0, stage.floorLine, stage);
                Default_object.Load();
                return Default_object;
        }
    }
    public virtual void Load() {}
    public override void Unload() {
        this.UnloadSounds();
        this.UnloadSpriteImages();
    }
}

}
