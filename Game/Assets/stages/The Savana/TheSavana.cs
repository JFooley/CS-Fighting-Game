using Stage_Space;
using Animation_Space;


public class TheSavana : Stage {
    public TheSavana()
        : base("The Savana", 530, 512, 512, "Assets/stages/The Savana/sprites", "Assets/stages/The Savana/sound", "Assets/stages/The Savana/thumb.png")
    {
        this.AmbientLight = new SFML.Graphics.Color(255, 240, 225, 255);
    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
        List<string> frames = defaultFrames.Select(i => i.ToString()).ToList();

        var animations = new Dictionary<string, State> {
            { "Default", new State(frames, "Default", 15)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}