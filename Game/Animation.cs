using Character_Space;
using SFML.System;

namespace Animation_Space {

    public class Animation {
        public List<FrameData> Frames { get; private set; }
        public List<string> SimpleFrames { get; private set; }

        // logic
        public int current_frame_index;
        public bool on_last_frame;
        public bool ended;
        public bool has_frame_change => this.frame_counter == 0; 

        // infos
        public int anim_size;
        public int real_anim_size => anim_size * (60 / this.framerate);
        public bool loop;
        public int framerate;
        public int screen_framerate;
        public int frame_counter;

        public Animation(List<FrameData> frames, int framerate = 30, int screenFramerate = 60, bool loop = true) {
            this.Frames = frames;
            this.current_frame_index = 0;
            this.anim_size = Frames.Count() - 1;
            this.loop = loop;
            this.framerate = framerate;
            this.screen_framerate = screenFramerate;
            this.frame_counter = 0;
        }

        public Animation(List<string> SimpleFrames, int framerate = 30, int screenFramerate = 60) {
            this.SimpleFrames = SimpleFrames;
            this.current_frame_index = 0;
            this.anim_size = SimpleFrames.Count() - 1;
            this.loop = true;
            this.framerate = framerate;
            this.screen_framerate = screenFramerate;
            this.frame_counter = 0;
        }

        public FrameData GetCurrentFrame()
        {
            return this.Frames[current_frame_index];
        }

        public string GetCurrentSimpleFrame()
        {
            return this.SimpleFrames[current_frame_index];
        }

        public bool AdvanceFrame() {
            // Calcula o número de frames de tela por frame de animação
            float framesPerAnimFrame = screen_framerate / framerate;

            // Incrementa o contador de frames
            frame_counter++;

            // Avança o frame de animação apenas quando o contador de frames atinge o limite calculado
            if (frame_counter >= framesPerAnimFrame) {
                frame_counter = 0; // Reseta o contador
                current_frame_index++; // Avança um frame
                
                if (this.ended && this.loop) this.Reset();

                if (current_frame_index == anim_size) this.on_last_frame = true;
                else this.on_last_frame = false;
                
                if (current_frame_index > anim_size) {
                    this.ended = true;
                    this.current_frame_index -= 1;
                } else this.ended = false;
                
                return true; // do frame change
            }

            return false; // no frame change
        }

        public void Reset() {
            current_frame_index = 0;
            frame_counter = 0;
            on_last_frame = false;
            ended = false;
        }
    }

    public class GenericBox {
        public const int HITBOX = 0;
        public const int HURTBOX = 1;
        public const int PUSHBOX = 2;
        public const int PARRYBOX = 3;
        public const int GRABBOX = 4;

        public Vector2f pA;
        public Vector2f pB;

        public float width;
        public float height;

        public int type; // 0 Hitbox, 1 Hurtbox, 2 Pushbox, 3 ParryBox, 4 Grabbox, <0 Animation Anchor
        public int quadsize = 250;

        public GenericBox(int type, int x1, int y1, int x2, int y2) {   
            this.type = type;
            this.pA.X = x1;
            this.pB.X = x2;
            this.pA.Y = y1;
            this.pB.Y = y2;
            this.width = x2 - x1;
            this.height = y2 - y1;
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
                charA.body.Position.X -= overlapX / 2;
                charB.body.Position.X += overlapX / 2;
            } else {
                // B está à esquerda de A; mova-os para afastá-los até o limite da sobreposição
                charA.body.Position.X += overlapX / 2;
                charB.body.Position.X -= overlapX / 2;
            }
        }

        // Get methods
        public Vector2f getRealA(Character charA) {
            float X = charA.facing == -1 ? charA.VisualPosition.X - this.pB.X + this.quadsize : charA.VisualPosition.X + this.pA.X;
            float Y = charA.VisualPosition.Y + this.pA.Y;

            return new Vector2f(X, Y);
        }
        public Vector2f getRealB(Character charA) {
            float X = charA.facing == -1 ? charA.VisualPosition.X - this.pA.X + this.quadsize : charA.VisualPosition.X + this.pB.X;
            float Y = charA.VisualPosition.Y + this.pB.Y;

            return new Vector2f(X, Y);
        } 
    }

    public class FrameData {
        public bool hasHit;
        public string Sprite_index { get; set; }
        public string Sound_index { get; set; }
        public float DeltaX { get; set; }
        public float DeltaY { get; set; }
        public List<GenericBox> Boxes { get; set; }

        public FrameData(int sprite_index, float deltaX, float deltaY, List<GenericBox> boxes, string Sound_index = "", bool hasHit = true) {
            this.Sprite_index = sprite_index.ToString();
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.Boxes = boxes;
            this.Sound_index = Sound_index;
            this.hasHit = hasHit;
        }

        public FrameData(string sprite_index, float deltaX, float deltaY, List<GenericBox> boxes, string Sound_index = "", bool hasHit = true) {
            this.Sprite_index = sprite_index;
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.Boxes = boxes;
            this.Sound_index = Sound_index;
            this.hasHit = hasHit;
        }
    }

}