using Character_Space;
using Animation_Space;
using Input_Space;

public class Ken : Character {
    public Ken(string initialState, int startX, int startY)
        : base("Ken", initialState, startX, startY, "D:/GABRIEL/Repositórios/Fighting Game CS/Game/Assets/chars/Ken")
    {
        this.LifePoints = 1000;
        this.StunPoints = 50;
    }
    
    public override void Load() {
        // Hurtboxes
        var hitbox1 = new GenericBox(0, 0, 10, 10, 1);
        var hurtbox1 = new GenericBox(0, 0, 10, 10, 2);

        // Animations
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

        var dashForwardFrames = new List<FrameData> {
            new FrameData(14768, 30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14769, 30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14770, 30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14771, 30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14772, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14773, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var dashBackwardFrames = new List<FrameData> {
            new FrameData(14774, -30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14775, -30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14776, -30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14777, -30, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14778, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14779, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingInFrames = new List<FrameData> {
            new FrameData(14696, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14697, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14698, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14699, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingFrames = new List<FrameData> {
            new FrameData(14699, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var crouchingOutFrames = new List<FrameData> {
            new FrameData(14700, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14701, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14702, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var heavyHadukenFrames = new List<FrameData> {
            new FrameData(15328, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15329, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15330, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15331, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15332, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15333, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15334, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15335, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15336, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15337, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15338, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15339, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var lightHadukenFrames = new List<FrameData> {
            new FrameData(15329, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15330, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15331, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15333, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15334, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15335, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15336, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15337, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15338, 0, 0, new List<GenericBox> { hurtbox1 }),
        };

        var heavyShoryFrames = new List<FrameData> {
            new FrameData(15342, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15343, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15344, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15345, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15346, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15347, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15348, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15349, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15350, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15351, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15352, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15353, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15354, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15355, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var lightShoryFrames = new List<FrameData> {
            new FrameData(15345, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15346, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15347, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15348, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15349, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15350, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15351, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15352, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15353, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15354, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15355, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var heavyTatsoFrames = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15357, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15358, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15359, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15457, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15458, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15459, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15460, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15461, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15457, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15458, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15459, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15460, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15461, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15366, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15367, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15368, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15369, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15370, 0, 0, new List<GenericBox> { hurtbox1 })
        };

        var lightTatsoFrames = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15357, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15358, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15359, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15457, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15458, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15459, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15460, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15461, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15366, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15367, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15368, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15369, 0, 0, new List<GenericBox> { hurtbox1 })
        };
        
        // States
        var animations = new Dictionary<string, Animation> {
            // Normals
            { "Idle", new Animation(idleFrames, "Idle")},
            { "AAttack", new Animation(LPFrames, "Idle")},
            { "BAttack", new Animation(LKFrames, "Idle")},
            { "CAttack", new Animation(MPFrames, "Idle")},
            { "DAttack", new Animation(MKFrames, "Idle")},
            // Movment
            { "WalkingForward", new Animation(walkingForwardFrames, "Idle")},
            { "WalkingBackward", new Animation(walkingBackwardFrames, "Idle")},
            { "DashForward", new Animation(dashForwardFrames, "Idle")},
            { "DashBackward", new Animation(dashBackwardFrames, "Idle")},
            { "CrouchingIn", new Animation(crouchingInFrames, "Crouching")},
            { "Crouching", new Animation(crouchingFrames, "CrouchingOut")},
            { "CrouchingOut", new Animation(crouchingOutFrames, "Idle")},
            // Specials
            { "LightShory", new Animation(lightShoryFrames, "Idle")},
            { "HeavyShory", new Animation(heavyShoryFrames, "Idle")},
            { "LightHaduken", new Animation(heavyHadukenFrames, "Idle")},
            { "HeavyHaduken", new Animation(heavyHadukenFrames, "Idle")},
            { "LightTatso", new Animation(lightTatsoFrames, "Idle")},
            { "HeavyTatso", new Animation(heavyTatsoFrames, "Idle")}
        };

        this.animations = animations;
        this.LoadSpriteImages();
    }

    public override void DoBehavior() {
        if ((this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward") & !InputManager.Instance.Key_hold(8) & !InputManager.Instance.Key_hold(9)) {
            this.ChangeState("Idle");
        }

        // Specials
        int[] light_shory_string = {9, 7, 9, 0};
        int[] heavy_shory_string = {9, 7, 9, 2};
        int[] light_haduken_string = {7, 9, 0};
        int[] heavy_haduken_string = {7, 9, 2};
        int[] light_tatso_string = {7, 8, 1};
        int[] heavy_tatso_string = {7, 8, 3};

        if (InputManager.Instance.CheckString(light_shory_string, 5) && this.canNormalAtack) {
            this.ChangeState("LightShory");
        } else if (InputManager.Instance.CheckString(heavy_shory_string, 5) && this.canNormalAtack) {
            this.ChangeState("HeavyShory");
        }

        if (InputManager.Instance.CheckString(light_haduken_string, 5) && this.canNormalAtack) {
            this.ChangeState("LightHaduken");
        } else if (InputManager.Instance.CheckString(heavy_haduken_string, 5) && this.canNormalAtack) {
            this.ChangeState("HeavyHaduken");
        }

        if (InputManager.Instance.CheckString(light_tatso_string, 5) && this.canNormalAtack) {
            this.ChangeState("LightTatso");
        } else if (InputManager.Instance.CheckString(heavy_tatso_string, 5) && this.canNormalAtack) {
            this.ChangeState("HeavyTatso");
        }

        // Cancels
        if (InputManager.Instance.Key_down(3) && this.CurrentState == "BAttack" && this.CurrentAnimation.currentFrameIndex >= 3) {
            this.ChangeState("DAttack");
        }

        // Normais
        if (InputManager.Instance.Key_down(0) && this.canNormalAtack) {
            this.ChangeState("AAttack");
        } else if (InputManager.Instance.Key_down(1) && this.canNormalAtack) {
            this.ChangeState("BAttack");
        } else if (InputManager.Instance.Key_down(2) && this.canNormalAtack) {
            this.ChangeState("CAttack");
        } else if (InputManager.Instance.Key_down(3) && this.canNormalAtack ) {
            this.ChangeState("DAttack");
        }

        // Movimento
        if (InputManager.Instance.Key_hold(7) && !InputManager.Instance.Key_hold(6) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("CrouchingIn");
        }
        if (this.CurrentState == "CrouchingOut" && InputManager.Instance.Key_hold(7) && !InputManager.Instance.Key_hold(6)) {
            this.ChangeState("Crouching");
        }

        int[] dash_forward_string = {9, 9};
        int[] dash_backward_string = {8, 8};
        if (InputManager.Instance.CheckString(dash_forward_string, 5) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("DashForward");
        } 
        else if (InputManager.Instance.CheckString(dash_backward_string, 5) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("DashBackward");
        }

        if (InputManager.Instance.Key_hold(8) && !InputManager.Instance.Key_hold(9) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("WalkingBackward");
        } else if (InputManager.Instance.Key_hold(9) && !InputManager.Instance.Key_hold(8) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingBackward" || this.CurrentState == "WalkingForward")) {
            this.ChangeState("WalkingForward");
        }
    }
}