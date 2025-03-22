using SFML.Graphics;
using SFML.System;
using Stage_Space;
using System.Drawing;

namespace UI_space {
    public class UI {
        private static UI instance;
        private int elapsed = 0;
        public int counter = 0;

        // Clocks
        public bool blink10Hz = true;
        public bool blink4Hz = true;
        public bool blink2Hz = true;
        public bool blink1Hz = true;

        private int graylife_A = 150;
        private int graylife_B = 150;

        public SFML.Graphics.Color light_background = new SFML.Graphics.Color(150, 150, 150);
        public SFML.Graphics.Color background = new SFML.Graphics.Color(35, 31, 34);
        public SFML.Graphics.Color outline = new SFML.Graphics.Color(255, 255, 255);
        public SFML.Graphics.Color bar_graylife = new SFML.Graphics.Color(200, 0, 0);
        public SFML.Graphics.Color bar_fulllife = new SFML.Graphics.Color(50, 190, 60);
        public SFML.Graphics.Color bar_life = new SFML.Graphics.Color(240, 220, 20);
        public SFML.Graphics.Color bar_super = new SFML.Graphics.Color(5, 110, 150);
        public SFML.Graphics.Color bar_super_full = new SFML.Graphics.Color(0, 185, 255);
        
        // visuals
        Sprite hud = new Sprite(new Texture("Assets/ui/hud.png"));
        private Dictionary<string, Dictionary<char, Sprite>> font_textures;
        private string superBarMsg = "Max!";

        private UI() {
            this.font_textures = new Dictionary<string, Dictionary<char, Sprite>>();
        }

