using Stage_Space;
using Animation_Space;


public class MidnightDuel : Stage {
    public MidnightDuel()
        : base("The Midnight Duel", 362, 800, 336, "Assets/stages/The Midnight Duel/sprites", "Assets/stages/The Midnight Duel/sound", "Assets/stages/The Midnight Duel/thumb.png")
    {
        this.AmbientLight = new SFML.Graphics.Color(210, 210, 255, 255);
    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0, 1, 2, 3};
        List<string> frames = defaultFrames.Select(i => i.ToString()).ToList();

        var animations = new Dictionary<string, Animation> {
            { "Default", new Animation(frames, "Default", 8)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}