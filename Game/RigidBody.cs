using Character_Space;
using SFML.System;

public class RigidBody {
    public Vector2f LastPosition;
    public Vector2f Position;
    public Vector2f Velocity;
    public float gravidade = Config.Gravity; // Aceleração gravitacional
    public float atrito => gravidade * 0.5F;

    public RigidBody(float X = 0, float Y = 0) {
        this.Position = new Vector2f(X, Y);
    }

    public void Update(Character player) {
        this.LastPosition = Position;
        this.Position.X += this.Velocity.X * player.facing;
        this.Position.Y += this.Velocity.Y;

        this.CheckGravity(player);
        this.CheckFriction();
    }

    private void CheckGravity(Character player) {
        if (this.Position.Y < player.floorLine) {
            this.Velocity.Y += this.gravidade;
        } else {
            this.Velocity.Y = 0;
            this.Position.Y = player.floorLine;
            if (this.LastPosition.Y > player.floorLine && this.Position.Y == player.floorLine) this.Velocity.X = 0;
        }
    }

    private void CheckFriction() {
        if (this.Velocity.X != 0 && this.Velocity.Y == 0) {
            this.Velocity.X = Math.Abs(this.Velocity.X) > this.atrito ? this.Velocity.X + (this.atrito * -Math.Sign(this.Velocity.X)) : 0;
        } 
    }

    public void SetVelocity(float X = 0, float Y = 0, bool raw_set = false) {
        if (raw_set) this.Velocity = new Vector2f(X, -Y);
        else this.Velocity = new Vector2f(X, -this.CalcularForcaY(Y));
    }

    public void AddVelocity(float X = 0, float Y = 0) {
        this.Velocity += new Vector2f(X, Y);
    }

    private float CalcularForcaY(float deltaY) {
        if (deltaY <= 0) return 0;

        float forcaY = (float)Math.Sqrt(2 * this.gravidade * deltaY);

        return forcaY;
    }

}