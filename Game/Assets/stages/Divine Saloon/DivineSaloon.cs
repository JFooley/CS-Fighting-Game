using Stage_Space;
using Animation_Space;


public class DivineSaloon : Stage {
    public DivineSaloon()
        : base("Divine Saloon", 530, 896, 496, "Assets/stages/Divine Saloon/sprites", "Assets/stages/Divine Saloon/sound")
    {

    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38,};

        var animations = new Dictionary<string, Animation> {
            { "Default", new Animation(defaultFrames, "Default", 15)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}