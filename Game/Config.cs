public static class Config {
    // Window
    public const string GameTitle = "Fighting Game CS";
    public const int WindowWidth = 1280;
    public const int WindowHeight = 720;

    public const int Framerate = 60;

    // Audio
    private static float _main_volume = 100f;
    private static float _character_volume = 100f;
    private static float _music_volume = 80f;
    private static float _effect_volume = 80f;
    private static float _UI_volume = 100f;
    public static float Main_Volume
    {
        get { return _main_volume; }
        set { _main_volume = value; }
    }
    public static float Character_Volume
    {
        get { return _character_volume * _main_volume; }
        set { _character_volume = value; }
    }
    public static float Music_Volume
    {
        get { return _music_volume * _main_volume; }
        set { _music_volume = value; }
    }
    public static float Effect_Volume
    {
        get { return _effect_volume * _main_volume; }
        set { _effect_volume = value; }
    }

    // Others
}
