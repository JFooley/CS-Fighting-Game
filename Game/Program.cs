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
    public const int MatchEnd = 4;

    public static void Main() {  
        // Aux
        // int selected_stage = int.Parse(Console.ReadLine());
        int selected_stage = 0;
        bool showBoxs = false;

        // Necessary infos
        int game_state = Intro;
        int sub_state = Intro;

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(Config.WindowWidth, Config.WindowHeight), Config.GameTitle);
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(Config.Framerate);
        window.SetVerticalSyncEnabled(Config.Vsync);
        
        // Cria uma view
        var view = new SFML.Graphics.View(new FloatRect(0, 0, Config.WindowWidth * 0.3f, Config.WindowHeight* 0.3f));
        // view.Zoom(0.3f);
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
                            stage.StopRoundTime();
                            stage.ResetTimer();
                            if (stage.character_A.CurrentState == "Idle" && stage.character_B.CurrentState == "Idle") { // Espera até a animação de intro finalizar
                                sub_state = RoundStart;
                            }
                            break;

                        case RoundStart: // Inicia a round
                            if (stage.CheckTimer(3)) UI.Instance.DrawText(window, "Fight!", 0, -30, spacing: -25);
                            else if (stage.CheckTimer(2)) UI.Instance.DrawText(window, "1", 0, -30, spacing: -25);
                            else if (stage.CheckTimer(1)) UI.Instance.DrawText(window, "2", 0, -30, spacing: -25);
                            else UI.Instance.DrawText(window, "3", 0, -30, spacing: -25);
                            if (!stage.CheckTimer(4)) break;

                            stage.ResetRoundTime();
                            stage.TogglePlayers();
                            sub_state = Battling;
                            break;

                        case Battling: // Durante a batalha
                            if (stage.CheckRoundEnd()) {
                                sub_state = RoundEnd;
                                stage.TogglePlayers();
                                stage.StopRoundTime();
                                stage.ResetTimer();
                            }
                            break;

                        case RoundEnd: // Fim de round
                            string message = stage.character_A.LifePoints.X <= 0 || stage.character_B.LifePoints.X <= 0 ? "KO" : "Timesup!";
                            UI.Instance.DrawText(window, message, 0, -30, spacing: -25);
                            if (!stage.CheckTimer(2)) break;

                            stage.ResetTimer();                            
                            if (stage.CheckMatchEnd()) {  
                                sub_state = MatchEnd;
                            } else {
                                sub_state = RoundStart;
                                stage.ResetPlayers();
                            }
                            break;

                        case MatchEnd: // Fim da partida
                            Thread.Sleep(2000);
                            stage.ResetMatch();
                            stage.ResetPlayers(force: true);
                            sub_state = Intro;
                            game_state = Intro;
                            break;
                    }

                    break;
            }

            // Finally
            if (showBoxs) UI.Instance.ShowFramerate(window);
            window.Display();

            // DEBUG
            if (InputManager.Instance.Key_down("Start")) showBoxs = !showBoxs;
            // DEBUG
        }
    }
}

