using Character_Space;
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
    public int frameCounter;

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

    public bool AdvanceFrame() {
        if (!doRun) return false;

        // Calcula o número de frames de tela por frame de animação
        float framesPerAnimFrame = screenFramerate / framerate;

        // Incrementa o contador de frames
        frameCounter++;

        // Avança o frame de animação apenas quando o contador de frames atinge o limite calculado
        if (frameCounter >= framesPerAnimFrame) {
            frameCounter = 0; // Reseta o contador
            currentFrameIndex++; // Avança um frame

            if (currentFrameIndex < animSize) {
                this.onLastFrame = false;
            } else if (currentFrameIndex == animSize) {
                this.onLastFrame = true;
            } else {
                this.Reset();
            }

            return true; // do frame change
        }

        return false; // no frame change
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

    

    public Vector2f pA;
    public Vector2f pB;

    public int type;

    public GenericBox(int type, int x1, int y1, int x2, int y2) {   
        this.type = type;
        this.pA.X = x1;
        this.pB.X = x2;
        this.pA.Y = y1;
        this.pB.Y = y2;
    }

    public static bool Intersects(GenericBox boxA, GenericBox boxB, Character charA, Character charB) {
        return (boxA.getRealA(charA).X < boxB.getRealB(charB).X) &&
            (boxA.getRealB(charA).X > boxB.getRealA(charB).X) &&
            (boxA.getRealA(charA).Y < boxB.getRealB(charB).Y) &&
            (boxA.getRealB(charA).Y > boxB.getRealA(charB).Y);
    }

    public static void Colide(GenericBox boxA, GenericBox boxB, Character charA, Character charB) {
        // Calcule a sobreposição entre boxA e boxB no eixo X
        var overlapX = Math.Min(boxA.getRealB(charA).X, boxB.getRealB(charB).X) - Math.Max(boxA.getRealA(charA).X, boxB.getRealA(charB).X);

        // Verifique quem está à esquerda
        if (boxA.getRealA(charA).X < boxB.getRealA(charB).X) {
            // A está à esquerda de B; mova-os para afastá-los até o limite da sobreposição
            charA.Position.X -= overlapX / 2;
            charB.Position.X += overlapX / 2;
        } else {
            // B está à esquerda de A; mova-os para afastá-los até o limite da sobreposição
            charA.Position.X += overlapX / 2;
            charB.Position.X -= overlapX / 2;
        }
    }

    // Get methods
    public Vector2f getRealA(Character charA) {
        float X = charA.facing == -1 ? charA.VisualPosition.X - this.pB.X + 250 : charA.VisualPosition.X + this.pA.X;
        float Y = charA.VisualPosition.Y + this.pA.Y;

        return new Vector2f(X, Y);
    }
    public Vector2f getRealB(Character charA) {
        float X = charA.facing == -1 ? charA.VisualPosition.X - this.pA.X + 250 : charA.VisualPosition.X + this.pB.X;
        float Y = charA.VisualPosition.Y + this.pB.Y;

        return new Vector2f(X, Y);
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