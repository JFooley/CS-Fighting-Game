﻿using Input_Space;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Stage_Space;
using UI_space;
using System.Diagnostics;
using Character_Space;

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

public static class Program {
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
    public const int Controls = 8;

    // Battle States
    public const int RoundStart = 1;
    public const int Battling = 2;
    public const int RoundEnd = 3;
    public const int MatchEnd = 4;

    // State holders
    public static int game_state;
    public static int return_state;
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
    private static int pointer = 0;
    private static string charA_selected = null;
    private static string charB_selected = null;
    private static int pointer_charA = 0;
    private static int pointer_charB = 0;
    public static bool loading = false;
    public static double last_frame_time = 0;

    // Shaders
    public static Shader colorTinterShader;
    public static Shader colorFillShader;
    public static Shader hueChange;

    // Data
    private static List<Stage> stages;
    private static List<Character> characters;
    private static Fireball fb = new Fireball();
    private static Hitspark hs = new Hitspark();
    private static Particle pt = new Particle();

    public static void Main() {  
        Config.LoadFromFile();
        
        // Necessary infos
        game_state = Intro;
        sub_state = Intro;

        // Crie uma janela
        if (Config.Fullscreen == true) window = new RenderWindow(new VideoMode(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height), Config.GameTitle, Styles.None);
        else window = new RenderWindow(new VideoMode(Config.RenderWidth*3, Config.RenderHeight*3), Config.GameTitle, Styles.Default);
        window.Closed += (sender, e) => window.Close();
        window.SetFramerateLimit(Config.Framerate);
        window.SetVerticalSyncEnabled(Config.Vsync);
        window.SetMouseCursorVisible(false);
        
        // Cria uma view
        window.SetView(view);

        // Inicializações
        InputManager.Initialize(autoDetectDevice: true);
        camera = Camera.GetInstance(window);
        frametimer = new Stopwatch();

        // Carregamento de shaders
        colorTinterShader = new Shader(null, null, "Assets/shaders/color_tinter.frag");
        colorFillShader = new Shader(null, null, "Assets/shaders/color_fill.frag");
        hueChange = new Shader(null, null, "Assets/shaders/hue_change.frag");

        // Carregamento de texturas de fontes
        UI.Instance.LoadFonts();
        
        // Carregamento dos personagens
        characters = new List<Character> {
            new Ken(),
            new Psylock(),
        };

        // Carregamento dos stages
        stages = new List<Stage> {
            new Stage("Random", "Assets/ui/random.png"),
            new BurningDojo(),
            new MidnightDuel(),
            new NightAlley(),
            new NYCSubway(),
            new RindoKanDojo(),
            new TheSavana(),
            new JapanFields(),
            new Stage("Settings", "Assets/ui/settings.png"),
        };
        stage = stages[0];

        // Visuals
        Sprite main_bg = new Sprite(new Texture("Assets/ui/title.png"));
        Sprite settings_bg = new Sprite(new Texture("Assets/ui/settings_bg.png"));
        Sprite char_bg = new Sprite(new Texture("Assets/ui/bgchar.png"));
        Sprite stage_bg = new Sprite();

        Sprite frame = new Sprite(new Texture("Assets/ui/frame.png"));
        Sprite fade90 = new Sprite(new Texture("Assets/ui/90fade.png"));
        Sprite controls = new Sprite(new Texture("Assets/ui/controls.png"));

        Sprite fight_logo = new Sprite(new Texture("Assets/ui/fight.png"));
        Sprite timesup_logo = new Sprite(new Texture("Assets/ui/timesup.png"));
        Sprite KO_logo = new Sprite(new Texture("Assets/ui/ko.png"));
        Sprite fslogo = new Sprite(new Texture("Assets/ui/fs.png"));

        Sprite sprite_A = new Sprite(characters[pointer_charA].thumb);
        Sprite sprite_B = new Sprite(characters[pointer_charB].thumb);

        while (window.IsOpen) {
            window.DispatchEvents();
            InputManager.Instance.Update();
            UI.Instance.Update();
            camera.Update();
            frametimer.Restart();

            switch (game_state)
            {
                case Intro:
                    if (UI.Instance.counter % 20 == 0) pointer = pointer < 3 ? pointer + 1 : 0;
                    fslogo.Position = new Vector2f(10, 139);

                    window.Draw(controls);
                    // window.Draw(fade90);
                    window.Draw(fslogo);

                    UI.Instance.DrawText(string.Concat(Enumerable.Repeat(".", pointer)), -122, 68, alignment: "left", spacing: -24);

                    if (!loading)
                    {
                        Thread main_loader = new Thread(MainLoader);
                        main_loader.Start();
                        loading = true;
                    }
                    break;

                case MainMenu:
                    window.Draw(main_bg);
                    UI.Instance.DrawText("by JFooley", 0, 76, spacing: Config.spacing_small - 1, textureName: "default small");
                    if (UI.Instance.blink2Hz || InputManager.Instance.Key_hold("Start")) UI.Instance.DrawText("press start", 0, 50, spacing: Config.spacing_medium, size: 1f, textureName: InputManager.Instance.Key_hold("Start") ? "default medium click" : "default medium white");

                    if (InputManager.Instance.Key_up("Start"))
                    {
                        game_state = SelectStage;
                        pointer = 0;
                    }
                    break;

                case SelectStage:
                    window.Draw(stages[pointer].thumb);
                    window.Draw(frame);

                    // draw texts
                    UI.Instance.DrawText(stages[pointer].name, 0, -80, spacing: Config.spacing_medium, textureName: InputManager.Instance.Key_hold("A") || InputManager.Instance.Key_hold("B") || InputManager.Instance.Key_hold("C") || InputManager.Instance.Key_hold("D") ? "default medium click" : "default medium white");
                    UI.Instance.DrawText(Program.player1_wins.ToString(), -Config.RenderWidth / 2, -Config.RenderHeight / 2, spacing: Config.spacing_medium, textureName: "default medium", alignment: "left");
                    UI.Instance.DrawText(Program.player2_wins.ToString(), Config.RenderWidth / 2, -Config.RenderHeight / 2, spacing: Config.spacing_medium, textureName: "default medium", alignment: "right");

                    UI.Instance.DrawText("E", -194, 67, spacing: Config.spacing_small, textureName: "icons", alignment: "left");
                    UI.Instance.DrawText("Return", -182, 67, spacing: Config.spacing_small, alignment: "left", textureName: InputManager.Instance.Key_hold("LB") ? "default small click" : "default small");

                    if (InputManager.Instance.Key_down("Left") && pointer > 0)
                    {
                        pointer -= 1;

                    }
                    else if (InputManager.Instance.Key_down("Right") && pointer < stages.Count - 1)
                    {
                        pointer += 1;

                    }
                    else if (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D"))
                    {
                        if (stages[pointer].name == "Settings")
                        {
                            return_state = SelectStage;
                            game_state = Settings;

                        }
                        else
                        {
                            if (stages[pointer].name == "Random") pointer = Program.random.Next(1, stages.Count() - 2);
                            Program.stage = stages[pointer];
                            game_state = SelectChar;
                        }
                        pointer = 0;
                    }
                    else if (InputManager.Instance.Key_up("LB"))
                    {
                        game_state = MainMenu;
                    }
                    break;

                case SelectChar:
                    window.Draw(char_bg);

                    // Setup sprites texture
                    sprite_A.Texture = characters[pointer_charA].thumb;
                    sprite_B.Texture = characters[pointer_charB].thumb;

                    // Draw Shadows
                    sprite_A.Scale = new Vector2f(1f, 1f);
                    sprite_A.Position = new Vector2f(Camera.Instance.X - 81 - sprite_A.GetLocalBounds().Width / 2, Camera.Instance.Y - 20 - sprite_A.GetLocalBounds().Height / 2);
                    window.Draw(sprite_A, new RenderStates(colorFillShader));

                    sprite_B.Scale = new Vector2f(-1f, 1f);
                    sprite_B.Position = new Vector2f(Camera.Instance.X + 73 + sprite_B.GetLocalBounds().Width / 2, Camera.Instance.Y - 20 - sprite_B.GetLocalBounds().Height / 2);
                    window.Draw(sprite_B, new RenderStates(colorFillShader));

                    // Draw main sprite
                    colorTinterShader.SetUniform("tintColor", new SFML.Graphics.Glsl.Vec3(0, 0, 0));
                    colorTinterShader.SetUniform("intensity", 0.75f);

                    sprite_A.Position = new Vector2f(Camera.Instance.X - 77 - sprite_B.GetLocalBounds().Width / 2, Camera.Instance.Y - 20 - sprite_B.GetLocalBounds().Height / 2);
                    if (charA_selected != null) window.Draw(sprite_A, new RenderStates(colorTinterShader));
                    else window.Draw(sprite_A);

                    sprite_B.Position = new Vector2f(Camera.Instance.X + 77 + sprite_B.GetLocalBounds().Width / 2, Camera.Instance.Y - 20 - sprite_B.GetLocalBounds().Height / 2);
                    sprite_B.Scale = new Vector2f(-1f, 1f);
                    if (charB_selected != null) window.Draw(sprite_B, new RenderStates(colorTinterShader));
                    else window.Draw(sprite_B);

                    // Draw texts
                    if (charA_selected == null) UI.Instance.DrawText("<  >", -77, -16);
                    if (charB_selected == null) UI.Instance.DrawText("<  >", +77, -16);

                    UI.Instance.DrawText(player1_wins.ToString(), 0, 63, alignment: "right");
                    UI.Instance.DrawText(player2_wins.ToString(), 0, 63, alignment: "left");

                    UI.Instance.DrawText(characters[pointer_charA].name, -77, 45, spacing: Config.spacing_small, textureName: "default small");
                    UI.Instance.DrawText(characters[pointer_charB].name, +77, 45, spacing: Config.spacing_small, textureName: "default small");

                    UI.Instance.DrawText("E", -194, 67, spacing: Config.spacing_small, textureName: "icons", alignment: "left");
                    UI.Instance.DrawText("Return", -182, 67, spacing: Config.spacing_small, alignment: "left", textureName: InputManager.Instance.Key_hold("LB") ? "default small click" : "default small");

                    // Chose option A
                    if (InputManager.Instance.Key_down("Left", player: 1) && pointer_charA > 0 && charA_selected == null)
                    {
                        pointer_charA -= 1;
                    }
                    else if (InputManager.Instance.Key_down("Right", player: 1) && pointer_charA < characters.Count - 1 && charA_selected == null)
                    {
                        pointer_charA += 1;
                    }
                    else if (InputManager.Instance.Key_down("A", player: 1) || InputManager.Instance.Key_down("B", player: 1) || InputManager.Instance.Key_down("C", player: 1) || InputManager.Instance.Key_down("D", player: 1))
                    {
                        charA_selected = characters[pointer_charA].name;
                    }

                    // Chose option B
                    if (InputManager.Instance.Key_down("Left", player: 2) && pointer_charB > 0 && charB_selected == null)
                    {
                        pointer_charB -= 1;
                    }
                    else if (InputManager.Instance.Key_down("Right", player: 2) && pointer_charB < characters.Count - 1 && charB_selected == null)
                    {
                        pointer_charB += 1;
                    }
                    else if (InputManager.Instance.Key_down("A", player: 2) || InputManager.Instance.Key_down("B", player: 2) || InputManager.Instance.Key_down("C", player: 2) || InputManager.Instance.Key_down("D", player: 2))
                    {
                        charB_selected = characters[pointer_charB].name;
                    }

                    // Return option
                    else if (InputManager.Instance.Key_up("LB"))
                    {
                        charB_selected = null;
                        charA_selected = null;
                        game_state = SelectStage;
                    }

                    // Ends when chars are selected
                    if (charA_selected != null && charB_selected != null && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")))
                    {
                        game_state = LoadScreen;
                    }
                    break;

                case LoadScreen:
                    stage.LoadStage();
                    stage.LoadCharacters(charA_selected, charB_selected);
                    camera.SetChars(stage.character_A, stage.character_B);
                    camera.SetLimits(stage.length, stage.height);

                    loading = false;
                    pointer = 0;
                    charA_selected = null;
                    charB_selected = null;
                    pointer_charA = 0;
                    pointer_charB = 0;

                    game_state = Battle;
                    break;

                case Battle:
                    stage.Update();

                    switch (sub_state)
                    {
                        case Intro:
                            stage.SetMusicVolume();
                            stage.StopRoundTime();
                            stage.ResetTimer();
                            if (stage.character_A.CurrentState == "Idle" && stage.character_B.CurrentState == "Idle")
                            { // Espera até a animação de intro finalizar
                                sub_state = RoundStart;
                            }
                            break;

                        case RoundStart: // Inicia a round
                            if (stage.CheckTimer(2))
                            {
                                stage.ResetRoundTime();
                                stage.StartRoundTime();
                                stage.ReleasePlayers();
                                sub_state = Battling;
                            }
                            else if (stage.CheckTimer(1))
                            {
                                fight_logo.Position = new Vector2f(Program.camera.X - 89, Program.camera.Y - 54);
                                window.Draw(fight_logo);
                            }
                            else UI.Instance.DrawText("Ready?", 0, -30, spacing: Config.spacing_medium, textureName: "default medium white");

                            break;

                        case Battling: // Durante a batalha
                            if (stage.CheckRoundEnd())
                            {
                                sub_state = RoundEnd;
                                stage.StopRoundTime();
                                stage.ResetTimer();
                            }
                            break;

                        case RoundEnd: // Fim de round
                            if (stage.GetTimerValue() < 3)
                            {
                                if (stage.character_A.LifePoints.X <= 0 || stage.character_B.LifePoints.X <= 0)
                                {
                                    KO_logo.Position = new Vector2f(Program.camera.X - 75, Program.camera.Y - 54);
                                    window.Draw(KO_logo);
                                }
                                else
                                {
                                    timesup_logo.Position = new Vector2f(Program.camera.X - 131, Program.camera.Y - 55);
                                    window.Draw(timesup_logo);
                                }
                            }
                            if (stage.CheckTimer(4))
                            {
                                stage.LockPlayers();
                                stage.ResetTimer();
                                if (stage.CheckMatchEnd())
                                {
                                    sub_state = MatchEnd;
                                }
                                else
                                {
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

                    UI.Instance.DrawText(winner_text, 0, -100, spacing: Config.spacing_medium, size: 1f, textureName: "default medium");
                    UI.Instance.DrawText("Rematch", 0, 0, spacing: Config.spacing_medium, size: 1f, textureName: pointer == 0 ? "default medium hover" : "default medium");
                    UI.Instance.DrawText("Change stage", 0, 20, spacing: Config.spacing_medium, size: 1f, textureName: pointer == 1 ? "default medium hover" : "default medium");
                    UI.Instance.DrawText("Exit game", 0, 40, spacing: Config.spacing_medium, size: 1f, textureName: pointer == 2 ? "default medium red" : "default medium");

                    // Change option
                    if (InputManager.Instance.Key_down("Up") && pointer > 0)
                    {
                        pointer -= 1;
                    }
                    else if (InputManager.Instance.Key_down("Down") && pointer < 2)
                    {
                        pointer += 1;
                    }

                    // Do option
                    if (pointer == 0 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")))
                    { // rematch
                        camera.SetChars(stage.character_A, stage.character_B);
                        camera.SetLimits(stage.length, stage.height);
                        stage.LockPlayers();
                        game_state = Battle;

                    }
                    else if (pointer == 1 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")))
                    { // MENU 
                        stage.UnloadStage();
                        game_state = SelectStage;
                    }
                    else if (pointer == 2 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")))
                    {
                        window.Close();
                    }

                    break;

                case Settings:
                    Camera.Instance.UnlockCamera();
                    window.Draw(settings_bg);

                    UI.Instance.DrawText("Settings", -80, -107, spacing: Config.spacing_medium);
                    //0
                    UI.Instance.DrawText("Main volume", -170, -74, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 0 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + Config.Main_Volume.ToString() + "% >", 0, -74, spacing: Config.spacing_small, textureName: pointer == 0 ? "default small red" : "default small");
                    //1
                    UI.Instance.DrawText("Music volume", -170, -64, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 1 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + Config._music_volume.ToString() + "% >", 0, -64, spacing: Config.spacing_small, textureName: pointer == 1 ? "default small red" : "default small");
                    //2
                    UI.Instance.DrawText("V-sync", -170, -54, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 2 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + (Config.Vsync ? "on" : "off") + " >", 0, -54, spacing: Config.spacing_small, textureName: pointer == 2 ? "default small red" : "default small");
                    //3
                    UI.Instance.DrawText("Window mode", -170, -44, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 3 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + (Config.Fullscreen ? "Fullscreen" : "Windowed") + " >", 0, -44, spacing: Config.spacing_small, textureName: pointer == 3 ? "default small red" : "default small");
                    //4
                    UI.Instance.DrawText("Round Lenght", -170, -34, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 4 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + Config.RoundLength + "s >", 0, -34, spacing: Config.spacing_small, textureName: pointer == 4 ? "default small red" : "default small");
                    //5
                    UI.Instance.DrawText("Match", -170, -24, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 5 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< First to " + Config.max_rounds + " >", 0, -24, spacing: Config.spacing_small, textureName: pointer == 5 ? "default small red" : "default small");
                    //6
                    UI.Instance.DrawText("Hitstop", -170, -14, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 6 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + (Config.hitStopTime == Config.defaultHitStopTime ? "Default" : Config.hitStopTime + " frames") + " >", 0, -14, spacing: Config.spacing_small, textureName: pointer == 6 ? "default small red" : "default small");
                    //7
                    UI.Instance.DrawText("Input window", -170, -04, alignment: "left", spacing: Config.spacing_small, textureName: pointer == 7 ? "default small hover" : "default small");
                    UI.Instance.DrawText("< " + (Config.inputWindowTime == 1 ? "frame perfect" : Config.inputWindowTime + " frames") + " >", 0, -04, spacing: Config.spacing_small, textureName: pointer == 7 ? "default small red" : "default small");
                    //8
                    UI.Instance.DrawText("Back", -80, 74, spacing: Config.spacing_medium, textureName: pointer == 8 ? "default medium red" : "default medium");

                    // Change option 
                    if (InputManager.Instance.Key_down("Up") && pointer > 0)
                    {
                        pointer -= 1;
                    }
                    else if (InputManager.Instance.Key_down("Down") && pointer < 8)
                    {
                        pointer += 1;
                    }

                    // Do option
                    if (pointer == 0)
                    {
                        if (InputManager.Instance.Key_down("Left") && Config.Main_Volume > 0) Config.Main_Volume -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config.Main_Volume < 100) Config.Main_Volume += 1;

                    }
                    else if (pointer == 1)
                    {
                        if (InputManager.Instance.Key_down("Left") && Config._music_volume > 0) Config._music_volume -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config._music_volume < 100) Config._music_volume += 1;

                    }
                    else if (pointer == 2 && (InputManager.Instance.Key_down("Left") || InputManager.Instance.Key_down("Right")))
                    {
                        Config.Vsync = !Config.Vsync;
                        window.SetVerticalSyncEnabled(Config.Vsync);

                    }
                    else if (pointer == 3 && (InputManager.Instance.Key_down("Left") || InputManager.Instance.Key_down("Right")))
                    {
                        Config.Fullscreen = !Config.Fullscreen;
                        window.Close();
                        if (Config.Fullscreen == true) window = new RenderWindow(new VideoMode(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height), Config.GameTitle, Styles.None);
                        else window = new RenderWindow(new VideoMode(Config.RenderWidth * 3, Config.RenderHeight * 3), Config.GameTitle, Styles.Default);
                        window.Closed += (sender, e) => window.Close();
                        window.SetFramerateLimit(Config.Framerate);
                        window.SetVerticalSyncEnabled(Config.Vsync);
                        window.SetMouseCursorVisible(false);
                        window.SetView(view);
                        camera.SetWindow(window);

                    }
                    else if (pointer == 4)
                    {
                        if (InputManager.Instance.Key_down("Left") && Config.RoundLength > 1) Config.RoundLength -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config.RoundLength < 99) Config.RoundLength += 1;

                    }
                    else if (pointer == 5)
                    {
                        if (InputManager.Instance.Key_down("Left") && Config.max_rounds > 1) Config.max_rounds -= 1;
                        else if (InputManager.Instance.Key_down("Right") && Config.max_rounds < 5) Config.max_rounds += 1;

                    }
                    else if (pointer == 6)
                    {
                        if (InputManager.Instance.Key_down("Left") && Config.hitStopTime > 0) Config.hitStopTime -= 1;
                        else if (InputManager.Instance.Key_down("Right")) Config.hitStopTime += 1;

                    }
                    else if (pointer == 7)
                    {
                        if (InputManager.Instance.Key_down("Left") && Config.inputWindowTime > 1) Config.inputWindowTime -= 1;
                        else if (InputManager.Instance.Key_down("Right")) Config.inputWindowTime += 1;

                    }
                    else if (pointer == 8 && (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")))
                    {
                        Config.SaveToFile();
                        Camera.Instance.LockCamera();
                        game_state = return_state;
                        pointer = 0;
                    }
                    break;

                case Controls:
                    if (Camera.GetInstance().isLocked) Camera.Instance.UnlockCamera();
                    window.Draw(controls);

                    if (InputManager.Instance.Key_up("A") || InputManager.Instance.Key_up("B") || InputManager.Instance.Key_up("C") || InputManager.Instance.Key_up("D")) {
                        game_state = return_state;
                        if (return_state == Battle) Camera.Instance.LockCamera();
                    }
                    break;
            }

            // Finally
            last_frame_time = frametimer.Elapsed.TotalMilliseconds/1000;
            window.Display();
            window.Clear();
        }
    }

    public static void MainLoader() { 
        foreach (var character in characters) {
            if (character.GetType() == typeof(Character)) continue;
            character.LoadSpriteImages();
            character.LoadSounds();
        }

        foreach (var stage in stages) {
            if (stage.GetType() == typeof(Stage)) continue;
            stage.LoadSpriteImages();
            stage.LoadSounds();
        }

        fb.LoadSpriteImages();
        fb.LoadSounds();

        hs.LoadSpriteImages();
        hs.LoadSounds();

        pt.LoadSpriteImages();
        pt.LoadSounds();
        
        loading = false;
        game_state = MainMenu;
    }
}

