using Stage_Space;
using Animation_Space;


public class RindoKanDojo : Stage {
    public RindoKanDojo()
        : base("Rindo-Kan Dojo", 496, 895, 475, "Assets/stages/Rindo-Kan Dojo/sprites", "Assets/stages/Rindo-Kan Dojo/sound", "Assets/stages/Rindo-Kan Dojo/thumb.png")
    {
        this.AmbientLight = new SFML.Graphics.Color(255, 255, 240, 255);
    }

    public override void LoadStage() {
        var defaultFrames = new List<int> {0};
        List<string> frames = defaultFrames.Select(i => i.ToString()).ToList();

        var animations = new Dictionary<string, State> {
            { "Default", new State(frames, "Default", 1)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }
}