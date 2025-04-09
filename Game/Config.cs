using Newtonsoft.Json;

public static class Config {
    // Window
    public const string GameTitle = "Project FS";
    public const int RenderWidth = 384;
    public const int RenderHeight = 216;
    public const int Framerate = 60;

    public static bool Fullscreen = true;
    public static bool Vsync = true;
    public static int inputWindowTime = 4;    
    
    // Battle
    public static int RoundLength = 90;
    public static int hitStopTime = 8;
    public static int max_rounds = 2;

    // Audio
    public static float _main_volume = 100f;
    public static float _character_volume = 80f;
    public static float _music_volume = 100f;
    public static float _effect_volume = 100f;

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

    // Battle constants 32 38 48
    public const float light_pushback = 3f;
    public const float medium_pushback = 4f;
    public const float heavy_pushback = 5.5f;
    public const int maxDistance = 350;
    public const int resetFrames = 20;
    public const float Gravity = 2450f / (Framerate*Framerate);

    // Text
    public const int spacing_small = -26; 
    public const int spacing_medium = -23;

    public static void SaveToFile(string filePath = "Assets/config.json") {
        var configData = new
        {
            Fullscreen,
            Vsync,
            inputWindowTime,
            _main_volume,
            _character_volume,
            _music_volume,
            _effect_volume,
            RoundLength,
            hitStopTime,
            max_rounds
        };

        string jsonString = JsonConvert.SerializeObject(configData, Formatting.Indented);
        File.WriteAllText(filePath, jsonString);
    }
    public static void LoadFromFile(string filePath = "Assets/config.json") {
        if (File.Exists(filePath)) {
            string jsonString = File.ReadAllText(filePath);
            var configData = JsonConvert.DeserializeObject<ConfigData>(jsonString);

            Fullscreen = configData.Fullscreen;
            Vsync = configData.Vsync;
            inputWindowTime = configData.inputWindowTime;
            _main_volume = configData._main_volume;
            _character_volume = configData._character_volume;
            _music_volume = configData._music_volume;
            _effect_volume = configData._effect_volume;
            RoundLength = configData.RoundLength;
            hitStopTime = configData.hitStopTime;
            max_rounds = configData.max_rounds;
        } else {
            Console.WriteLine("Config file not found. Using default settings.");
        }
    }

    private class ConfigData {
        public bool Fullscreen { get; set; }
        public bool Vsync { get; set; }
        public int inputWindowTime { get; set; }
        public float _main_volume { get; set; }
        public float _character_volume { get; set; }
        public float _music_volume { get; set; }
        public float _effect_volume { get; set; }
        public int RoundLength { get; set; }
        public int hitStopTime { get; set; }
        public int max_rounds { get; set; }
    }
}