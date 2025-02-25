using Character_Space;
using Animation_Space;
using SFML.System;
using Stage_Space;

public class Fireball : Character {
    public Fireball(string initialState, float startX, float startY, int team, int facing, Stage stage)
        : base("Fireball", initialState, startX, startY, "Assets/particles/sprites/Fireball", "Assets/particles/sounds/Fireball", stage, 1) {
            this.team = team;
            this.facing = facing;
            this.LifePoints = new Vector2i(1, 1);
        }
    public override void Load() {
        // Animations
        var kenFB0 = new GenericBox(0, 139, 115, 163, 143);
        var kenFB1 = new GenericBox(1, 139, 115, 163, 143);
        
        var KenFireballFrames = new List<FrameData> { 
            new FrameData(21, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(22, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(23, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(24, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(25, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(26, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(27, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(28, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(29, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(210, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(211, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(212, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(213, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(214, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(215, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(216, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(217, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(218, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(219, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(220, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
            new FrameData(221, 0, 0, new List<GenericBox> {kenFB1, kenFB0}),
        };

        var KenFireballFinal = new List<FrameData> {
            new FrameData(12, 0, 0, new List<GenericBox> {}),
            new FrameData(13, 0, 0, new List<GenericBox> {}),
            new FrameData(14, 0, 0, new List<GenericBox> {}),
            new FrameData(15, 0, 0, new List<GenericBox> {}),
            new FrameData(16, 0, 0, new List<GenericBox> {}),
            new FrameData(17, 0, 0, new List<GenericBox> {}),
        };

        // States
        var animations = new Dictionary<string, Animation> {
            {"Ken1", new Animation(KenFireballFrames, "Ken1", 20)},
            {"Ken2", new Animation(KenFireballFrames, "Ken2", 30)},
            {"KenExit", new Animation(KenFireballFinal, "Remove", 30)},
            {"Remove", new Animation(KenFireballFinal, "Remove", 60)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehave() {        
        base.DoBehave();
        switch (this.CurrentState) {
            case "Ken1":
                this.SetVelocity(X: 4);
                if (this.LifePoints.X == -1) {
                    this.ChangeState("KenExit");
                    this.SetVelocity(raw_set: true);
                    break;
                }
                break;

            case "Ken2":
                this.SetVelocity(X: 5);
                if (this.LifePoints.X == -1) {
                    this.ChangeState("KenExit");
                    this.SetVelocity(raw_set: true);
                    break;
                }
                break;

            case "Remove":
                this.remove = true;
                break;
                
            default:
                break;
        }
    }

    public override int ImposeBehavior(Character target) {
        int hit = -1;

        if (target.name == "Fireball") {
            target.LifePoints.X = -1;
            return hit;
        };

        switch (this.CurrentState) {
            case "Ken1":
                Character.Pushback(target: target, self: this, "Medium", force_push: true);
                this.ChangeState("KenExit");
                this.SetVelocity(raw_set: true);

                if (target.isBlocking()) {
                    hit = 0;
                    Character.Damage(target: target, self: this, 10, 0);
                    target.BlockStun(this, 20, force: true);

                } else {
                    hit = 1;
                    Character.Damage(target: target, self: this, 63, 48);
                    target.HitStun(this, 30, force: true);
                }
                break;

            case "Ken2":
                Character.Pushback(target: target, self: this, "Heavy", force_push: true);
                this.ChangeState("KenExit");
                this.SetVelocity(raw_set: true);

                if (target.isBlocking()) {
                    hit = 0;
                    Character.Damage(target: target, self: this, 10, 0);
                    target.BlockStun(this, 20, force: true);

                } else {
                    hit = 1;
                    Character.Damage(target: target, self: this, 125, 78);
                    target.HitStun(this, 30, force: true);
                }
                break;

            default:
                this.remove = true;
                break;
        }
        return hit;
    }


}