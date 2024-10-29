using Character_Space;
using Animation_Space;
using SFML.System;

public class Hitspark : Character {
    public Hitspark(string initialState, float startX, float startY, int team)
        : base("Hitspark", initialState, startX, startY, "Assets/particles/sprites", "Assets/particles/sounds") {
            this.team = team;
            this.LifePoints = new Vector2i(1, 1);
        }

    public override void Load() {
        // Animations
        var onHit1 = new List<FrameData> { 
            new FrameData(21, 0, 0, new List<GenericBox> {}),
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

        var onBlock1 = new List<FrameData> {
            new FrameData(31, 0, 0, new List<GenericBox> {}),
            new FrameData(32, 0, 0, new List<GenericBox> {}),
            new FrameData(33, 0, 0, new List<GenericBox> {}),
            new FrameData(34, 0, 0, new List<GenericBox> {}),
            new FrameData(35, 0, 0, new List<GenericBox> {}),
            new FrameData(36, 0, 0, new List<GenericBox> {}),
        };

        // States
        var animations = new Dictionary<string, Animation> {
            {"OnHit1", new Animation(onHit1, "OnHit1", 60)},
            {"OnBlock1", new Animation(onBlock1, "OnBlock1", 30)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehavior() {        
        if (this.CurrentAnimation.onLastFrame) this.LifePoints.X = -1;
    }
}