using Character_Space;
using Animation_Space;
using SFML.System;
using Input_Space;
using System.Runtime.InteropServices;

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
            new FrameData(01, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(02, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(03, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(04, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(05, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(06, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(07, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(08, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(09, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(010, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(011, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(012, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(013, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(014, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(015, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(016, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(017, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(018, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(019, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(020, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(021, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
        };

        var KenFireballFinal = new List<FrameData> {
            new FrameData(11, 0, 0, new List<GenericBox> {}),
            new FrameData(12, 0, 0, new List<GenericBox> {}),
            new FrameData(13, 0, 0, new List<GenericBox> {}),
            new FrameData(14, 0, 0, new List<GenericBox> {}),
            new FrameData(15, 0, 0, new List<GenericBox> {}),
            new FrameData(16, 0, 0, new List<GenericBox> {}),
        };

        // States
        var animations = new Dictionary<string, Animation> {
            {"Ken1", new Animation(KenFireballFrames, "Ken1", 20)},
            {"Ken2", new Animation(KenFireballFrames, "Ken2", 30)},
            {"KenExit", new Animation(KenFireballFinal, "", 30, doChangeState: false)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehavior() {        
        switch (this.CurrentState) {
            case "Ken1":
                this.Position.X += 7 * this.facing;
                 break;

            case "Ken2":
                this.Position.X += 10 * this.facing;
                 break;

            case "KenExit":
                if (this.CurrentAnimation.onLastFrame) this.LifePoints.X = -1;
                break;

            default:
                break;
        }
    }

    public override void ImposeBehavior(Character target) {
        switch (this.CurrentState) {
            case "Ken1":
                target.SetVelocity(-5, 0, 2);
                this.ChangeState("KenExit");
                if (!target.isBlocking()) {
                    target.ChangeState("Airboned");
                } else {
                    target.ChangeState("Blocking");
                }
                break;

            case "Ken2":
                target.SetVelocity(-5, 0, 2);
                this.ChangeState("KenExit");
                if (!target.isBlocking()) {
                    target.ChangeState("Airboned");
                } else {
                    target.ChangeState("Blocking");
                }
                break;

            default:
                this.LifePoints.X = -1;
                break;
        }
    }
}