using Stage_Space;
using Animation_Space;


public class NYCSubway : Stage {
    
    public NYCSubway()
        : base("NYC Subway", 539, 912, 512, "Assets/stages/NYC Subway/sprites", "Assets/stages/NYC Subway/sound")
    {

    }

    public override void LoadStage() {
        var day = new List<int> {10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143};
        
        var animations = new Dictionary<string, Animation> {
            { "day", new Animation(day, "day", 30)},
        };

        this.CurrentState = "day";

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}