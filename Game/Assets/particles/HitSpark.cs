using Character_Space;
using Animation_Space;
using Stage_Space;

public class Hitspark : Character {
    public Hitspark(string initialState, float startX, float startY, int facing, Stage stage = null)
        : base("Hitspark", initialState, startX, startY, "Assets/particles/sprites/Hitspark", "Assets/particles/sounds/Hitspark", stage) {
            this.team = 0;
            this.facing = facing;
        }

    public override void Load() {
        // Animations
        var onHit1 = new List<FrameData> { 
            new FrameData(11, 0, 0, new List<GenericBox> {}, "on_hit_1"),
            new FrameData(12, 0, 0, new List<GenericBox> {}),
            new FrameData(13, 0, 0, new List<GenericBox> {}),
            new FrameData(14, 0, 0, new List<GenericBox> {}),
            new FrameData(15, 0, 0, new List<GenericBox> {}),
            new FrameData(16, 0, 0, new List<GenericBox> {}),
            new FrameData(17, 0, 0, new List<GenericBox> {}),
            new FrameData(18, 0, 0, new List<GenericBox> {}),
            new FrameData(19, 0, 0, new List<GenericBox> {}),
            new FrameData(110, 0, 0, new List<GenericBox> {}),
        };

        var onHit2 = new List<FrameData> { 
            new FrameData(21, 0, 0, new List<GenericBox> {}, "on_hit_2"),
            new FrameData(22, 0, 0, new List<GenericBox> {}),
            new FrameData(23, 0, 0, new List<GenericBox> {}),
            new FrameData(24, 0, 0, new List<GenericBox> {}),
            new FrameData(25, 0, 0, new List<GenericBox> {}),
            new FrameData(26, 0, 0, new List<GenericBox> {}),
            new FrameData(27, 0, 0, new List<GenericBox> {}),
            new FrameData(28, 0, 0, new List<GenericBox> {}),
            new FrameData(29, 0, 0, new List<GenericBox> {}),
            new FrameData(210, 0, 0, new List<GenericBox> {}),
        };

        var onHit3 = new List<FrameData> { 
            new FrameData(41, 0, 0, new List<GenericBox> {}, "on_hit_3"),
            new FrameData(42, 0, 0, new List<GenericBox> {}),
            new FrameData(43, 0, 0, new List<GenericBox> {}),
            new FrameData(44, 0, 0, new List<GenericBox> {}),
            new FrameData(45, 0, 0, new List<GenericBox> {}),
            new FrameData(46, 0, 0, new List<GenericBox> {}),
            new FrameData(47, 0, 0, new List<GenericBox> {}),
            new FrameData(48, 0, 0, new List<GenericBox> {}),
            new FrameData(49, 0, 0, new List<GenericBox> {}),
            new FrameData(410, 0, 0, new List<GenericBox> {}),
        };

        var onBlock = new List<FrameData> {
            new FrameData(31, 0, 0, new List<GenericBox> {}, "on_block"),
            new FrameData(32, 0, 0, new List<GenericBox> {}),
            new FrameData(33, 0, 0, new List<GenericBox> {}),
            new FrameData(34, 0, 0, new List<GenericBox> {}),
            new FrameData(35, 0, 0, new List<GenericBox> {}),
            new FrameData(36, 0, 0, new List<GenericBox> {}),
        };

        // States
        var animations = new Dictionary<string, Animation> {
            {"OnHit1", new Animation(onHit1, "Remove", 60)},
            {"OnHit2", new Animation(onHit2, "Remove", 60)},
            {"OnHit3", new Animation(onHit3, "Remove", 60)},
            {"OnBlock", new Animation(onBlock, "Remove", 30)},
            {"Remove", new Animation(onBlock, "Remove", 60)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehave() {        
        if (this.CurrentState == "Remove") {
            this.remove = true;
        } 
    }
}