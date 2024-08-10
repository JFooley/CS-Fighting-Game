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

    private RenderWindow _window;

    // Camera position
    public int X { get; private set; }
    public int Y { get; private set; }

    public float size_ratio = 3.0f;

    private Camera(RenderWindow window, int X = 0, int Y = 0)
    {
        this.X = X;
        this.Y = Y;
        this._window = window;
    }

    public static Camera GetInstance(RenderWindow window = null, int X = 0, int Y = 0)
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new Camera(window, X, Y);
            }
            return _instance;
        }
    }

    public void SetChars(Character charA, Character charB)
    {
        this.CharA = charA;
        this.CharB = charB;
    }

    public void Update(RenderWindow window, View view)
    {
        if (CharA != null && CharB != null)
        {
            this.X = (this.CharA.PositionX + this.CharB.PositionX) / 2;
            this.Y = ((this.CharA.PositionY + this.CharB.PositionY) / 2) + 90;
        }

        view.Center = new Vector2f(this.X, this.Y);
        window.setView(view);
    }

    public Vector2f GetRealPosition(int X_sprite, int Y_sprite)
    {
        float X_real = (X_sprite - this.X) * this.size_ratio + (_window.Size.X / 2);
        float Y_real = (Y_sprite - this.Y) * this.size_ratio + (_window.Size.Y / 2);

        return new Vector2f(X_real, Y_real);
    }
}
