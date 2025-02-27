using SFML.Graphics;
using SFML.System;
using Stage_Space;
using System.Collections.Generic;

namespace UI_space {
    public class UI {
        private static UI instance;
        private Clock clock;
        private int elapsed = 0;
        private int counter = 0;

        private int graylife_A = 150;
        private int graylife_B = 150;

        private Color bar_background = new Color(35, 31, 34);
        private Color bar_outline = new Color(255, 255, 255);
        private Color bar_graylife = new Color(200, 0, 0);
        private Color bar_fulllife = new Color(50, 190, 60);
        private Color bar_life = new Color(240, 220, 20);
        private Color dizzybar_background = new Color(150, 150, 150);
        private Color bar_super = new Color(41, 168, 195);
        private Color bar_super_full = new Color(0, 210, 255);


        private Dictionary<string, Dictionary<char, Sprite>> font_textures;

        private UI() {
            this.font_textures = new Dictionary<string, Dictionary<char, Sprite>>();
            this.clock = new Clock();
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

        // Simple Draw Calls
        public void ShowFramerate(RenderWindow window, string textureName) {
            var frametime = this.clock.Restart().AsSeconds();
            this.counter = this.counter >= 30 ? 0 : this.counter + 1;
            this.elapsed = this.counter == 1 ? (int) (1 / frametime) : this.elapsed;
            this.DrawText(window, "" + this.elapsed, -190, 80, spacing: -19, alignment: "left", size: 0.8f, textureName: textureName);
        }

        public void DrawText(RenderWindow window, string text, float X, float Y, float spacing = 0, float size = 0, string alignment = "center", bool absolutePosition = false, string textureName = "default") {
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

        public void DrawRectangle(RenderWindow window, float X, float Y, float width, float height, Color color) {
            RectangleShape rectangle = new RectangleShape(new Vector2f(width, height))
            {
                Position = new Vector2f(Camera.GetInstance().X + X, Camera.GetInstance().Y + Y),
                FillColor = color
            };

            window.Draw(rectangle);
        }
    
        // Battle UI
        public void DrawBattleUI(RenderWindow window, Stage stage) {
            // Draw time
            this.DrawText(window, "" + Math.Max(stage.round_time, 0), 0, -113, alignment: "center", spacing: -25, size: 1.4375f, textureName: "1");

            // Draw Combo text
            if (stage.character_A.comboCounter > 1) this.DrawText(window, "Combo " + stage.character_A.comboCounter, -185, -80, spacing: -15, alignment: "left", size: 0.875f, textureName: "default grad");
            if (stage.character_B.comboCounter > 1) this.DrawText(window, "Combo " + stage.character_B.comboCounter, 185, -80, spacing: -15, alignment: "right", size: 0.875f, textureName: "default grad");

            // Draw lifebar A
            UI.Instance.DrawText(window, stage.character_A.name, -185, -110, spacing: -10, size: 0.5f, alignment: "left", textureName: "default");
            var lifeA_scale = stage.character_A.LifePoints.X * 150 / stage.character_A.LifePoints.Y;
            var lifeA = Math.Max(Math.Min(lifeA_scale, 150), 0);
            if (stage.character_B.comboCounter == 0) this.graylife_A = lifeA > this.graylife_A ? this.graylife_A = lifeA : (int) (this.graylife_A + (lifeA - this.graylife_A) * 0.01);
            this.DrawRectangle(window, -181, -96, 152, 12, this.bar_outline);
            this.DrawRectangle(window, -180, -95, 150, 8, this.bar_background);
            this.DrawRectangle(window, -180 + (150 - this.graylife_A), -95, this.graylife_A, 8, this.bar_graylife);
            this.DrawRectangle(window, -180 + (150 - lifeA), -95, lifeA, 8, stage.character_A.LifePoints.X == stage.character_A.LifePoints.Y ? this.bar_fulllife : this.bar_life);

            // Draw Super bar A
            var superA_scale = stage.character_A.SuperPoints.X * 119 / stage.character_A.SuperPoints.Y;
            var superA = Math.Max(Math.Min(superA_scale, 119), 0);
            this.DrawRectangle(window, -181, 96, 121, 6, this.bar_outline);
            this.DrawRectangle(window, -180, 97, 119, 4, this.bar_background);
            this.DrawRectangle(window, -180 + (119 - superA), 97, superA, 4, stage.character_B.SuperPoints.X == stage.character_B.SuperPoints.Y ? this.bar_super_full : this.bar_super);
            if (stage.character_A.SuperPoints.X == stage.character_A.SuperPoints.Y && stage.round_time % 2 == 0) {
                this.DrawText(window, "Ready!", -62, 92, spacing: -8, alignment: "left", size: 0.4f, textureName: "default");
            }

            // Draw Stun bar A
            var stunA_scale = ( stage.character_A.DizzyPoints.Y - stage.character_A.DizzyPoints.X) * 150 / stage.character_A.DizzyPoints.Y;
            var stunA = Math.Max(Math.Min(stunA_scale, 150), 0);
            this.DrawRectangle(window, -180, -86, 150, 1, this.dizzybar_background);
            this.DrawRectangle(window, -180 + (150 - stunA), -86, stunA, 1, this.bar_graylife);
            
            // Draw lifebar B
            UI.Instance.DrawText(window, stage.character_B.name, 185, -110, spacing: -10, size: 0.5f, alignment: "right", textureName: "default");
            var lifeB_scale = stage.character_B.LifePoints.X * 150 / stage.character_B.LifePoints.Y;
            var lifeB = Math.Max(Math.Min(lifeB_scale, 150), 0);
            if (stage.character_A.comboCounter == 0) this.graylife_B = lifeB > this.graylife_B ? this.graylife_B = lifeB : (int) (this.graylife_B + (lifeB - this.graylife_B) * 0.01);
            this.DrawRectangle(window, 29, -96, 152, 12, this.bar_outline);
            this.DrawRectangle(window, 30, -95, 150, 8, this.bar_background);
            this.DrawRectangle(window, 30, -95, this.graylife_B, 8, this.bar_graylife);
            this.DrawRectangle(window, 30, -95, lifeB, 8, stage.character_B.LifePoints.X == stage.character_B.LifePoints.Y ? this.bar_fulllife : this.bar_life);
            
            // Draw Super bar B
            var superB_scale = stage.character_B.SuperPoints.X * 119 / stage.character_B.SuperPoints.Y;
            var superB = Math.Max(Math.Min(superB_scale, 119), 0);
            this.DrawRectangle(window, 60, 96, 121, 6, this.bar_outline);
            this.DrawRectangle(window, 61, 97, 119, 4, this.bar_background);
            this.DrawRectangle(window, 61, 97, superB, 4, stage.character_B.SuperPoints.X == stage.character_B.SuperPoints.Y ? this.bar_super_full : this.bar_super);
            if (stage.character_B.SuperPoints.X == stage.character_B.SuperPoints.Y && stage.round_time % 2 == 0) {
                this.DrawText(window, "Ready!", 64, 92, spacing: -8, alignment: "right", size: 0.4f, textureName: "default");
            }

            // Draw Stun bar B
            var stunB_scale = ( stage.character_B.DizzyPoints.Y - stage.character_B.DizzyPoints.X) * 150 / stage.character_B.DizzyPoints.Y;
            var stunB = Math.Max(Math.Min(stunB_scale, 150), 0);
            this.DrawRectangle(window, 30, -86, 150, 1, this.dizzybar_background);
            this.DrawRectangle(window, 30, -86, stunB, 1, this.bar_graylife);

            // Draw round indicator ≈
            this.DrawText(window, string.Concat(Enumerable.Repeat("·", Config.max_rounds - stage.rounds_A)) + string.Concat(Enumerable.Repeat("≈", stage.rounds_A)), -16, -97, spacing: -25, alignment: "right", size: 1.25f, textureName: "1");
            this.DrawText(window, string.Concat(Enumerable.Repeat("≈", stage.rounds_B)) + string.Concat(Enumerable.Repeat("·", Config.max_rounds - stage.rounds_B)),  16, -97, spacing: -25, alignment: "left", size: 1.25f, textureName: "1");
        }
        
    }

    public static class BitmapFont {
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        public static char[] characters = 
        {
            '≈',  '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.',
            '/',  '0', '1', '2', '3', '4', '5', '6',  '7', '8', '9', ':', ';', '<', '=', 
            '>',  '?', '@', 'A', 'B', 'C', 'D', 'E',  'F', 'G', 'H', 'I', 'J', 'K', 'L', 
            'M',  'N', 'O', 'P', 'Q', 'R', 'S', 'T',  'U', 'V', 'W', 'X', 'Y', 'Z', '[', 
            '\\', ']', '^', '_', '`', 'a', 'b', 'c',  'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k',  'l', 'm', 'n', 'o', 'p', 'q', 'r',  's', 't', 'u', 'v', 'w', 'x', 'y', 
            'z',  '{', '¦', '}', '~', 'Ç', 'ü', 'é',  'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë',
            'è',  'ï', 'î', 'ì', 'Ä', 'Å', 'É', 'æ',  'Æ', 'ô', 'ö', 'ò', 'û', 'ù', 'ÿ', 
            'Ö',  'Ü', '¢', '£', '¥', 'ƒ', 'á', 'í',  'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º', 
            '¿',  '⌐', '¬', '½', '¼', '¡', '«', '»',  'π', 'µ', '±', '≥', '≤', '÷', 
            '≈',  '°', '·', '√', ' '
        };

        private const int CellSize = 32; // Tamanho de cada célula
        private const int Columns = 16;    // Número de colunas
        private const int Rows = 16;       // Número de linhas

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
            if (index == -1 || index >= Columns * Rows - 8) // Ignora as últimas 8 células
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
