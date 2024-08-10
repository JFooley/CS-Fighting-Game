using Input_Space;
using Character_Space;
using SFML.Graphics;
using SFML.Window;

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
        int game_state = 0;
        bool showBoxs = false;

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(1366, 768), "Fighting Game CS");
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(60);

        // Inicializa o input e camera
        InputManager.Initialize(InputManager.KEYBOARD_INPUT, true);
        Camera camera = Camera.GetInstance(window);

        // Carega o Stage
        var stage = new BurningDojo();
        stage.LoadStage();

        // Carrega os personagens
        Console.WriteLine("Carregando os persoangens");
        var Ken_object = new Ken("Idle", stage.start_point_A, stage.floorLine);
        Ken_object.Load();
        var Psylock_object = new Psylock("Idle", stage.start_point_B, stage.floorLine);
        Psylock_object.Load();

        // Ultimos ajustes
        List<Character> OnSceneCharacters = new List<Character> {Ken_object, Psylock_object};
        camera.SetChars(Ken_object, Psylock_object);
        camera.Update();
        stage.setChars(Ken_object, Psylock_object);

        window.Resized += (sender, e) =>
{
    Camera.GetInstance().UpdateScreenSize(e.Size);
};

        while (window.IsOpen) {
            // First
            window.DispatchEvents();
            window.Clear(Color.Black);
            InputManager.Instance.Update();
            camera.Update();

            // on Battle Scene
            stage.Update(window);
            foreach (Character char_object in OnSceneCharacters) char_object.Update();
            foreach (Character char_object in OnSceneCharacters) char_object.Render(window, showBoxs);

            // DEBUG
            if (InputManager.Instance.Key_down(4)) showBoxs = !showBoxs;

            Console.Clear();
            foreach (Character char_object in OnSceneCharacters) {
                Console.WriteLine("-----------------------Personagem A-----------------------");
                Console.WriteLine("Posição X: " + char_object.PositionX + " Posição Y: " + char_object.PositionY);
                Console.WriteLine("State: " + char_object.CurrentState + " Frame Index: " + char_object.CurrentAnimation.currentFrameIndex + " Sprite Index: " + char_object.CurrentSprite);
            }
            Console.WriteLine("Camera - X: " + camera.X + " Y: " + camera.Y);
            // DEBUG
            
            // Finally
            window.Display();
        }
    }
}

