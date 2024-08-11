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

    public float camera_zoom = 0.3f;

    private RenderWindow _window;
    private View _view;

    // Camera position
    public int X { get; private set; }
    public int Y { get; private set; }

    private Camera(RenderWindow window, View view, int X = 0, int Y = 0)
    {
        this.X = X;
        this.Y = Y;
        this._window = window;
        this._view = view;
    }

    public static Camera GetInstance(RenderWindow window = null, View view = null, int X = 0, int Y = 0)
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new Camera(window, view, X, Y);
            }
            return _instance;
        }
    }

    public void SetChars(Character charA, Character charB)
    {
        this.CharA = charA;
        this.CharB = charB;
    }

    public void SetLimits(int length, int height) {
        this.X_stage_limits = length;
        this.Y_stage_limits = height;
    }

    public void Update()
    {   
        // Camera to center between players
        if (CharA != null && CharB != null)
        {
            this.X = (this.CharA.PositionX + this.CharB.PositionX) / 2;
            this.Y = ((this.CharA.PositionY + this.CharB.PositionY) / 2) - 125;
        }

        // Limit camera pos
        float halfViewWidth = this._view.Size.X / 2;
        float halfViewHeight = this._view.Size.Y / 2;
        this.X = (int) Math.Max(halfViewWidth, Math.Min(this.X, this.X_stage_limits - halfViewWidth));
        this.Y = (int) Math.Max(halfViewHeight, Math.Min(this.Y, this.Y_stage_limits - halfViewHeight));

        // View pos to camera pos
        this._view.Center = new Vector2f(this.X, this.Y);
        this._window.SetView(this._view);
    }

}
