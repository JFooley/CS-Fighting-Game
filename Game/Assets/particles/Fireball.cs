using Character_Space;
using Animation_Space;
using SFML.System;
using Input_Space;

public class Fireball : Character {
    public Fireball(string initialState, float startX, float startY, int team)
        : base("Fireball", initialState, startX, startY, "Assets/particles/sprites", "Assets/particles/sounds") {
            this.team = team;
            this.LifePoints = new Vector2i(1, 1);
        }

    public override void Load() {
        // Animations
        var kenFB0 = new GenericBox(0, 139, 115, 163, 143);
        var kenFB1 = new GenericBox(1, 139, 115, 163, 143);
        var KenFireballFrames = new List<FrameData> { 
            new FrameData(1, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(2, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(3, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(4, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(5, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(6, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(7, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(8, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(9, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(10, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(11, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(12, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(13, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(14, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(15, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(16, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(17, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(18, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(19, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(20, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(21, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
        };

        // States
        var animations = new Dictionary<string, Animation> {
            {"Ken1", new Animation(KenFireballFrames, "Ken1", 20)},
            {"Ken2", new Animation(KenFireballFrames, "Ken2", 30)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehavior() {        
        switch (this.CurrentState) {
            case "Ken1":
                this.Position.X += 5 * this.facing;
                 break;

            case "Ken2":
                this.Position.X += 10 * this.facing;
                 break;

            default:
                break;
        }
    }

    public override void ImposeBehavior(Character target) {
        this.LifePoints.X = 0;
        switch (this.CurrentState) {
            case "Ken1":
                target.SetVelocity(-5, 0, 2);
                if (!target.isBlocking()) {
                    target.ChangeState("Airboned");
                } else {
                    target.ChangeState("Blocking");
                }
                break;

            case "Ken2":
                target.SetVelocity(-5, 0, 2);
                if (!target.isBlocking()) {
                    target.ChangeState("Airboned");
                } else {
                    target.ChangeState("Blocking");
                }
                break;

            default:
                break;
        }
    }
}