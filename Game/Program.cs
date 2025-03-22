using Input_Space;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Stage_Space;
using UI_space;
using System.Diagnostics;

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
    // Winner index
    public const int Drawn = 0;
    public const int Player1 = 1;
    public const int Player2 = 2;

    // Game States
    public const int Intro = 0;
    public const int MainMenu = 1;
    public const int SelectStage = 2;
    public const int SelectChar = 3;
    public const int LoadScreen = 4;
    public const int Battle = 5;
    public const int PostBattle = 6;
    public const int Settings = 7;

    // Battle States
    public const int RoundStart = 1;
    public const int Battling = 2;
    public const int RoundEnd = 3;
    public const int MatchEnd = 4;

    // State holders
    public static int game_state;
    public static int sub_state;

    // Common objects
    public static Camera camera;
    public static Stage stage;
    public static RenderWindow window;
    private static Stopwatch frametimer;
    public static Random random = new Random();

    // Session infos
    public static int player1_wins = 0;
    public static int player2_wins = 0;
    public static int winner = 0;

    // View
    public static SFML.Graphics.View view = new SFML.Graphics.View(new FloatRect(0, 0, Config.RenderWidth, Config.RenderHeight));

    // Aux
    public static int pointer = 0;
    public static int charA_selected = 0;
    public static int charB_selected = 0;
    public static bool loading = false;
    public static double last_frame_time = 0;

    public static void Main() {  
        Config.LoadFromFile();
        
        // Necessary infos
        game_state = Intro;
        sub_state = Intro;

        // Crie uma janela
        window = new RenderWindow(new VideoMode(Config.WindowWidth, Config.WindowHeight), Config.GameTitle);
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(Config.Framerate);
        window.SetVerticalSyncEnabled(Config.Vsync);
        
        // Cria uma view
        window.SetView(view);

        // Inicializações
        InputManager.Initialize(autoDetectDevice: true);
        camera = Camera.GetInstance(window);
        frametimer = new Stopwatch();

        // Carregamento de texturas de fontes
        UI.Instance.LoadFonts();

        // Instanciamento do stages
        List<Stage> stages = new List<Stage>{
            new Stage("Random", 0, 0, 0, "", "", "Assets/stages/random.png"),
            new BurningDojo(),
            new MidnightDuel(),
            new NightAlley(),
            new NYCSubway(),
            new RindoKanDojo(),
            new TheSavana(),
            new JapanFields(),
            new Stage("Settings", 0, 0, 0, "", "", "Assets/stages/settings.png"),
        };
        stage = stages[0];

        // Menu e SelectStage visuals
        Sprite main_screen = new Sprite(new Texture("Assets/ui/title.png"));
        Sprite frame = new Sprite(new Texture("Assets/ui/frame.png"));
        Sprite fade90 = new Sprite(new Texture("Assets/ui/90fade.png"));
        Sprite fight_logo = new Sprite(new Texture("Assets/ui/fight.png"));
        Sprite timesup_logo = new Sprite(new Texture("Assets/ui/timesup.png"));
        Sprite KO_logo = new Sprite(new Texture("Assets/ui/ko.png"));
        Sprite settings_bg = new Sprite(new Texture("Assets/ui/settings.png"));
        Sprite fslogo = new Sprite(new Texture("Assets/ui/fs.png"));

        while (window.IsOpen) {
            window.DispatchEvents();
            InputManager.Instance.Update();
            UI.Instance.Update();
            camera.Update();
            frametimer.Restart();

            switch (game_state) {
                case Intro:
                    game_state = MainMenu;
                    break;

                case MainMenu:
                    window.Draw(main_screen);
                    if (UI.Instance.blink2Hz || InputManager.Instance.Key_hold("Start")) UI.Instance.DrawText(window, "press start", 0, 50, spacing: Config.spacing_medium, size: 1f, textureName: InputManager.Instance.Key_hold("Start") ? "default medium click" : "default medium white");

                    if (InputManager.Instance.Key_up("Start")) {
                        game_state = SelectStage;
                        pointer = 0;
                    }
                    break;

                case SelectStage:
                    window.Draw(stages[pointer].thumb);
                    window.Draw(frame);
                    UI.Instance.DrawText(window, stages[pointer].name, 0, -80, spacing: Config.spacing_medium, size: 1f, textureName: InputManager.Instance.Key_hold("A") || InputManager.Instance.Key_hold("B") || InputManager.Instance.Key_hold("C") || InputManager.Instance.Key_hold("D") ? "default medium click" : "default medium white");

                    UI.Instance.DrawText(window, Program.player1_wins.ToString(), -Config.RenderWidth/2, -Config.RenderHeight/2, spacing: Config.spacing_medium, size: 1f, textureName: "default medium", alignment: "left");
                    UI.Instance.DrawText(window, Program.player2_wins.ToString(), Config.RenderWidth/2, -Config.RenderHeight/2, spacing: Config.spacing_medium, size: 1f, textureName: "default medium", alignment: "right");

                    if (InputManager.Instance.Key_down("Left") && pointer > 0) {
                        pointer -= 1;
                    } else if (InputManager.Instance.Key_down("Right") && pointer < stages.Count - 1) {
                        pointer += 1;
                    } else if (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")) {
                        if (stages[pointer].name == "Settings") {
                            game_state = Settings;
                        } else {
                            if (stages[pointer].name == "Random") pointer = Program.random.Next(1, stages.Count()-2);
                            Program.stage = stages[pointer];
                            game_state = SelectChar;
                        }
                        pointer = 0;
                    } 
                    break;
                
                case SelectChar:
                    charA_selected = 0;
                    charB_selected = 0;
                    game_state = LoadScreen;
                    break;
                
                case LoadScreen:
                    if (UI.Instance.counter % 20 == 0) pointer = pointer < 3 ? pointer + 1 : 0;
                    fslogo.Position = new Vector2f(10, 139);
                    window.Draw(fslogo);
                    UI.Instance.DrawText(window, string.Concat(Enumerable.Repeat(".", pointer)), -122, 68, alignment: "left", spacing: -24);

                    if (!loading) {
                        Thread stage_loader = new Thread(StageLoader);
                        stage_loader.Start();
                        loading = true;
                    }
                    break;

                case Battle:
                    stage.Update(window);
                    
                    switch (sub_state) {
                        case Intro:
                            stage.SetMusicVolume();
                            stage.StopRoundTime();
                            stage.ResetTimer();
                            if (stage.character_A.CurrentState == "Idle" && stage.character_B.CurrentState == "Idle") { // Espera até a animação de intro finalizar
                                sub_state = RoundStart;
                            }
                            break;

                        case RoundStart: // Inicia a round
                            if (stage.CheckTimer(2)) {
                                stage.ResetRoundTime();
                                stage.StartRoundTime();
                                stage.ReleasePlayers();
                                sub_state = Battling;
                            } else if (stage.CheckTimer(1)) {
                                fight_logo.Position = new Vector2f(Program.camera.X - 89, Program.camera.Y - 54);
                                window.Draw(fight_logo);
                            } else UI.Instance.DrawText(window, "Ready?", 0, -30, spacing: Config.spacing_medium, textureName: "default medium white");

                            break;

                        case Battling: // Durante a batalha
                            if (stage.CheckRoundEnd()) {
                                sub_state = RoundEnd;
                                stage.StopRoundTime();
                                stage.ResetTimer();
                            }
                            break;

                        case RoundEnd: // Fim de round
                            if (stage.GetTimerValue() < 3) {
                                if (stage.character_A.LifePoints.X <= 0 || stage.character_B.LifePoints.X <= 0) {
                                    KO_logo.Position = new Vector2f(Program.camera.X - 75, Program.camera.Y - 54);
                                    window.Draw(KO_logo);
                                } else {
                                    timesup_logo.Position = new Vector2f(Program.camera.X - 131, Program.camera.Y - 55);
                                    window.Draw(timesup_logo);
                                }
                            } 
                            if (stage.CheckTimer(4)) {
                                stage.LockPlayers();
                                stage.ResetTimer();                            
                                if (stage.CheckMatchEnd()) {
                                    sub_state = MatchEnd;
                                } else {
                                    sub_state = RoundStart;
                                    stage.ResetPlayers();
                                }
                            }
                            break;

                        case MatchEnd: // Fim da partida
                            camera.Reset();
                            stage.StopMusic();
                            stage.ResetMatch();
                            stage.ResetPlayers(force: true, total_reset: true);

                            sub_state = Intro;
                            game_state = PostBattle;
                            break;

                    }
                    
                    break;
                
                case PostBattle:
                    window.Draw(stage.thumb);
                    window.Draw(fade90);

                    string winner_text;
                    if (winner == Program.Drawn) winner_text = "Drawn";
                    else winner_text = "Player " + winner + " wins";

                    UI.Instance.DrawText(window, winner_text, 0, -100, spacing: Config.spacing_medium, size: 1f, textureName: "default medium");
                    UI.Instance.DrawText(window, "Rematch", 0, 0, spacing: Config.spacing_medium, size: 1f, textureName: pointer == 0 ? "default medium hover" : "default medium");
                    UI.Instance.DrawText(window, "Menu", 0, 20, spacing: Config.spacing_medium, size: 1f, textureName: pointer == 1 ? "default medium hover" : "default medium");
                    UI.Instance.DrawText(window, "Exit", 0, 40, spacing: Config.spacing_medium, size: 1f, textureName: pointer == 2 ? "default medium red" : "default medium");

                    // Change option
                    if (InputManager.Instance.Key_down("Up") && pointer > 0) {
                        pointer -= 1;
                    } else if (InputManager.Instance.Key_down("Down") && pointer < 2) {
                        pointer += 1;
                    }

                    // Do option
                    if (pointer == 0 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { // rematch
                        camera.SetChars(stage.character_A, stage.character_B);
                        camera.SetLimits(stage.length, stage.height);
                        stage.LockPlayers();
                        game_state = Battle;

                    } else if (pointer == 1 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { // MENU 
                        stage.UnloadStage();
                        game_state = SelectStage;
                    } else if (pointer == 2 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) {
                        window.Close();
                    }

                    break;
                
                case Settings:
                    window.Draw(settings_bg);
                    UI.Instance.DrawText(window, "Settings", -80, -107, spacing: Config.spacing_medium);
                    //0
                    UI.Instance.DrawText(window, "Main volume", -170, -74, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 0 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + Config.Main_Volume.ToString() + "% >", 0, -74, spacing: Config.spacing_small, textureName: pointer == 0 ? "default small red" : "default small");
                    //1
                    UI.Instance.DrawText(window, "Music volume", -170, -64, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 1 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + Config._music_volume.ToString() + "% >", 0, -64, spacing: Config.spacing_small, textureName: pointer == 1 ? "default small red" : "default small");
                    //2
                    UI.Instance.DrawText(window, "V-sync", -170, -54, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 2 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + (Config.Vsync ? "on" : "off") + " >", 0, -54, spacing: Config.spacing_small, textureName: pointer == 2 ? "default small red" : "default small");
                    //3
                    UI.Instance.DrawText(window, "Round Lenght", -170, -44, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 3 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + Config.RoundLength + "s >", 0, -44, spacing: Config.spacing_small, textureName: pointer == 3 ? "default small red" : "default small");
                    //4
                    UI.Instance.DrawText(window, "Match", -170, -34, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 4 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + Config.max_rounds + " rounds >", 0, -34, spacing: Config.spacing_small, textureName: pointer == 4 ? "default small red" : "default small");
                    //5
                    UI.Instance.DrawText(window, "Hitstop", -170, -24, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 5 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + (Config.hitStopTime == 0 ? "no hitstop" : Config.hitStopTime + " frames") + " >", 0, -24, spacing: Config.spacing_small, textureName: pointer == 5 ? "default small red" : "default small");
                    //6
                    UI.Instance.DrawText(window, "Input window", -170, -14, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 6 ? "default small hover" : "default small");
                    UI.Instance.DrawText(window, "< " + (Config.inputWindowTime == 1 ? "frame perfect" : Config.inputWindowTime + " frames") + " >", 0, -14, spacing: Config.spacing_small, textureName: pointer == 6 ? "default small red" : "default small");
                    //7
                    UI.Instance.DrawText(window, "Back", -80, 74, spacing: Config.spacing_medium, textureName: pointer == 7 ? "default medium red" : "default medium");

                    // Change option 
                    if (InputManager.Instance.Key_down("Up") && pointer > 0) {
                        pointer -= 1;
                    } else if (InputManager.Instance.Key_down("Down") && pointer < 7) {
                        pointer += 1;
                    }

                    // Do option
                    if (pointer == 0) { 
                        if (InputManager.Instance.Key_down("Left") && Config.Main_Volume > 0) Config.Main_Volume -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config.Main_Volume < 100) Config.Main_Volume += 1;

                    } else if (pointer == 1) { 
                        if (InputManager.Instance.Key_down("Left") && Config._music_volume > 0) Config._music_volume -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config._music_volume < 100) Config._music_volume += 1;

                    } else if (pointer == 2 && (InputManager.Instance.Key_down("Left") || InputManager.Instance.Key_down("Right"))) { 
                        Config.Vsync = !Config.Vsync;
                        window.SetVerticalSyncEnabled(Config.Vsync);

                    } else if (pointer == 3) { 
                        if (InputManager.Instance.Key_down("Left") && Config.RoundLength > 1) Config.RoundLength -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config.RoundLength < 99) Config.RoundLength += 1;

                    } else if (pointer == 4) { 
                        if (InputManager.Instance.Key_down("Left") && Config.max_rounds > 1) Config.max_rounds -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config.max_rounds < 5) Config.max_rounds += 1;

                    } else if (pointer == 5) { 
                        if (InputManager.Instance.Key_down("Left") && Config.hitStopTime > 0) Config.hitStopTime -= 1;
                        else if (InputManager.Instance.Key_down("Right")) Config.hitStopTime += 1;

                    } else if (pointer == 6) { 
                        if (InputManager.Instance.Key_down("Left") && Config.inputWindowTime > 1) Config.inputWindowTime -= 1;
                        else if (InputManager.Instance.Key_down("Right")) Config.inputWindowTime += 1;

                    } else if (pointer == 7 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { 
                        Config.SaveToFile();
                        game_state = SelectStage;
                        pointer = 0;
                    }
                    break;
            }

            // Finally
            last_frame_time = frametimer.Elapsed.TotalMilliseconds/1000;
            window.Display();
            window.Clear();
        }
    }

    public static void StageLoader() { 
        stage.LoadStage();
        stage.LoadCharacters(charA_selected, charB_selected);
        camera.SetChars(stage.character_A, stage.character_B);
        camera.SetLimits(stage.length, stage.height);
        loading = false;
        pointer = 0;
        game_state = Battle;
    }
}

