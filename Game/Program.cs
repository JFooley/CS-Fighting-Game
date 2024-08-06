using Input_Space;
using Character_Space;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Security.Cryptography.X509Certificates;

// ----- Game States -----
// 0 - Intro
// 1 - Main Screen
// 2 - Select Character
// 3 - Battle Intro
// 4 - Round Start
// 5 - Round End
// 5 - Battle End

public static class Program
{
    public static void Main() {  
        // Inicializa o input
        InputManager.Initialize(InputManager.KEYBOARD_INPUT, true);

        int game_satate = 0;

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(1280, 720), "Fighting Game CS");
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(24);

        // Carrega os personagens
        Console.WriteLine("Carregando os persoangens");
        var Ken_object = new Ken("Idle", 100, 30);
        Ken_object.Load();
        // var Akuma_object = new Akuma("Idle", 100, 250);
        // Akuma_object.Load();

        List<Character> OnSceneCharacters = new List<Character> {Ken_object};

        while (window.IsOpen) {
            // First
            window.DispatchEvents();
            window.Clear(Color.Black);
            InputManager.Instance.Update();

            // on Battle Scene
            foreach (Character char_object in OnSceneCharacters) char_object.Update();

            // Render Temporário
            foreach (Character char_object in OnSceneCharacters) char_object.Render(window, true);

            // DEBUG
            Console.Clear();
            foreach (Character char_object in OnSceneCharacters) {
                Console.WriteLine("-----------------------Personagem A-----------------------");
                Console.WriteLine("Posição X: " + char_object.PositionX + " Posição Y: " + char_object.PositionY);
                Console.WriteLine("State: " + char_object.CurrentState + " Frame Index: " + char_object.CurrentAnimation.currentFrameIndex + " Sprite Index: " + char_object.CurrentSprite);
            }
            // DEBUG
            
            // Finally
            window.Display();
        }
    }
}

