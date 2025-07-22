using Animation_Space;

public class State {
    public Animation animation;
    public string state;
    public string post_state;
    public int priority; // Light: 0, Medium: 1, Heavy: 2, Special: 3, Super: 4, Throw: 5, Parry: 6, Particles: 7
    public string hitstop; // None, Light, Medium, Heavy

    // Combat logic
    public bool not_acting;
    public bool air;
    public bool low;
    public bool on_hit;
    public bool on_block;
    public bool is_parry;
    public bool can_be_parried;
    public bool can_be_hit;

    // Anim logic
    public bool doTrace;
    public bool doGlow;
    public bool change_on_end;
    public bool change_on_ground;

    public State(List<FrameData> frames, string post_state, int framerate = 30, int priority = -1, bool loop = true, bool change_on_end = true, bool change_on_ground = false, bool canBeParried = true, bool doTrace = false, bool doGlow = false, string hitstop = "Light", bool not_acting = false, bool air = false, bool low = false, bool on_hit = false, bool on_block = false, bool is_parry = false, bool can_be_hit = true) {
        this.animation = new Animation(frames, framerate, loop);
        this.post_state = post_state;
        this.priority = priority;
        this.change_on_end = change_on_end;
        this.change_on_ground = change_on_ground;
        this.doTrace = doTrace;
        this.doGlow = doGlow;
        this.hitstop = hitstop;
        this.can_be_parried = canBeParried;
        this.can_be_hit = can_be_hit;
        this.not_acting = not_acting;
        this.air = air;
        this.low = low;
        this.on_hit = on_hit;
        this.on_block = on_block;
        this.is_parry = is_parry;
    }

    public State(List<string> frames, string post_state, int framerate = 30, int priority = -1, bool loop = true, bool change_on_end = true, bool change_on_ground = false, bool canBeParried = true, bool doTrace = false, bool doGlow = false, string hitstop = "Light", bool not_acting = false, bool air = false, bool low = false, bool on_hit = false, bool on_block = false, bool is_parry = false, bool can_be_hit = true) {
        this.animation = new Animation(frames, framerate);
        this.post_state = post_state;
        this.priority = priority;
        this.change_on_end = change_on_end;
        this.change_on_ground = change_on_ground;
        this.doTrace = doTrace;
        this.doGlow = doGlow;
        this.hitstop = hitstop;
        this.can_be_parried = canBeParried;
        this.can_be_hit = can_be_hit;
        this.not_acting = not_acting;
        this.air = air;
        this.low = low;
        this.on_hit = on_hit;
        this.on_block = on_block;
        this.is_parry = is_parry;
    }
}
