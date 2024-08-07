namespace Animation_Space {

public class Animation {
    public List<FrameData> Frames { get; private set; }
    public int currentFrameIndex;
    public bool onLastFrame;
    public string post_state; 
    public int animSize;
    public bool doRun;
    public bool doChangeState;
    public int framerate;
    public int screenFramerate;
    private int frameCounter;

    public Animation(List<FrameData> frames, string post_state, int framerate = 24, int screenFramerate = 60, bool doChangeState = true) {
        this.Frames = frames;
        this.currentFrameIndex = 0;
        this.doRun = true;
        this.animSize = Frames.Count() - 1;
        this.post_state = post_state;
        this.doChangeState = doChangeState;
        this.framerate = framerate;
        this.screenFramerate = screenFramerate;
        this.frameCounter = 0;
    }

    public FrameData GetCurrentFrame()
    {
        return Frames[currentFrameIndex];
    }

    public void AdvanceFrame() {
        if (!doRun) return;

        // Calcula o número de frames de tela por frame de animação
        float framesPerAnimFrame = screenFramerate / framerate;

        // Incrementa o contador de frames
        frameCounter++;

        // Avança o frame de animação apenas quando o contador de frames atinge o limite calculado
        if (frameCounter >= framesPerAnimFrame) {
            frameCounter = 0; // Reseta o contador

            if (currentFrameIndex < animSize) {
                currentFrameIndex++;
                onLastFrame = false;
            } else {
                onLastFrame = true;
            }
        }
    }

    public void Reset() {
        currentFrameIndex = 0;
        onLastFrame = false;
        frameCounter = 0;
    }
}

public class GenericBox {
    public const int HITBOX = 0;
    public const int HURTBOX = 1;
    public const int PUSHBOX = 2;

    public int x1, x2, y1, y2;
    public int type;

    public GenericBox(int type, int x1, int y1, int x2, int y2) {   
        this.type = type;
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;
    }
}

public class FrameData {
    public int Sprite_index { get; set; }
    public string Sound_index { get; set; }
    public int DeltaX { get; set; }
    public int DeltaY { get; set; }
    public List<GenericBox> Boxes { get; set; }

    public FrameData(int sprite_index, int deltaX, int deltaY, List<GenericBox> boxes, string Sound_index = "") {
        this.Sprite_index = sprite_index;
        this.DeltaX = deltaX;
        this.DeltaY = deltaY;
        this.Boxes = boxes;
        this.Sound_index = Sound_index;
    }
}

}