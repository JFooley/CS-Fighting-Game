public static class Config {
    // Window
    public const string GameTitle = "Fighting Game CS";
    public const int WindowWidth = 1280;
    public const int WindowHeight = 720;
    public const int RenderWidth = 384;
    public const int RenderHeight = 216;

    public const bool Vsync = true;
    public const int Framerate = 60;


    public const int maxDistance = 350;
    public const int resetFrames = 20;
    public const int inputWindowTime = 4;

    // Audio
    private static float _main_volume = 50f;
    private static float _character_volume = 80f;
    private static float _music_volume = 70f;
    private static float _effect_volume = 100f;
    public static float Main_Volume
    {
        get { return _main_volume; }
        set { _main_volume = value; }
    }
    public static float Character_Volume
    {
        get { return _character_volume * (_main_volume / 100); }
        set { _character_volume = value; }
    }
    public static float Music_Volume
    {
        get { return _music_volume  * (_main_volume / 100); }
        set { _music_volume = value; }
    }
    public static float Effect_Volume
    {
        get { return _effect_volume * (_main_volume / 100); }
        set { _effect_volume = value; }
    }

    public static float Gravity = 2450f / (Framerate*Framerate);

    // Battle constants 32 38 48
    public static int RoundLength = 90;
    public static int hitStopTime = 10;
    public static int max_rounds = 2;

    public static float light_pushback = 3f;
    public static float medium_pushback = 4f;
    public static float heavy_pushback = 5.5f;
}