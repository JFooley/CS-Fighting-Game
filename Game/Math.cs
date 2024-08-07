namespace Aux_Space {

public class Physics {
    public static int GetTrajectory(int max_Y, int max_T, int current_T = 0) {
        if (current_T < 0) current_T = 0;
        
        // A posição Y mais alta (ponto mais alto da curva)
        float c = max_Y;

        // O tempo total da trajetória
        float maxTime = max_T;

        // O tempo em que o objeto atinge o ponto mais alto
        float b = maxTime / 2.0f;

        // A constante 'a' que define a largura da parábola
        float a = c / (b * b);

        // Calcula a posição Y no tempo current_T
        float y = -a * (current_T - b) * (current_T - b) + c;

        return (int) (y);
    }
}


}