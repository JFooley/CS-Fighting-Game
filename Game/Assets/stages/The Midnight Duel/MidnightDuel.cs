using Stage_Space;
using Animation_Space;


public class MidnightDuel : Stage {
    public MidnightDuel()
        : base("The Midnight Duel", 356, 800, 336, "Assets/stages/The Midnight Duel/sprites", "Assets/stages/The Midnight Duel/sound")
    {

    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0, 1, 2, 3};

        var animations = new Dictionary<string, Animation> {
            { "Default", new Animation(defaultFrames, "Default", 8)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}