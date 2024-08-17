using Input_Space;
using Character_Space;
using SFML.Graphics;
using SFML.Window;
using System.Configuration;
using Stage_Space;
using System.CodeDom;

// ----- Game States -----
// 0 - Intro
// 1 - Main Screen
// 2 - Select Character
// 3 - Battle Intro
// 4 - Round Start
// 5 - Round End
// 5 - Battle End

// ----- Screen Infos -----
// True Size: 1280x720
// View Size: 384x216

public static class Program
{
    public static void Main() {  
        Console.WriteLine("Stage index: ");
        int selected_stage = int.Parse(Console.ReadLine());
        bool showBoxs = false;

        // Necessary infos
        int game_state = 0;

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(Config.WindowWidth, Config.WindowHeight), Config.GameTitle);
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(Config.Framerate);
        window.SetVerticalSyncEnabled(true);
        
        // Cria uma view
        var view = new View(new FloatRect(0, 0, Config.WindowWidth, Config.WindowHeight));
        view.Zoom(0.3f);
        window.SetView(view);

        // Inicializa o input e camera
        InputManager.Initialize(InputManager.KEYBOARD_INPUT, true);
        Camera camera = Camera.GetInstance(window, view);

        // Carega o Stage
        List<Stage> stages = new List<Stage>{
            new BurningDojo(),
            new MidnightDuel(),
            new NightAlley(),
        };
        var stage = stages[selected_stage];
        stage.LoadStage();

        // Carrega os personagens
        Console.WriteLine("Carregando os persoangens");
        var Ken_object = new Ken("Intro", stage.start_point_A, stage.floorLine);
        Ken_object.Load();
        var Psylock_object = new Psylock("Intro", stage.start_point_B, stage.floorLine);
        Psylock_object.Load();

        // Ultimos ajustes
        List<Character> OnSceneCharacters = new List<Character> {Ken_object, Psylock_object};
        camera.SetChars(Ken_object, Psylock_object);
        camera.SetLimits(stage.length, stage.height);
        stage.setChars(Ken_object, Psylock_object);

        while (window.IsOpen) {
            // First
            window.DispatchEvents();
            window.Clear(Color.Black);
            InputManager.Instance.Update();
            camera.Update();

            // on Battle Scene
            foreach (Character char_object in OnSceneCharacters) char_object.Update();
            stage.Update(window);
            foreach (Character char_object in OnSceneCharacters) char_object.Render(window, showBoxs);

            // DEBUG
            if (InputManager.Instance.Key_down("Start")) showBoxs = !showBoxs;

            Console.Clear();
            foreach (Character char_object in OnSceneCharacters) {
                Console.WriteLine("-----------------------Personagem "+ char_object.name + "-----------------------");
                Console.WriteLine("Posição X: " + char_object.Position.X + " Posição Y: " + char_object.Position.Y);
                Console.WriteLine("State: " + char_object.CurrentState + " Frame Index: " + char_object.CurrentAnimation.currentFrameIndex + " Sprite Index: " + char_object.CurrentSprite);
            }
            Console.WriteLine("-----------------------Outros-----------------------");
            Console.WriteLine("Camera - X: " + camera.X + " Y: " + camera.Y);
            Console.WriteLine("Inputs: " + Convert.ToString(InputManager.Instance.getButtonState, 2).PadLeft(12, '0'));
            // DEBUG
            
            // Finally
            window.Display();
        }
    }
}