        public static UI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UI();
                }
                return instance;
            }
        }

        public void Update() {
            this.counter = this.counter <= 60 ? this.counter + 1 : 1;
            this.blink10Hz = this.counter % (60/10) == 0 ? this.blink10Hz = !this.blink10Hz : this.blink10Hz;
            this.blink4Hz = this.counter % (60/4) == 0 ? this.blink4Hz = !this.blink4Hz : this.blink4Hz;
            this.blink2Hz = this.counter % (60/2) == 0 ? this.blink2Hz = !this.blink2Hz : this.blink2Hz;
            this.blink1Hz = !this.blink2Hz;
        }

        // Loads
        public void LoadCharacterSprites(float size, string textureName) {
            Dictionary<char, Sprite> characterSprites = new Dictionary<char, Sprite>(); // Cria grupo de sprites
            foreach (char c in BitmapFont.characters) {
                Sprite sprite = BitmapFont.GetCharacterSprite(c, size, textureName);
                if (sprite != null)
                {
                    characterSprites[c] = sprite;
                }
            }
            this.font_textures[textureName] = characterSprites;
        }

        // Draw Callers
        public void ShowFramerate(RenderWindow window, string textureName) {
            this.elapsed = this.counter % (60/2) == 0 ? (int) (1 / Program.last_frame_time) : this.elapsed;
            this.DrawText(window, this.elapsed.ToString() + " - " + Program.last_frame_time.ToString("F5"), 0, 82, spacing: Config.spacing_small, size: 1f, textureName: textureName);
        }

        public void DrawText(RenderWindow window, string text, float X, float Y, float spacing = 0, float size = 1f, string alignment = "center", bool absolutePosition = false, string textureName = "default medium") {
            float totalWidth = 0;
            float pos_X;
            float pos_Y;
            float offset_X = X;
            List<Sprite> text_sprites = new List<Sprite> {};

            // Calcular a largura total do texto

            foreach (char c in text)
            {
                if (font_textures[textureName].TryGetValue(c, out Sprite letter))
                {
                    var sprite = new Sprite(letter);
                    if (size > 0) sprite.Scale = new Vector2f(size, size);
                    totalWidth += sprite.GetGlobalBounds().Width + spacing;
                    text_sprites.Add(sprite);
                }
            }
            
            // Compensa o primeiro caractere
            totalWidth -= spacing;

            // Ajustar posição se centralizado
            if (alignment == "center") {
                offset_X -= totalWidth / 2; 
            } else if (alignment == "right") {
                offset_X -= totalWidth; 
            }

            if (absolutePosition) {
                pos_X = X;
                pos_Y = Y;
                offset_X = 0;
            } else {
                pos_X = Camera.GetInstance().X;
                pos_Y = Camera.GetInstance().Y;
            }
            foreach (Sprite sprite in text_sprites) {   
                sprite.Position = new Vector2f(pos_X + offset_X, pos_Y + Y);
                window.Draw(sprite);
                offset_X += sprite.GetGlobalBounds().Width + spacing;
            }
        }

        public void DrawRectangle(RenderWindow window, float X, float Y, float width, float height, SFML.Graphics.Color color) {
            RectangleShape rectangle = new RectangleShape(new Vector2f(width, height))
            {
                Position = new Vector2f(Camera.GetInstance().X + X, Camera.GetInstance().Y + Y),
                FillColor = color
            };

            window.Draw(rectangle);
        }
    
        // Battle UI
        public void DrawBattleUI(RenderWindow window, Stage stage) {
            // Draw hud
            hud.Position = new Vector2f(Program.camera.X - 192, Program.camera.Y - 108);
            window.Draw(hud);

            // Character A
            // Draw lifebar A
            var lifeA_scale = stage.character_A.LifePoints.X * 150 / stage.character_A.LifePoints.Y;
            var lifeA = Math.Max(Math.Min(lifeA_scale, 150), 0);
            if (stage.character_B.comboCounter == 0) this.graylife_A = lifeA > this.graylife_A ? this.graylife_A = lifeA : (int) (this.graylife_A + (lifeA - this.graylife_A) * 0.01);
            this.DrawRectangle(window, -180 + (150 - this.graylife_A), -95, this.graylife_A, 8, this.bar_graylife);
            this.DrawRectangle(window, -180 + (150 - lifeA), -95, lifeA, 8, stage.character_A.LifePoints.X == stage.character_A.LifePoints.Y ? this.bar_fulllife : this.bar_life);

            // Draw Super bar A
            var superA_scale = stage.character_A.SuperPoints.X * 119 / stage.character_A.SuperPoints.Y;
            var superA = Math.Max(Math.Min(superA_scale, 119), 0);
            this.DrawRectangle(window, -180 + (119 - superA), 97, superA, 4, this.bar_super);
            if (stage.character_A.SuperPoints.X >= stage.character_A.SuperPoints.Y/2) {
                var control = stage.character_A.SuperPoints.X == stage.character_A.SuperPoints.Y ? this.blink10Hz : true;
                if (control) this.DrawRectangle(window, -180 + (119 - superA), 97, superA, 4, this.bar_super_full);
            }
            if (stage.character_A.SuperPoints.X == stage.character_A.SuperPoints.Y && this.blink2Hz) this.DrawText(window, this.superBarMsg, -193, 72, spacing: Config.spacing_medium, alignment: "left", textureName: "default medium");
            
            // Draw Stun bar A
            var stunA_scale = ( stage.character_A.DizzyPoints.Y - stage.character_A.DizzyPoints.X) * 150 / stage.character_A.DizzyPoints.Y;
            var stunA = Math.Max(Math.Min(stunA_scale, 150), 0);
            this.DrawRectangle(window, -180 + (150 - stunA), -86, stunA, 1, this.bar_graylife);

            // Character B
            // Draw lifebar B
            var lifeB_scale = stage.character_B.LifePoints.X * 150 / stage.character_B.LifePoints.Y;
            var lifeB = Math.Max(Math.Min(lifeB_scale, 150), 0);
            if (stage.character_A.comboCounter == 0) this.graylife_B = lifeB > this.graylife_B ? this.graylife_B = lifeB : (int) (this.graylife_B + (lifeB - this.graylife_B) * 0.01);
            this.DrawRectangle(window, 30, -95, this.graylife_B, 8, this.bar_graylife);
            this.DrawRectangle(window, 30, -95, lifeB, 8, stage.character_B.LifePoints.X == stage.character_B.LifePoints.Y ? this.bar_fulllife : this.bar_life);
            
            // Draw Super bar B
            var superB_scale = stage.character_B.SuperPoints.X * 119 / stage.character_B.SuperPoints.Y;
            var superB = Math.Max(Math.Min(superB_scale, 119), 0);
            this.DrawRectangle(window, 61, 97, superB, 4, this.bar_super);
            if (stage.character_B.SuperPoints.X >= stage.character_B.SuperPoints.Y/2) {
                var control = stage.character_B.SuperPoints.X == stage.character_B.SuperPoints.Y ? this.blink10Hz : true;
                if (control) this.DrawRectangle(window, 61, 97, superB, 4, this.bar_super_full);
            }
            if (stage.character_B.SuperPoints.X == stage.character_B.SuperPoints.Y && this.blink2Hz) this.DrawText(window, this.superBarMsg, 193, 72, spacing: Config.spacing_medium, alignment: "right", textureName: "default medium");

            // Draw Stun bar B
            var stunB_scale = ( stage.character_B.DizzyPoints.Y - stage.character_B.DizzyPoints.X) * 150 / stage.character_B.DizzyPoints.Y;
            var stunB = Math.Max(Math.Min(stunB_scale, 150), 0);
            this.DrawRectangle(window, 30, -86, stunB, 1, this.bar_graylife);
            
            
            // HUD elements
            // Draw names
            UI.Instance.DrawText(window, stage.character_A.name, -194, -95, spacing: Config.spacing_small, size: 1f, alignment: "left", textureName: "default small white");
            UI.Instance.DrawText(window, stage.character_B.name, 194, -95, spacing: Config.spacing_small, size: 1f, alignment: "right", textureName: "default small white");

            // Draw Combo text
            if (stage.character_A.comboCounter > 1) this.DrawText(window, "Combo " + stage.character_A.comboCounter, -190, -80, spacing: -23, alignment: "left", size: 1f, textureName: "default medium white");
            if (stage.character_B.comboCounter > 1) this.DrawText(window, "Combo " + stage.character_B.comboCounter, 190, -80, spacing: -23, alignment: "right", size: 1f, textureName: "default medium white");

            // Draw time
            this.DrawText(window, "" + Math.Max(stage.round_time, 0), 0, -106, alignment: "center", spacing: -8, size: 1f, textureName: "1");

            // Draw round indicator ≈
            this.DrawText(window, string.Concat(Enumerable.Repeat("*", stage.rounds_A)), -20, -93, spacing: -19, alignment: "right", textureName: "icons");
            this.DrawText(window, string.Concat(Enumerable.Repeat("*", stage.rounds_B)),  20, -93, spacing: -19, alignment: "left", textureName: "icons");
        }
        
        public void LoadFonts() {
            BitmapFont.Load("default medium", "Assets/fonts/default medium.png");
            BitmapFont.Load("default medium grad", "Assets/fonts/default medium grad.png");
            BitmapFont.Load("default medium white", "Assets/fonts/default medium white.png");
            BitmapFont.Load("default medium red", "Assets/fonts/default medium red.png");
            BitmapFont.Load("default medium click", "Assets/fonts/default medium click.png");
            BitmapFont.Load("default medium hover", "Assets/fonts/default medium hover.png");

            BitmapFont.Load("default small", "Assets/fonts/default small.png");
            BitmapFont.Load("default small grad", "Assets/fonts/default small grad.png");
            BitmapFont.Load("default small white", "Assets/fonts/default small white.png");
            BitmapFont.Load("default small red", "Assets/fonts/default small red.png");
            BitmapFont.Load("default small click", "Assets/fonts/default small click.png");
            BitmapFont.Load("default small hover", "Assets/fonts/default small hover.png");

            BitmapFont.Load("1", "Assets/fonts/font1.png");
            BitmapFont.Load("icons", "Assets/fonts/icons.png");

            UI.Instance.LoadCharacterSprites(32, "default medium");
            UI.Instance.LoadCharacterSprites(32, "default medium grad");
            UI.Instance.LoadCharacterSprites(32, "default medium white");
            UI.Instance.LoadCharacterSprites(32, "default medium red");
            UI.Instance.LoadCharacterSprites(32, "default medium click");
            UI.Instance.LoadCharacterSprites(32, "default medium hover");

            UI.Instance.LoadCharacterSprites(32, "default small");
            UI.Instance.LoadCharacterSprites(32, "default small grad");
            UI.Instance.LoadCharacterSprites(32, "default small white");
            UI.Instance.LoadCharacterSprites(32, "default small red");
            UI.Instance.LoadCharacterSprites(32, "default small click");
            UI.Instance.LoadCharacterSprites(32, "default small hover");

            UI.Instance.LoadCharacterSprites(32, "1");
            UI.Instance.LoadCharacterSprites(32, "icons");
        }
    }

    public static class BitmapFont {
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static char[] characters = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
            'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '!', '@', '#', '$', '%', '^', '&', '*',
            '(', ')', '_', '+', '-', '=', '[', ']', '{', '}',
            ';', ':', '\'', '"', '\\', '|', ',', '.', '<', '>',
            '/', '?', '¿', '\u0020', '«', '»', '•', '°', 'µ', '~'
        };

        private const int CellSize = 32; // Tamanho de cada célula
        private const int Columns = 10;    // Número de colunas
        private const int Rows = 10;       // Número de linhas

        public static void Load(string textureName, string textureFile)
        {
            // Carrega a textura do atlas e a associa a um nome
            Texture texture = new Texture(textureFile);
            textures[textureName] = texture;
        }

        public static Sprite GetCharacterSprite(char character, float size, string textureName)
        {
            // Verifica se a textura existe
            if (!textures.ContainsKey(textureName))
            {
                return null; // Retorna null se a textura não for encontrada
            }

            Texture texture = textures[textureName];

            // Encontra o índice do caractere no array
            int index = Array.IndexOf(characters, character);
            if (index == -1 || index >= Columns * Rows) // Ignora as últimas 8 células
            {
                return null; // Retorna null se o caractere não for encontrado
            }

            // Calcula a posição na textura
            int x = (index % Columns) * CellSize;
            int y = (index / Columns) * CellSize;

            // Cria um sprite do caractere
            Sprite sprite = new Sprite(texture, new IntRect(x, y, CellSize, CellSize));
            sprite.Scale = new Vector2f(size / CellSize, size / CellSize); // Ajusta o tamanho
            return sprite;
        }
    }

}
