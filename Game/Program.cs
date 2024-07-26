using Input_Space;
using System;

public static class Program
{
    public static void Main()
    {
        InputManager inputManager = new InputManager(InputManager.KEYBOARD_INPUT, true);

        while (true)
        {
            inputManager.Update();

            if (inputManager.IsButtonDown(0)) // Verifica se o botão virtual A foi pressionado
            {
                Console.WriteLine("Botão A pressionado");
            }


            System.Threading.Thread.Sleep(16); // Para evitar uso excessivo de CPU, faça uma pequena pausa
        }
    }
}

