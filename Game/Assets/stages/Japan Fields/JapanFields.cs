using Stage_Space;
using Animation_Space;


public class JapanFields : Stage {
    public JapanFields()
        : base("Japan Fields", 540, 896, 511, "Assets/stages/Japan Fields/sprites", "Assets/stages/Japan Fields/sound", "Assets/stages/Japan Fields/thumb.png")
    {
        this.AmbientLight = new SFML.Graphics.Color(255, 255, 230, 255);
    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0};
        List<string> frames = defaultFrames.Select(i => i.ToString()).ToList();

        var animations = new Dictionary<string, State> {
            { "Default", new State(frames, "Default", 1)},
        };

        this.states = animations;
    }
}