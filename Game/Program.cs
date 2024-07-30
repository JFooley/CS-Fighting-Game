using Input_Space;
using Animation_Space;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

public static class Program
{
    public static void Main()
    {
        InputManager inputManager = new InputManager(InputManager.KEYBOARD_INPUT, true);

        // Crie uma janela
        RenderWindow window = new RenderWindow(new VideoMode(1280, 720), "Game");
        window.Closed += (sender, e) => window.Close();

        // Defina algumas GenericBoxes
        var hitbox1 = new GenericBox(0, 0, 10, 10, 1);
        var hurtbox1 = new GenericBox(0, 0, 10, 10, 2);

        // Crie frames de animação para diferentes estados
        var idleFrames = new List<FrameData> {
            new FrameData(14657, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14658, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14659, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14660, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14661, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14662, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14663, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14664, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14665, 0, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14666, 0, 0, new List<GenericBox> { hurtbox1 }),
        };

        var AFrames = new List<FrameData> {
            new FrameData(15008, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15009, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15010, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15011, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 }),
            new FrameData(15012, 0, 0, new List<GenericBox> { hitbox1, hurtbox1 })
        };

        var walkingForwardFrames = new List<FrameData> {
            new FrameData(14672, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14673, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14674, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14675, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14676, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14677, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14678, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14679, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14680, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14681, 15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14682, 15, 0, new List<GenericBox> { hurtbox1 })
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(14684, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14685, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14686, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14687, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14688, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14689, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14690, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14691, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14692, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14693, -15, 0, new List<GenericBox> { hurtbox1 }),
            new FrameData(14694, -15, 0, new List<GenericBox> { hurtbox1 })
        };

        // Crie animações
        var idleAnimation = new Animation(idleFrames, "Idle");
        var AAnimation = new Animation(AFrames, "Idle");
        var walkingFAnimation = new Animation(walkingForwardFrames, "Idle");
        var walkingBAnimation = new Animation(walkingBackwardFrames, "Idle");

        // Crie um dicionário de animações
        var animations = new Dictionary<string, Animation>
        {
            { "Idle", idleAnimation},
            { "AAttack", AAnimation},
            { "WalkingForward", walkingFAnimation},
            { "WalkingBackward", walkingBAnimation}
        };

        // Crie um personagem com as animações e estado inicial
        var character = new Character("Ken", animations, "Idle", 100, 100, "D:/GABRIEL/Repositórios/Testes em C#/Game/Assets/chars/Ken");
        character.LoadSpriteImages();

        while (window.IsOpen) {   
            window.DispatchEvents();
            window.Clear(Color.Black);
            inputManager.Update();
            character.Update();

            // Temporário
            if ((character.CurrentState == "WalkingForward" || character.CurrentState == "WalkingBackward") & (!inputManager.Key_hold(8) & !inputManager.Key_hold(9))) {
                character.ChangeState("Idle");
            }

            if (inputManager.Key_down(0)) {
                character.ChangeState("AAttack");
            }

            if (inputManager.Key_hold(8)) {
                character.ChangeState("WalkingBackward");

            } else if (inputManager.Key_hold(9)) {
                character.ChangeState("WalkingForward");
            }

            // Temporário
            Sprite temp_sprite = new Sprite(character.GetCurrentSpriteImage()) {
                Position = new Vector2f(character.PositionX, character.PositionY) // Define a posição inicial do temp_sprite
            };
            temp_sprite.Scale = new Vector2f(2.0f, 2.0f);
            window.Draw(temp_sprite);

            // DEBUG
            Console.Clear();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Posição X: " + character.PositionX + " Posição Y: " + character.PositionY);
            Console.WriteLine("State: " + character.CurrentState + " Frame Index: " + character.CurrentAnimation.currentFrameIndex + " Sprite Index: " + character.CurrentSprite);
            // DEBUG

            window.Display(); 
            Thread.Sleep(16);
        }
    }
}

