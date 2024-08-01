using Input_Space;
using Character_Space;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

public static class Program
{
    public static void Main() {  
        // Inicializa o input
        InputManager.Initialize(InputManager.KEYBOARD_INPUT, true);

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(1280, 720), "Fighting Game CS");
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(24);

        // Carrega os personagens
        Console.WriteLine("Carregando os persoangens");
        var Ken_object = new Ken("Idle", 100, 30);
        Ken_object.Load();
        var Akuma_object = new Akuma("Idle", 100, 250);
        Akuma_object.Load();

        List<Character> OnSceneCharacters = new List<Character> {Ken_object, Akuma_object};

        while (window.IsOpen) {   
            window.DispatchEvents();
            window.Clear(Color.Black);
            InputManager.Instance.Update();

            foreach (Character char_object in OnSceneCharacters) char_object.Update();

            // Render Temporário
            foreach (Character char_object in OnSceneCharacters) {
                Sprite temp_sprite = char_object.GetCurrentSpriteImage();
                temp_sprite.Position = new Vector2f(char_object.PositionX, char_object.PositionY);
                temp_sprite.Scale = new Vector2f(2.0f, 2.0f);
                window.Draw(temp_sprite);
            }

            // DEBUG
            Console.Clear();
            Console.WriteLine("-----------------------Personagem A-----------------------");
            Console.WriteLine("Posição X: " + Ken_object.PositionX + " Posição Y: " + Ken_object.PositionY);
            Console.WriteLine("State: " + Ken_object.CurrentState + " Frame Index: " + Ken_object.CurrentAnimation.currentFrameIndex + " Sprite Index: " + Ken_object.CurrentSprite);
            Console.WriteLine("-----------------------Personagem B-----------------------");
            Console.WriteLine("Posição X: " + Akuma_object.PositionX + " Posição Y: " + Akuma_object.PositionY);
            Console.WriteLine("State: " + Akuma_object.CurrentState + " Frame Index: " + Akuma_object.CurrentAnimation.currentFrameIndex + " Sprite Index: " + Akuma_object.CurrentSprite);
            // DEBUG

            window.Display();
        }
    }
}

