using SFML.Graphics;
using SFML.System;
using Character_Space;
using SFML.Window;

public class Camera
{
    private static Camera _instance;
    private static readonly object _lock = new object();

    public Character CharA { get; private set; }
    public Character CharB { get; private set; }

    public int X_stage_limits = 0;
    public int Y_stage_limits = 0;

    public RenderWindow window;

    // Camera Position
    public float X { get; private set; }
    public float Y { get; private set; }

    private Camera(RenderWindow window, int X = Config.RenderWidth/2, int Y = Config.RenderHeight/2) {
        this.X = X;
        this.Y = Y;
        this.window = window;
    }

    public static Camera Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new Camera(null, 0, 0);
            }
            return _instance;
        }
    }

    public static Camera GetInstance(RenderWindow window = null, int X = Config.RenderWidth/2, int Y = Config.RenderHeight/2) {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new Camera(window, X, Y);
            }
            return _instance;
        }
    }

    public void SetWindow(RenderWindow window) {
        this.window = window;
    }
    public void SetChars(Character charA, Character charB) {
        this.CharA = charA;
        this.CharB = charB;
    }
    public void SetLimits(int length, int height) {
        this.X_stage_limits = length;
        this.Y_stage_limits = height;
    }
    public void Update() {   
        // Camera to center between players
        if (CharA != null && CharB != null) {
            this.X = (this.CharA.body.Position.X + this.CharB.body.Position.X) / 2;
            this.Y = ((this.CharA.body.Position.Y + this.CharB.body.Position.Y) / 2) - 125;

            // Limit camera pos
            float halfViewWidth = Program.view.Size.X / 2;
            float halfViewHeight = Program.view.Size.Y / 2;
            this.X = (int) Math.Max(halfViewWidth, Math.Min(this.X, this.X_stage_limits - halfViewWidth));
            this.Y = (int) Math.Max(halfViewHeight, Math.Min(this.Y, this.Y_stage_limits - halfViewHeight));

            // View pos to camera pos
            Program.view.Center = new Vector2f(this.X, this.Y);
            this.window.SetView(Program.view);
        } else {
            Program.view.Center = new Vector2f(Config.RenderWidth / 2, Config.RenderHeight / 2);
            this.window.SetView(Program.view);
        }
    }
    public void Reset() {
        this.CharA = null;
        this.CharB = null;
        this.X = Config.RenderWidth/2;
        this.Y = Config.RenderHeight/2;
        this.X_stage_limits = 0;
        this.Y_stage_limits = 0;
    }

}
