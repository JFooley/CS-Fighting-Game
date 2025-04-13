using Animation_Space;

public class State {
    public Animation animation;
    public string state;
    public string post_state;
    public int priority; // Light: 0, Medium: 1, Heavy: 2, Special: 3, Super: 4, Throw: 5, Parry: 6, Particles: 7
    public string hitstop; // None, Light, Medium, Heavy

    // Combat logic
    public bool canBeParried;

    // Anim logic
    public bool doTrace;
    public bool doGlow;
    public bool loopAnimation;
    public bool change_on_end;
    public bool change_on_ground;

    public State(List<FrameData> frames, string post_state, int framerate = 30, int priority = -1, bool loop = true, bool changeOnLastframe = true, bool changeOnGround = false, bool canBeParried = true, bool doTrace = false, bool doGlow = false, string hitstop = "Light") {
        this.animation = new Animation(frames, framerate, loop);
        this.post_state = post_state;
        this.priority = priority;
        this.change_on_end = changeOnLastframe;
        this.change_on_ground = changeOnGround;
        this.doTrace = doTrace;
        this.doGlow = doGlow;
        this.hitstop = hitstop;
        this.canBeParried = canBeParried;
    }

    public State(List<string> frames, string post_state, int framerate = 30, int priority = -1, bool changeOnLastframe = true, bool changeOnGround = false, bool doTrace = false, bool doGlow = false, bool canBeParried = true, string hitstop = "Light" ) {
        this.animation = new Animation(frames, framerate);
        this.post_state = post_state;
        this.priority = priority;
        this.change_on_end = changeOnLastframe;
        this.change_on_ground = changeOnGround;
        this.doTrace = doTrace;
        this.doGlow = doGlow;
        this.hitstop = hitstop;
        this.canBeParried = canBeParried;
    }
}
