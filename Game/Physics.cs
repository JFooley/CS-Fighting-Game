using Character_Space;
using SFML.System;

namespace Aux_Space {

public class Physics {
    public float counter = 0;

    float start_Y;
    float target_Y;
    float max_Y;
    int anim_length;
    
    private float GetTrajectory(float start_Y, float target_Y, float max_Y, int anim_length) {
        if (this.counter == 0) {
            this.start_Y = start_Y;
            this.target_Y = target_Y;
            this.max_Y = max_Y;
            this.anim_length = anim_length;
        }
        this.counter++;
        
        float a = (this.start_Y - 2 * this.max_Y + this.target_Y) / (float)(this.anim_length * this.anim_length);
        float b = (2 * this.max_Y - 2 * this.start_Y) / (float)this.anim_length;
        float c = this.start_Y;

        // Calcula o valor de Y na parábola para o current_X dado
        float y = a * this.counter * this.counter + b * this.counter + c;

        if (this.counter >= this.anim_length) { 
            this.counter = 0;
        }

        return y;
    }

    public void reset() {
        this.counter = 0;
    }

    public void Update(Character character) {
        if (character.Velocity.Z > 0) {
            if (character.Velocity.X != 0) {
                character.Position.X += character.Velocity.X * character.facing;
            }

            character.Position.Y = this.GetTrajectory(
                start_Y: character.Position.Y, 
                target_Y: character.floorLine, 
                max_Y: character.Position.Y - (int) character.Velocity.Y * 2, 
                anim_length: (int) character.Velocity.Z
            );

            character.Velocity.Z -= 1;
        } else {
            character.Velocity.X = 0;
            character.Velocity.Y = 0;
        }
    }
}


}