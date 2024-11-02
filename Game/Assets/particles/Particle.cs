using Character_Space;
using Animation_Space;
using SFML.System;
using SFML.Graphics;

public class Particle : Character {
    public Particle(string initialState, float startX, float startY, int facing)
        : base("Particle", initialState, startX, startY, "Assets/particles/sprites/Particle", "Assets/particles/sounds/Particle", null) {
            this.facing = facing;
        }

    public override void Load() {
        // Animations
        var SALighting = new List<FrameData> { 
            new FrameData(11, 0, 0, new List<GenericBox> {}),
            new FrameData(12, 0, 0, new List<GenericBox> {}),
            new FrameData(13, 0, 0, new List<GenericBox> {}),
            new FrameData(14, 0, 0, new List<GenericBox> {}),
            new FrameData(15, 0, 0, new List<GenericBox> {}),
            new FrameData(16, 0, 0, new List<GenericBox> {}),
            new FrameData(17, 0, 0, new List<GenericBox> {}),
            new FrameData(18, 0, 0, new List<GenericBox> {}),
            new FrameData(19, 0, 0, new List<GenericBox> {}),
            new FrameData(110, 0, 0, new List<GenericBox> {}),
            new FrameData(111, 0, 0, new List<GenericBox> {}),
            new FrameData(112, 0, 0, new List<GenericBox> {}),
            new FrameData(113, 0, 0, new List<GenericBox> {}),
            new FrameData(114, 0, 0, new List<GenericBox> {}),
            new FrameData(115, 0, 0, new List<GenericBox> {}),
            new FrameData(116, 0, 0, new List<GenericBox> {}),
            new FrameData(117, 0, 0, new List<GenericBox> {}),
            new FrameData(118, 0, 0, new List<GenericBox> {}),
            new FrameData(119, 0, 0, new List<GenericBox> {}),
            new FrameData(120, 0, 0, new List<GenericBox> {}),
            new FrameData(121, 0, 0, new List<GenericBox> {}),
            new FrameData(122, 0, 0, new List<GenericBox> {}),
            new FrameData(123, 0, 0, new List<GenericBox> {}),
            new FrameData(124, 0, 0, new List<GenericBox> {}),
            new FrameData(125, 0, 0, new List<GenericBox> {}),
            new FrameData(126, 0, 0, new List<GenericBox> {}),
            new FrameData(127, 0, 0, new List<GenericBox> {}),
            new FrameData(128, 0, 0, new List<GenericBox> {}),
            new FrameData(129, 0, 0, new List<GenericBox> {}),
            new FrameData(130, 0, 0, new List<GenericBox> {}),
            new FrameData(131, 0, 0, new List<GenericBox> {}),
            new FrameData(132, 0, 0, new List<GenericBox> {}),
            new FrameData(133, 0, 0, new List<GenericBox> {}),
            new FrameData(134, 0, 0, new List<GenericBox> {}),
            new FrameData(135, 0, 0, new List<GenericBox> {}),
            new FrameData(136, 0, 0, new List<GenericBox> {}),
            new FrameData(137, 0, 0, new List<GenericBox> {}),
            new FrameData(138, 0, 0, new List<GenericBox> {}),
            new FrameData(139, 0, 0, new List<GenericBox> {}),
            new FrameData(140, 0, 0, new List<GenericBox> {}),
            new FrameData(141, 0, 0, new List<GenericBox> {}),
            new FrameData(142, 0, 0, new List<GenericBox> {}),
            new FrameData(143, 0, 0, new List<GenericBox> {}),
            new FrameData(144, 0, 0, new List<GenericBox> {}),
            new FrameData(145, 0, 0, new List<GenericBox> {}),
            new FrameData(146, 0, 0, new List<GenericBox> {}),
            new FrameData(147, 0, 0, new List<GenericBox> {}),
            new FrameData(148, 0, 0, new List<GenericBox> {}),
            new FrameData(149, 0, 0, new List<GenericBox> {}),
            new FrameData(150, 0, 0, new List<GenericBox> {}),
            new FrameData(151, 0, 0, new List<GenericBox> {}),
            new FrameData(152, 0, 0, new List<GenericBox> {}),
            new FrameData(153, 0, 0, new List<GenericBox> {}),
            new FrameData(154, 0, 0, new List<GenericBox> {}),
        };

        var SABlink = new List<FrameData> {
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
            new FrameData(211, 0, 0, new List<GenericBox> {}),
            new FrameData(212, 0, 0, new List<GenericBox> {}),
            new FrameData(213, 0, 0, new List<GenericBox> {}),
            new FrameData(214, 0, 0, new List<GenericBox> {}),
            new FrameData(215, 0, 0, new List<GenericBox> {}),
            new FrameData(216, 0, 0, new List<GenericBox> {}),
            new FrameData(217, 0, 0, new List<GenericBox> {}),
            new FrameData(218, 0, 0, new List<GenericBox> {}),
            new FrameData(219, 0, 0, new List<GenericBox> {}),
            new FrameData(220, 0, 0, new List<GenericBox> {}),
            new FrameData(221, 0, 0, new List<GenericBox> {}),
        };

        // States
        var animations = new Dictionary<string, Animation> {
            {"SALighting", new Animation(SALighting, "Default", 60, doChangeState: false)},
            {"SABlink", new Animation(SABlink, "Default", 30, doChangeState: false)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
    public override void DoRender(RenderWindow window, bool drawHitboxes = false) {
        if (!this.render) return;
        
        // Render sprite
        Sprite temp_sprite = this.GetCurrentSpriteImage();
        temp_sprite.Position = new Vector2f(this.Position.X - (temp_sprite.GetLocalBounds().Width / 2 * this.facing), this.Position.Y - temp_sprite.GetLocalBounds().Height / 2);
        temp_sprite.Scale = new Vector2f(this.size_ratio * this.facing, this.size_ratio);
        window.Draw(temp_sprite);

        // Play sounds
        base.PlaySound();
    }
    public override void DoBehave() {        
        if (this.CurrentAnimation.onLastFrame) this.remove = true;
    }
}