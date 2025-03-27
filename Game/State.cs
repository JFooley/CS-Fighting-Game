using Animation_Space;

public class State {
    public Animation animation;
    public string state;
    public string post_state;
    public int priority; // Light: 0, Medium: 1, Heavy: 2, Special: 3, Super: 4, Throw: 5, Parry: 6
    public string[] hitstop = {"Default", "Default", "Default"}; // None, Default, Light, Medium, Heavy

    public bool doTrace;
    public bool loopAnimation;
    public bool changeOnLastframe;
    public bool changeOnGround;

    public State(List<FrameData> frames, string post_state, int framerate = 30, int priority = 0, bool loop = true, bool changeOnLastframe = true, bool changeOnGround = false, bool doTrace = false, string[] hitstop = null) {
        this.animation = new Animation(frames, framerate, Config.Framerate, loop);
        this.post_state = post_state;
        this.priority = priority;
        this.changeOnLastframe = changeOnLastframe;
        this.changeOnGround = changeOnGround;
        this.doTrace = doTrace;
        this.hitstop = hitstop == null ? new string[] {"Default", "Default", "Default"} : hitstop;
    }

    public State(List<string> frames, string post_state, int framerate = 30, int priority = 0, bool changeOnLastframe = true, bool changeOnGround = false, bool doTrace = false, string[] hitstop = null) {
        this.animation = new Animation(frames, framerate, Config.Framerate);
        this.post_state = post_state;
        this.priority = priority;
        this.changeOnLastframe = changeOnLastframe;
        this.changeOnGround = changeOnGround;
        this.doTrace = doTrace;
        this.hitstop = hitstop == null ? new string[] {"Default", "Default", "Default"} : hitstop;
    }
}
