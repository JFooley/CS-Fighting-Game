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
    public const int SelectScreen = 2;
    public const int Battle = 3;

    public const int RoundStart = 1;
    public const int Battling = 2;
    public const int RoundEnd = 3;
    public const int MatchEnd = 4;

    public static void Main() {  
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
        window.SetView(view);

        // Inicializações
        InputManager.Initialize(autoDetectDevice: true);
        Camera camera = Camera.GetInstance(window, view);
        BitmapFont.Load("Assets/fonts/atlas.png");
        UI.Instance.LoadCharacterSprites(40);

        List<Stage> stages = new List<Stage>{
            new BurningDojo(),
            new MidnightDuel(),
            new NightAlley(),
        };
        Stage stage = stages[0];

        // temp
        int stage_index = 0;

        while (window.IsOpen) {
            // First
            window.DispatchEvents();
            InputManager.Instance.Update();

            switch (game_state) {
                case Intro:
                    game_state = MainScreen;
                    break;

                case MainScreen:
                    UI.Instance.DrawText(window, "press start", Config.WindowWidth * 0.3f / 2, Config.WindowHeight * 0.3f / 2 + 30, spacing: -20, size: 0.9f);

                    if (InputManager.Instance.Key_down("Start")) {
                        game_state = SelectScreen;
                    }
                    break;

                case SelectScreen:
                    UI.Instance.DrawText(window, stages[stage_index].name, Config.WindowWidth * 0.3f / 2, Config.WindowHeight * 0.3f / 2, spacing: -20, size: 0.9f);
                    UI.Instance.DrawText(window, "¿ ¥ ª", Config.WindowWidth * 0.3f / 2, Config.WindowHeight * 0.3f / 2 + 30, spacing: -10, size: 0.9f);

                    if (InputManager.Instance.Key_down("Left") && stage_index > 0) {
                        stage_index -= 1;
                    } else if (InputManager.Instance.Key_down("Right") && stage_index < stages.Count - 1) {
                        stage_index += 1;
                    } else if (InputManager.Instance.Key_down("A")) {
                        // Seleciona os chars e o stage
                        int charA_selected = 0;
                        int charB_selected = 0;
                        stage = stages[stage_index];
                        
                        // Escolhe o Stage
                        stage.LoadStage();

                        // Escolhe os personagens
                        stage.LoadCharacters(charA_selected, charB_selected);

                        // Configura a camera
                        camera.SetChars(stage.character_A, stage.character_B);
                        camera.SetLimits(stage.length, stage.height);

                        game_state = Battle;
                    } 
                    break;

                case Battle:
                    camera.Update();
                    stage.Update(window);

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
                                stage.StopRoundTime();
                                stage.ResetTimer();
                            }
                            break;

                        case RoundEnd: // Fim de round
                            string message = stage.character_A.LifePoints.X <= 0 || stage.character_B.LifePoints.X <= 0 ? "Æ" : "Time's up!";
                            UI.Instance.DrawText(window, message, 0, -30, spacing: -25, size: message == "Æ" ? 2 : 1.3f);
                            if (!stage.CheckTimer(3)) break;
                            stage.TogglePlayers();

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
                            // game_state = SelectScreen;

                            game_state = Battle; // remover dps
                            break;
                    }

                    break;
            }

            // Finally
            window.Display();
            window.Clear();
            if (InputManager.Instance.Key_down("Select")) stage.debug_mode = !stage.debug_mode;
        }
    }
}

