using Animation_Space;

public class State {
    public Animation animation;
    public string state;
    public string post_state;
    public string priority;
    public string[] hitstop = {"Default", "Default", "Default"}; // None, Default, Light, Medium, Heavy

    public bool doTrace;
    public bool loopAnimation;
    public bool changeOnLastframe;
    public bool changeOnGround;

    public State(List<FrameData> frames, string post_state, int framerate = 30, int screenFramerate = 60, bool loop = true, bool changeOnLastframe = true, bool changeOnGround = false, bool doTrace = false, string[] hitstop = null) {
        this.animation = new Animation(frames, framerate, screenFramerate, loop);
        this.post_state = post_state;
        this.changeOnLastframe = changeOnLastframe;
        this.changeOnGround = changeOnGround;
        this.doTrace = doTrace;
        this.hitstop = hitstop == null ? new string[] {"Default", "Default", "Default"} : hitstop;
    }

    public State(List<string> frames, string post_state, int framerate = 30, int screenFramerate = 60, bool loop = true, bool changeOnLastframe = true, bool changeOnGround = false, bool doTrace = false, string[] hitstop = null) {
        this.animation = new Animation(frames, framerate, screenFramerate);
        this.post_state = post_state;
        this.changeOnLastframe = changeOnLastframe;
        this.changeOnGround = changeOnGround;
        this.doTrace = doTrace;
        this.hitstop = hitstop == null ? new string[] {"Default", "Default", "Default"} : hitstop;
    }
}
