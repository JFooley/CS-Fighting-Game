namespace Aux_Space {

public class Physics {
    public float counter = 0;
    float a;
    float b;

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

        if (this.counter >= this.anim_length) this.counter = 0;

        return (int)Math.Round(y);
    }

    public int GetTrajectoryOld(int start_position, int target_position, int max_Y, int t, ref int current_t) {
        current_t++;
        
  
        // Pontos
        float x0 = 0;
        float y0 = start_position;
        float x1 = t;
        float y1 = target_position;
        float x2 = t / 2;
        float y2 = max_Y;

        // Cálculo dos polinômios de Lagrange
        float L0 = ((current_t - x1) * (current_t - x2)) / ((x0 - x1) * (x0 - x2));
        float L1 = ((current_t - x0) * (current_t - x2)) / ((x1 - x0) * (x1 - x2));
        float L2 = ((current_t - x0) * (current_t - x1)) / ((x2 - x0) * (x2 - x1));

        // Cálculo de y
        float y = L0 * y0 + L1 * y1 + L2 * y2;


        if (current_t >= t) current_t = 0;
        return (int) y;
    }

}


}