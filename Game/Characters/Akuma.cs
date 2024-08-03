using Character_Space;
using Animation_Space;
using Input_Space;

public class Akuma : Character {
    public Akuma(string initialState, int startX, int startY)
        : base("Akuma", initialState, startX, startY, "D:/GABRIEL/Repositórios/Fighting Game CS/Game/Assets/chars/Akuma", "D:/GABRIEL/Repositórios/Fighting Game CS/Game/Assets/sounds/Ken")
    {
        this.LifePoints = 900;
        this.StunPoints = 40;
    }
    
    public override void Load() {
        var hitbox1 = new GenericBox(0, 0, 10, 10, 1);
        var hurtbox1 = new GenericBox(0, 0, 10, 10, 2);

        var idleFrames = new List<FrameData> {
            new FrameData(18273, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18274, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18275, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18276, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18277, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18278, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18279, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18280, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18281, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18282, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var MPFrames = new List<FrameData> {
            new FrameData(18629, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18630, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18631, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18632, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18633, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18634, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18635, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18636, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };
        
        var HPFrames = new List<FrameData> {
            new FrameData(18637, 10, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18638, 10, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18639, 10, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18640, 10, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18641, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18642, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18643, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18644, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18645, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };

        var MKFrames = new List<FrameData> {
            new FrameData(18704, 3, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18705, 3, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18706, 3, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18707, 3, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18708, 3, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18709, 3, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18710, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18711, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18712, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18713, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18714, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18715, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18716, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };

        var LKFrames = new List<FrameData> {
            new FrameData(18726, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18728, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18729, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18730, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18731, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18732, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18733, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18758, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(18759, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
        };

        var walkingForwardFrames = new List<FrameData> {
            new FrameData(18298, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18288, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18289, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18290, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18291, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18292, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18293, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18294, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18295, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18296, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18297, 15, 0, new List<GenericBox> { hurtbox1 }),
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(18299, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18300, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18301, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18302, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18303, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18304, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18305, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18306, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18307, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18308, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18309, -15, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingInFrames = new List<FrameData> {
            new FrameData(18312, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18313, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18314, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18315, 0, 0, new List<GenericBox> { hurtbox1 }),
        };

        var crouchingFrames = new List<FrameData> {
            new FrameData(18316, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingOutFrames = new List<FrameData> {
            new FrameData(18317, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18318, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18319, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var idleAnimation = new Animation(idleFrames, "Idle");
        var AAnimation = new Animation(MPFrames, "Idle");
        var BAnimation = new Animation(LKFrames, "Idle");
        var CAnimation = new Animation(HPFrames, "Idle");
        var DAnimation = new Animation(MKFrames, "Idle");
        var walkingFAnimation = new Animation(walkingForwardFrames, "Idle");
        var walkingBAnimation = new Animation(walkingBackwardFrames, "Idle");
        var crouchingInAnimation = new Animation(crouchingInFrames, "Crouching");
        var crouchingAnimation = new Animation(crouchingFrames, "CrouchingOut");
        var crouchingOutAnimation = new Animation(crouchingOutFrames, "Idle");

        var animations = new Dictionary<string, Animation>
        {
            { "Idle", idleAnimation},
            { "AAttack", AAnimation},
            { "BAttack", BAnimation},
            { "CAttack", CAnimation},
            { "DAttack", DAnimation},
            { "WalkingForward", walkingFAnimation},
            { "WalkingBackward", walkingBAnimation},
            { "CrouchingIn", crouchingInAnimation},
            { "Crouching", crouchingAnimation},
            { "CrouchingOut", crouchingOutAnimation}
        };

        this.animations = animations;

        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehavior() {
        if ((this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward") & !InputManager.Instance.Key_hold(8) & !InputManager.Instance.Key_hold(9)) {
            this.ChangeState("Idle");
        }

        if (InputManager.Instance.Key_down(0) && this.canNormalAtack) {
            this.ChangeState("AAttack");
        } else if (InputManager.Instance.Key_down(1) && this.canNormalAtack) {
            this.ChangeState("BAttack");
        } else if (InputManager.Instance.Key_down(2) && this.canNormalAtack) {
            this.ChangeState("CAttack");
        } else if (InputManager.Instance.Key_down(3) && this.canNormalAtack ) {
            this.ChangeState("DAttack");
        }

        // target combo
        if (InputManager.Instance.Key_down(2) && this.CurrentState == "AAttack" && this.CurrentAnimation.currentFrameIndex >= 3) {
            this.ChangeState("CAttack");
        }

        if (InputManager.Instance.Key_hold(7) && !InputManager.Instance.Key_hold(6) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("CrouchingIn");
        }
        if (this.CurrentState == "CrouchingOut" && InputManager.Instance.Key_hold(7) && !InputManager.Instance.Key_hold(6)) {
            this.ChangeState("Crouching");
        }

        if (InputManager.Instance.Key_hold(8) && !InputManager.Instance.Key_hold(9) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward")) {
            this.ChangeState("WalkingBackward");
        } else if (InputManager.Instance.Key_hold(9) && !InputManager.Instance.Key_hold(8) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("WalkingForward");
        }
    }
}