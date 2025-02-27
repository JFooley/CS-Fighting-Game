using Stage_Space;
using Animation_Space;


public class MakotoDojo : Stage {
    public MakotoDojo()
        : base("Makoto Dojo", 530, 896, 496, "Assets/stages/Makoto Dojo/sprites", "Assets/stages/Makoto Dojo/sound")
    {

    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0};

        var animations = new Dictionary<string, Animation> {
            { "Default", new Animation(defaultFrames, "Default", 1)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}