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
    public const int Drawn = 0;
    public const int Player1 = 1;
    public const int Player2 = 2;

    public const int Intro = 0;
    public const int MainMenu = 1;
    public const int SelectStage = 2;
    public const int SelectChar = 3;
    public const int LoadScreen = 4;
    public const int Battle = 5;
    public const int PostBattle = 6;
    public const int ConfigScreen = 7;

    public const int RoundStart = 1;
    public const int Battling = 2;
    public const int RoundEnd = 3;
    public const int MatchEnd = 4;

    public static int game_state;
    public static int sub_state;
    public static Camera camera;
    public static Random random = new Random();

    public static int player1_wins = 0;
    public static int player2_wins = 0;
    public static int winner = 0;

    public static SFML.Graphics.View view = new SFML.Graphics.View(new FloatRect(0, 0, Config.RenderWidth, Config.RenderHeight));

    public static void Main() {  
        // Necessary infos
        game_state = Intro;
        sub_state = Intro;

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(Config.WindowWidth, Config.WindowHeight), Config.GameTitle);
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(Config.Framerate);
        window.SetVerticalSyncEnabled(Config.Vsync);
        
        // Cria uma view
        window.SetView(view);

        // Inicializações
        InputManager.Initialize(autoDetectDevice: true);
        camera = Camera.GetInstance(window);

        // Carregamento de texturas de fontes
        BitmapFont.Load("default medium", "Assets/fonts/default medium.png");
        BitmapFont.Load("default medium grad", "Assets/fonts/default medium grad.png");
        BitmapFont.Load("default medium white", "Assets/fonts/default medium white.png");
        BitmapFont.Load("default medium red", "Assets/fonts/default medium red.png");
        BitmapFont.Load("default medium click", "Assets/fonts/default medium click.png");
        BitmapFont.Load("default medium hover", "Assets/fonts/default medium hover.png");

        BitmapFont.Load("default small", "Assets/fonts/default small.png");
        BitmapFont.Load("default small grad", "Assets/fonts/default small grad.png");
        BitmapFont.Load("default small white", "Assets/fonts/default small white.png");
        BitmapFont.Load("default small red", "Assets/fonts/default small red.png");
        BitmapFont.Load("default small click", "Assets/fonts/default small click.png");
        BitmapFont.Load("default small hover", "Assets/fonts/default small hover.png");

        BitmapFont.Load("1", "Assets/fonts/font1.png");
        BitmapFont.Load("icons", "Assets/fonts/icons.png");

        UI.Instance.LoadCharacterSprites(32, "default medium");
        UI.Instance.LoadCharacterSprites(32, "default medium grad");
        UI.Instance.LoadCharacterSprites(32, "default medium white");
        UI.Instance.LoadCharacterSprites(32, "default medium red");
        UI.Instance.LoadCharacterSprites(32, "default medium click");
        UI.Instance.LoadCharacterSprites(32, "default medium hover");

        UI.Instance.LoadCharacterSprites(32, "default small");
        UI.Instance.LoadCharacterSprites(32, "default small grad");
        UI.Instance.LoadCharacterSprites(32, "default small white");
        UI.Instance.LoadCharacterSprites(32, "default small red");
        UI.Instance.LoadCharacterSprites(32, "default small click");
        UI.Instance.LoadCharacterSprites(32, "default small hover");

        UI.Instance.LoadCharacterSprites(32, "1");
        UI.Instance.LoadCharacterSprites(32, "icons");

        // Instanciamento do stages
        List<Stage> stages = new List<Stage>{
            new Stage("Random", 0, 0, 0, "", "", "Assets/stages/thumb.png"),
            new BurningDojo(),
            new NYCSubway(),
            new MidnightDuel(),
            new NightAlley(),
            new TheSavana(),
            new DivineSaloon(),
            new JapanFields(),
        };
        Stage stage = stages[0];

        int pointer = 0;
        int charA_selected = 0;
        int charB_selected = 0;

        // Menu e SelectStage visuals
        Sprite main_screen = new Sprite(new Texture("Assets/ui/title.png"));
        Sprite frame = new Sprite(new Texture("Assets/ui/frame.png"));
        Sprite fade90 = new Sprite(new Texture("Assets/ui/90fade.png"));
        Sprite fight_logo = new Sprite(new Texture("Assets/ui/fight.png"));
        Sprite timesup_logo = new Sprite(new Texture("Assets/ui/timesup.png"));
        Sprite KO_logo = new Sprite(new Texture("Assets/ui/ko.png"));

        while (window.IsOpen) {
            window.DispatchEvents();
            InputManager.Instance.Update();
            camera.Update();

            switch (game_state) {
                case Intro:
                    game_state = MainMenu;
                    break;

                case MainMenu:
                    window.Draw(main_screen);
                    UI.Instance.DrawText(window, "press start", 0, 50, spacing: Config.spacing_medium, size: 1f, textureName: InputManager.Instance.Key_hold("Start") ? "default medium click" : "default medium white");

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
                        game_state = SelectChar;
                        if (stages[pointer].name == "Random") pointer = Program.random.Next(1, stages.Count()-1);
                    } 
                    break;
                
                case SelectChar:
                    charA_selected = 0;
                    charB_selected = 0;
                    game_state = LoadScreen;
                    break;
                
                case LoadScreen:
                    // Seleciona os chars e o stage
                    stage = stages[pointer];
                    
                    // Da load no Stage
                    stage.LoadStage();

                    // Da load nos personagens
                    stage.LoadCharacters(charA_selected, charB_selected);

                    // Configura a camera
                    camera.SetChars(stage.character_A, stage.character_B);
                    camera.SetLimits(stage.length, stage.height);
                    
                    game_state = Battle;
                    pointer = 0;
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
                                stage.TogglePlayers();
                                sub_state = Battling;
                            } else if (stage.CheckTimer(1)) {
                                fight_logo.Position = new Vector2f(Program.camera.X - 89, Program.camera.Y - 54);
                                window.Draw(fight_logo);
                            } else UI.Instance.DrawText(window, "Ready?", 0, -30, size: 2, spacing: Config.spacing_medium * 2, textureName: "default medium");

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
                                stage.TogglePlayers();
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
                        game_state = Battle;

                    } else if (pointer == 1 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) { // MENU 
                        stage.UnloadStage();
                        game_state = SelectStage;
                    } else if (pointer == 2 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))) {
                        window.Close();
                    }

                    break;
                
                case ConfigScreen:
                    
                    break;
            }

            // Finally
            window.Display();
            window.Clear();
        }
    }
}

