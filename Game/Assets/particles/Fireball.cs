using Character_Space;
using Animation_Space;
using SFML.System;
using Stage_Space;

public class Fireball : Character {
    public Fireball(string initialState, int life_points, float startX, float startY, int team, int facing, Stage stage)
        : base("Fireball", initialState, startX, startY, "Assets/particles/sprites/Fireball", "Assets/particles/sounds/Fireball", stage, 1) {
            this.playerIndex = team;
            this.facing = facing;
            this.LifePoints = new Vector2i(life_points, life_points);
            this.shadow_size = 0;
        }
    public override void Load() {
        // Animations
        var kenFB0 = new GenericBox(0, 139, 115, 163, 143);
        var kenFB1 = new GenericBox(1, 139, 115, 163, 143);
        
        var KenFireballFrames = new List<FrameData> { 
            new FrameData(21, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(22, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(23, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(24, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(25, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(26, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(27, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(28, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(29, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(210, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(211, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(212, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(213, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(214, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(215, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(216, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(217, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(218, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(219, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(220, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
            new FrameData(221, 0, 0, new List<GenericBox> {kenFB1, kenFB0}, hasHit: false),
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
        var animations = new Dictionary<string, State> {
            {"Ken1", new State(KenFireballFrames, "Ken1", 20, 7)},
            {"Ken2", new State(KenFireballFrames, "Ken2", 20, 7)},
            {"Ken3", new State(KenFireballFrames, "Ken3", 30, 7)},
            {"KenExit", new State(KenFireballFinal, "Remove", 30, 7)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehave() {
        base.DoBehave();
        if (this.State.post_state == "Remove" && this.CurrentAnimation.ended) {
            this.remove = true;
            this.CurrentAnimation.Reset();
        }

        if (Math.Abs(Program.camera.X - this.VisualPosition.X) > Config.RenderWidth) this.remove = true;  
        
        switch (this.CurrentState) {
            case "Ken1":
                this.SetVelocity(X: 4);
                if (this.LifePoints.X <= 0) {
                    this.ChangeState("KenExit");
                    this.SetVelocity(raw_set: true);
                    break;
                }
                break;

            case "Ken2":
                this.SetVelocity(X: 5);
                if (this.LifePoints.X <= 0) {
                    this.ChangeState("KenExit");
                    this.SetVelocity(raw_set: true);
                    break;
                }
                break;
            
            case "Ken3":
                this.SetVelocity(X: 6);
                if (this.LifePoints.X <= 0) {
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

    public override int ImposeBehavior(Character target, bool parried = false) {
        int hit = -1;

        if (this.LifePoints.X <= 0) return hit;

        if (parried && this.State.canBeParried) {
            this.LifePoints.X -= 1;
            this.hasHit = true;
            return Character.PARRY;
        }

        if (target.name == "Fireball") {
            target.LifePoints.X -= 1;
            this.LifePoints.X -= 1;
            this.hasHit = true;
            return hit;
        };

        switch (this.CurrentState) {
            case "Ken1":
                this.LifePoints.X -= 1;
                this.SetVelocity(raw_set: true);

                if (target.isBlocking()) {
                    hit = 0;
                    Character.Damage(target: target, self: this, 10, 0);
                    target.BlockStun(this, 20, force: true);

                } else {
                    hit = 1;
                    Character.Damage(target: target, self: this, 63, 48);
                    target.Stun(this, 30, force: true);
                }

                if (this.playerIndex == 0) Character.GetSuperPoints(target: target, self: stage.character_A, hit);
                else Character.GetSuperPoints(target: target, self: stage.character_B, hit);
                break;

            case "Ken2":
                this.LifePoints.X -= 1;
                this.SetVelocity(raw_set: true);

                if (target.isBlocking()) {
                    hit = 0;
                    Character.Damage(target: target, self: this, 10, 0);
                    target.BlockStun(this, 20, force: true);

                } else {
                    hit = 1;
                    Character.Damage(target: target, self: this, 125, 78);
                    target.Stun(this, 30, force: true);
                }

                if (this.playerIndex == 0) Character.GetSuperPoints(target: target, self: stage.character_A, hit);
                else Character.GetSuperPoints(target: target, self: stage.character_B, hit);
                break;
            
            case "Ken3":
                this.LifePoints.X -= 1;
                this.SetVelocity(raw_set: true);

                if (target.isBlocking()) {
                    hit = 0;
                    Character.Damage(target: target, self: this, 10, 0);
                    target.BlockStun(this, 20, force: true);

                } else {
                    hit = 1;
                    Character.Damage(target: target, self: this, 125, 78);
                    target.Stun(this, 30, force: true);
                }

                if (this.playerIndex == 0) Character.GetSuperPoints(target: target, self: stage.character_A, hit);
                else Character.GetSuperPoints(target: target, self: stage.character_B, hit);
                break;

            default:
                this.remove = true;
                break;
        }
        return hit;
    }


}