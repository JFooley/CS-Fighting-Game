using Stage_Space;
using Animation_Space;


public class BurningDojo : Stage {
    public BurningDojo()
        : base("Burning Dojo", 369, 800, 336, "Assets/stages/Burning Dojo/sprites", "Assets/stages/Burning Dojo/sound", "Assets/stages/Burning Dojo/thumb.png")
    {
        this.AmbientLight = new SFML.Graphics.Color(255, 230, 210, 255);
    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0, 1, 2, 3, 4, 5, 6, 7};
        List<string> frames = defaultFrames.Select(i => i.ToString()).ToList();

        var animations = new Dictionary<string, State> {
            { "Default", new State(frames, "Default", 15)},
        };

        this.states = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}