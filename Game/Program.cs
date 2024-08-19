using Input_Space;
using Character_Space;
using SFML.Graphics;
using SFML.Window;
using Stage_Space;

// ----- Game States -----
// 0 - Intro
// 1 - Main Screen
// 2 - Select Character
// 3 - Battle

// ----- Sub States -----
// 0 - Battle Intro
// 1 - Round Start
// 2 - Battling
// 3 - Round End

// ----- Screen Infos -----
// True Size: 1280x720
// View Size: 384x216

public static class Program
{
    public const int Intro = 0;
    public const int MainScreen = 1;
    public const int SelectCharacter = 2;
    public const int Battle = 3;
    public const int RoundStart = 1;
    public const int Battling = 2;
    public const int RoundEnd = 3;

    public static void Main() {  
        Console.WriteLine("Stage index: ");
        int selected_stage = int.Parse(Console.ReadLine());
        bool showBoxs = false;

        // Necessary infos
        int game_state = Intro;
        int sub_state = Intro;

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
        camera.SetChars(Ken_object, Psylock_object);
        camera.SetLimits(stage.length, stage.height);
        stage.setChars(Ken_object, Psylock_object);
        stage.OnSceneCharacters = new List<Character> {Ken_object, Psylock_object};

        while (window.IsOpen) {
            // First
            window.DispatchEvents();
            InputManager.Instance.Update();

            //
            switch (game_state) {
                case Intro:
                    game_state = Battle;
                    break;

                case MainScreen:
                    break;

                case SelectCharacter:
                    break;

                case Battle:
                    camera.Update();
                    stage.Update(window, showBoxs);

                    switch (sub_state) {
                        case Intro:
                            stage.ResetRoundTime();
                            if (stage.character_A.CurrentState == "Idle" && stage.character_B.CurrentState == "Idle") { // Espera até a animação de intro finalizar
                                sub_state = RoundStart;
                            }
                            break;

                        case RoundStart: // Inicia a round
                            stage.ResetRoundTime();
                            stage.ResetPlayers();
                            stage.TogglePlayers();
                            sub_state = Battling;
                            break;

                        case Battling: // Durante a batalha
                            if (stage.CheckRoundEnd()) {
                                sub_state = RoundEnd;
                                stage.TogglePlayers();
                            }
                            break;

                        case RoundEnd: // Finaliza do round

                            Thread.Sleep(2000); // RETIRAR

                            if (stage.CheckMatchEnd()) { // Verifica se a partida terminou
                                stage.ResetMatch();
                                stage.ResetPlayers(force: true);
                                sub_state = Intro;
                                game_state = Intro;
                            } else {
                                sub_state = RoundStart;
                            }
                            break;
                    }

                    break;
            }

            // Finally
            window.Display();

            // DEBUG
            if (InputManager.Instance.Key_change("Start")) showBoxs = !showBoxs;
            Console.Clear();
            foreach (Character char_object in stage.OnSceneCharacters) {
                Console.WriteLine("-----------------------Personagem "+ char_object.name + "-----------------------");
                Console.WriteLine("Posição X: " + char_object.Position.X + " Posição Y: " + char_object.Position.Y);
                Console.WriteLine("State: " + char_object.CurrentState + " - Frame Index: " + char_object.CurrentAnimation.currentFrameIndex + " - Sprite Index: " + char_object.CurrentSprite);
                Console.WriteLine("LP: " + char_object.LifePoints.X + "/" + char_object.LifePoints.Y);
                Console.WriteLine("SP: " + char_object.StunPoints.X + "/" + char_object.StunPoints.Y);
                Console.WriteLine("Facing: " + char_object.facing);
            }
            Console.WriteLine("-----------------------Outros-----------------------");
            Console.WriteLine("Camera - X: " + camera.X + " Y: " + camera.Y);
            Console.WriteLine("Inputs: " + Convert.ToString(InputManager.Instance.getButtonState, 2).PadLeft(12, '0'));
            Console.WriteLine("-----------------------Battle-----------------------");
            Console.WriteLine("Rounds A - " + stage.rounds_A + " | " + (stage.round_length - stage.elapsed_time) + " | " + stage.rounds_B + " - Rounds B");
            // DEBUG
        }
    }
}

