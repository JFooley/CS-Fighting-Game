namespace Aux_Space {

public class Physics {
    public float counter = 0;

    int start_Y;
    int target_Y;
    int max_Y;
    int anim_length;
    
    public int GetTrajectory(int start_Y, int target_Y, int max_Y, int anim_length) {
        if (this.counter == 0) {
            this.start_Y = start_Y;
            this.target_Y = target_Y;
            this.max_Y = max_Y;
            this.anim_length = anim_length;
        }
        this.counter++;
        
        double a = (this.start_Y - 2 * this.max_Y + this.target_Y) / (double)(this.anim_length * this.anim_length);
        double b = (2 * this.max_Y - 2 * this.start_Y) / (double)this.anim_length;
        double c = this.start_Y;

        // Calcula o valor de Y na parábola para o current_X dado
        double y = a * this.counter * this.counter + b * this.counter + c;

        if (this.counter >= this.anim_length) { 
            this.counter = 0;
        } else if (y == target_Y ) {
            this.counter -= 1;
        }

        return (int)Math.Round(y);
    }

    public void reset() {
        this.counter = 0;
    }

}


}