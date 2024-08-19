using SFML.System;

namespace Animation_Space {

public class Animation {
    public List<FrameData> Frames { get; private set; }
    public List<int> SimpleFrames { get; private set; }
    public int currentFrameIndex;
    public bool onLastFrame;
    public string post_state; 
    public int animSize;
    public int realAnimSize => animSize * (60 / this.framerate);

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

    public Animation(List<int> SimpleFrames, string post_state, int framerate = 24, int screenFramerate = 60) {
        this.SimpleFrames = SimpleFrames;
        this.currentFrameIndex = 0;
        this.doRun = true;
        this.animSize = SimpleFrames.Count() - 1;
        this.post_state = post_state;
        this.doChangeState = true;
        this.framerate = framerate;
        this.screenFramerate = screenFramerate;
        this.frameCounter = 0;
    }

    public FrameData GetCurrentFrame()
    {
        return this.Frames[currentFrameIndex];
    }

    public int GetCurrentSimpleFrame()
    {
        return this.SimpleFrames[currentFrameIndex];
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

    public Vector2i pA;
    public Vector2i pB;

    public int type;

    public GenericBox(int type, int x1, int y1, int x2, int y2) {   
        this.type = type;
        this.pA.X = x1;
        this.pB.X = x2;
        this.pA.Y = y1;
        this.pB.Y = y2;
    }

    public bool Intersects(GenericBox box, Vector2i position_A, Vector2i position_B) {
        return this.pA.X + position_A.X < box.pB.X + position_B.X && // Verifica se o lado direito da box atual está à esquerda do lado direito da outra box
            this.pB.X + position_A.X > box.pA.X + position_B.X && // Verifica se o lado esquerdo da box atual está à direita do lado esquerdo da outra box
            this.pA.Y + position_A.Y < box.pB.Y + position_B.Y && // Verifica se o lado inferior da box atual está acima do lado inferior da outra box
            this.pB.Y + position_A.Y > box.pA.Y + position_B.Y;   // Verifica se o lado superior da box atual está abaixo do lado superior da outra box
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