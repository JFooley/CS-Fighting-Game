using Input_Space;
using Animation_Space;
using System;

public static class Program
{
    public static void Main()
    {
        InputManager inputManager = new InputManager(InputManager.KEYBOARD_INPUT, true);

        // Defina algumas GenericBoxes
        var hitbox1 = new GenericBox(0, 0, 10, 10, 1);
        var hurtbox1 = new GenericBox(0, 0, 10, 10, 2);

        // Crie frames de animação para diferentes estados
        var idleFrames = new List<FrameData> {
            new FrameData(0, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(1, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(2, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(3, 0, 0, new List<GenericBox> { hurtbox1 }),
        };

        var attackFrames = new List<FrameData> {
            new FrameData(4, 1, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(5, 1, 1, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(6, 1, 1, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(7, 1, -1, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(8, 1, -1, new List<GenericBox> { hitbox1, hurtbox1 }),
        };

        var walkingForwardFrames = new List<FrameData> {
            new FrameData(9, 1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(10, 1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(11, 1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(12, 1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(13, 1, 0, new List<GenericBox> { hurtbox1 }),
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(14, -1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(15, -1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(16, -1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(17, -1, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(18, -1, 0, new List<GenericBox> { hurtbox1 }),
        };

        // Crie animações
        var idleAnimation = new Animation(idleFrames, "Idle");
        var attackAnimation = new Animation(attackFrames, "Idle");
        var walkingFAnimation = new Animation(walkingForwardFrames, "Idle");
        var walkingBAnimation = new Animation(walkingBackwardFrames, "Idle");

        // Crie um dicionário de animações
        var animations = new Dictionary<string, Animation>
        {
            { "Idle", idleAnimation},
            { "Attack", attackAnimation},
            { "WalkingForward", walkingFAnimation},
            { "WalkingBackward", walkingBAnimation}
        };

        // Crie um personagem com as animações e estado inicial
        var character = new Character("Ken", animations, "Idle", 100, 100, "D:/GABRIEL/Repositórios/Testes em C#/Game/Assets/chars/Ken");

        character.LoadSpriteImages();

        while (true)
        {
            inputManager.Update();
            character.Update();

            if (inputManager.Key_down(0)) 
            {
                character.ChangeState("Attack");
            }

            if (inputManager.Key_hold(8)) {
                character.ChangeState("WalkingBackward");

            } else if (inputManager.Key_hold(9)) {
                character.ChangeState("WalkingForward");
            }

            Console.Clear();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Posição X: " + character.PositionX + " Posição Y: " + character.PositionY);
            Console.WriteLine("State: " + character.CurrentState + " Frame Index: " + character.CurrentAnimation.currentFrameIndex + " Sprite Index: " + character.CurrentSprite);

            System.Threading.Thread.Sleep(16); // Para evitar uso excessivo de CPU, faça uma pequena pausa
        }
    }
}

