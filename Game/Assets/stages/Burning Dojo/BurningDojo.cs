using Stage_Space;
using Animation_Space;


public class BurningDojo : Stage {
    public BurningDojo()
        : base("Burning Dojo", 120, 800, 336, "Assets/stages/Burning Dojo/sprites", "Assets/stages/Burning Dojo/sound")
    {

    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0, 1, 2, 3, 4, 5, 6, 7};

        var animations = new Dictionary<string, Animation> {
            { "Default", new Animation(defaultFrames, "Default", 15)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}