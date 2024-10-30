using Input_Space;
using Character_Space;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Stage_Space;
using UI_space;
using System.Windows.Forms;

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
        // Aux
        Console.WriteLine("Stage index: ");
        int selected_stage = int.Parse(Console.ReadLine());
        bool showBoxs = false;
        bool printInfos = false;

        // Necessary infos
        int game_state = Intro;
        int sub_state = Intro;

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(Config.WindowWidth, Config.WindowHeight), Config.GameTitle);
        window.Closed += (sender, e) => window.Close();

        // FPS controll
        Clock clock = new Clock();
        float deltaTime;
        window.SetFramerateLimit(Config.Framerate);
        window.SetVerticalSyncEnabled(true);
        
        // Cria uma view
        var view = new SFML.Graphics.View(new FloatRect(0, 0, Config.WindowWidth, Config.WindowHeight));
        view.Zoom(0.3f);
        window.SetView(view);

        // Inicializações
        InputManager.Initialize(autoDetectDevice: true);
        Camera camera = Camera.GetInstance(window, view);
        BitmapFont.Load("Assets/fonts/atlas.png");
        UI.Instance.LoadCharacterSprites(40);

        // Carega o Stage
        List<Stage> stages = new List<Stage>{
            new BurningDojo(),
            new MidnightDuel(),
            new NightAlley(),
        };
        var stage = stages[selected_stage];
        stage.LoadStage();

        // Carrega os personagens
        Console.WriteLine("Carregando os persoangens, aguarde");
        var Ken_object = new Ken("Intro", stage.start_point_A, stage.floorLine, stage);
        Ken_object.Load();
        var Psylock_object = new Psylock("Intro", stage.start_point_B, stage.floorLine, stage);
        Psylock_object.Load();

        // Ultimos ajustes
        camera.SetChars(Ken_object, Psylock_object);
        camera.SetLimits(stage.length, stage.height);
        stage.setChars(Ken_object, Psylock_object);
        stage.OnSceneCharacters = new List<Character> {Ken_object, Psylock_object};
        stage.TogglePlayers();
        Console.WriteLine("Finalizado");

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
                            if (InputManager.Instance.anyKey) {
                                if (stage.CheckMatchEnd()) { // Verifica se a partida terminou
                                    stage.ResetMatch();
                                    stage.ResetPlayers(force: true);
                                    sub_state = Intro;
                                    game_state = Intro;
                                } else {
                                    sub_state = RoundStart;
                                }
                            }
                            break;
                    }

                    break;
            }

            // Finally
            window.Display();
            deltaTime = clock.Restart().AsSeconds();

            // DEBUG
            if (InputManager.Instance.Key_down("Start")) showBoxs = !showBoxs;
            if (InputManager.Instance.Key_down("Select")) printInfos = !printInfos;

            if (printInfos) {
                Console.Clear();
                Console.WriteLine("FPS: " + (int) (1 / deltaTime) + " | Frame time: " + deltaTime);
                Console.WriteLine("-----------------------Game-----------------------");
                Console.WriteLine("State: " + game_state + " Sub-state: " + sub_state);
                for (int i = 0; i < 2; i++) {
                    var char_object = stage.OnSceneCharacters[i];
                    Console.WriteLine("-----------------------Personagem "+ char_object.name + "-----------------------");
                    Console.WriteLine("Posição X: " + char_object.Position.X + " Posição Y: " + char_object.Position.Y);
                    Console.WriteLine("State: " + char_object.CurrentState + " - Frame Index: " + char_object.CurrentAnimation.currentFrameIndex + " - Sprite Index: " + char_object.CurrentSprite);
                    Console.WriteLine("LP: " + char_object.LifePoints.X + "/" + char_object.LifePoints.Y);
                    Console.WriteLine("SP: " + char_object.DizzyPoints.X + "/" + char_object.DizzyPoints.Y);
                    Console.WriteLine("Facing: " + char_object.facing);
                    Console.WriteLine("Blocking High: " + char_object.isBlockingHigh());
                    Console.WriteLine("Blocking Low:  " + char_object.isBlockingLow());
                }
                Console.WriteLine("-----------------------Outros-----------------------");
                Console.WriteLine("Camera - X: " + camera.X + " Y: " + camera.Y);
                Console.WriteLine("Inputs Default: " + Convert.ToString(InputManager.Instance.buttonState[0], 2).PadLeft(14, '0'));
                Console.WriteLine("Inputs Char A:  " + Convert.ToString(InputManager.Instance.buttonState[1], 2).PadLeft(14, '0'));
                Console.WriteLine("Inputs Char B:  " + Convert.ToString(InputManager.Instance.buttonState[2], 2).PadLeft(14, '0'));
                Console.WriteLine("-----------------------Battle-----------------------");
                Console.WriteLine("Rounds A - " + stage.rounds_A + " | " + stage.round_time + " | " + stage.rounds_B + " - Rounds B");
            }
            // DEBUG
        }
    }
}

