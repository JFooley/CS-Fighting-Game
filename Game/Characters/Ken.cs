using Character_Space;
using Animation_Space;
using Input_Space;

public class Ken : Character {
    public Ken(string initialState, int startX, int startY)
        : base("Ken", initialState, startX, startY, "D:/GABRIEL/Repositórios/Testes em C#/Game/Assets/chars/Ken")
    {
        this.LifePoints = 1000;
        this.StunPoints = 50;
    }
    
    public override void Load() {
        var hitbox1 = new GenericBox(0, 0, 10, 10, 1);
        var hurtbox1 = new GenericBox(0, 0, 10, 10, 2);

        var idleFrames = new List<FrameData> {
            new FrameData(14657, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14658, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14659, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14660, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14661, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14662, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14663, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14664, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14665, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14666, 0, 0, new List<GenericBox> { hurtbox1 }),
        };

        var LPFrames = new List<FrameData> {
            new FrameData(15008, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15009, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15010, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15011, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15012, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };
        
        var MPFrames = new List<FrameData> {
            new FrameData(15013, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15014, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15015, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15016, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15017, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15018, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15018, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };

        var MKFrames = new List<FrameData> {
            new FrameData(15104, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15072, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15073, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15074, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15075, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15076, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15077, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15078, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15079, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15080, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15081, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15082, 4, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
        };

        var LKFrames = new List<FrameData> {
            new FrameData(15104, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15105, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15106, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15107, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15108, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15109, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15110, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };

        var walkingForwardFrames = new List<FrameData> {
            new FrameData(14671, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14672, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14673, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14674, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14675, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14676, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14677, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14678, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14679, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14680, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14681, 15, 0, new List<GenericBox> { hurtbox1 })
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(14683, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14684, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14685, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14686, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14687, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14688, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14689, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14690, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14691, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14692, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14693, -15, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingInFrames = new List<FrameData> {
            new FrameData(14696, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14697, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14698, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14699, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingFrames = new List<FrameData> {
            new FrameData(14700, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14701, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14702, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingOutFrames = new List<FrameData> {

        };

        var idleAnimation = new Animation(idleFrames, "Idle");
        var AAnimation = new Animation(LPFrames, "Idle");
        var BAnimation = new Animation(LKFrames, "Idle");
        var CAnimation = new Animation(MPFrames, "Idle");
        var DAnimation = new Animation(MKFrames, "Idle");
        var walkingFAnimation = new Animation(walkingForwardFrames, "Idle");
        var walkingBAnimation = new Animation(walkingBackwardFrames, "Idle");
        var crouchingInAnimation = new Animation(crouchingInFrames, "Crouching");
        var crouchingAnimation = new Animation(crouchingFrames, "Crouching");
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

        // Exemplo de Cancel/Target combo
        if (InputManager.Instance.Key_down(3) && this.CurrentState == "BAttack" && this.CurrentAnimation.currentFrameIndex >= 3) {
            this.ChangeState("DAttack");
        }

        if (InputManager.Instance.Key_hold(8) && !InputManager.Instance.Key_hold(9) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward")) {
            this.ChangeState("WalkingBackward");
        } else if (InputManager.Instance.Key_hold(9) && !InputManager.Instance.Key_hold(8) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("WalkingForward");
        }
    }
}