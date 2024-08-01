using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Input_Space
{
public class InputManager {
    public const int KEYBOARD_INPUT = 0;
    public const int JOYSTICK_INPUT = 1;

    private int inputDevice;
    private bool autoDetectDevice;
    private int buttonState;
    private int buttonLastState;

    // Mapping para teclado e mouse
    private Dictionary<Keys, int> keyMap;
    private Dictionary<int, int> joystickMap;

    // Buffer de inputs para os últimos 240 frames
    private readonly Queue<int> inputBuffer;
    private const int BufferSize = 240;

    // Singleton
    private static InputManager instance;

    // Construtor
    private InputManager(int inputDevice, bool autoDetectDevice = true)
    {
        this.inputDevice = inputDevice;
        this.autoDetectDevice = autoDetectDevice;
        this.buttonState = 0b0;
        this.buttonLastState = 0b0;

        keyMap = new Dictionary<Keys, int>
        {
            { Keys.Q, 0 },
            { Keys.W, 1 },
            { Keys.E, 2 },
            { Keys.R, 3 },
            { Keys.Enter, 4 },
            { Keys.Space, 5 },
            { Keys.Up, 6 },
            { Keys.Down, 7 },
            { Keys.Left, 8 },
            { Keys.Right, 9 }
        };

        joystickMap = new Dictionary<int, int>
        {
            { 0x1000, 0 },
            { 0x2000, 1 },
            { 0x4000, 2 },
            { 0x8000, 3 },
            { 0x0010, 4 },
            { 0x0020, 5 },
            { 0x0001, 6 },
            { 0x0002, 7 },
            { 0x0004, 8 },
            { 0x0008, 9 }
        };

        // Inicializa o buffer de inputs
        inputBuffer = new Queue<int>(BufferSize);
    }

    // Singleton
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager(KEYBOARD_INPUT, true);
            }
            return instance;
        }
    }
    public static void Initialize(int inputDevice, bool autoDetectDevice = true)
    {
        if (instance == null)
        {
            instance = new InputManager(inputDevice, autoDetectDevice);
        }
    }

    public void Update()
    {
        if (autoDetectDevice && JoystickInput.IsJoystickConnected()) {
            inputDevice = JOYSTICK_INPUT;
        }
        else {
            inputDevice = KEYBOARD_INPUT;
        }

        int currentInput = 0;

        if (inputDevice == KEYBOARD_INPUT) {
            currentInput = RawInput.ReadKeyboardState(keyMap);
        }
        else if (inputDevice == JOYSTICK_INPUT) {
            currentInput = JoystickInput.ReadJoystickState(joystickMap);
        }
        else {
            throw new InvalidOperationException("Invalid input device"); }

        buttonLastState = buttonState;
        buttonState = 0;

        for (int i = 0; i < 10; i++) {
            if ((currentInput & (1 << i)) != 0) {
                buttonState |= (1 << i);
            }
        }

        // Adiciona o estado atual do botão no buffer
        if (inputBuffer.Count >= BufferSize) {
            inputBuffer.Dequeue();
        }
        inputBuffer.Enqueue(buttonState);
    }

    public bool Key_hold(int button)
    {
        return (buttonState & (1 << button)) != 0;
    }

    public bool Key_down(int button)
    {
        return (buttonState & (1 << button)) != 0 && (buttonLastState & (1 << button)) == 0;
    }

    public bool Key_up(int button)
    {
        return (buttonState & (1 << button)) == 0 && (buttonLastState & (1 << button)) != 0;
    }

    public bool CheckString(int[] sequence, int maxFrames){
        int frameCount = inputBuffer.Count;
        int sequenceLength = sequence.Length;

        // Converter a fila para uma lista temporária para acesso por índice
        List<int> bufferList = inputBuffer.ToList();

        // Primeiro, verificar se o último botão da sequência foi pressionado neste frame
        if (frameCount < 2 || (bufferList[frameCount - 1] & (1 << sequence[sequenceLength - 1])) == 0 || (bufferList[frameCount - 2] & (1 << sequence[sequenceLength - 1])) != 0) {
            return false;
        }

        // Começa a verificar a sequência de trás para frente
        int currentIndex = sequenceLength - 1;
        int currentFrame = frameCount - 1;

        for (int i = currentIndex; i >= 0; i--) {
            bool found = false;
            for (int j = currentFrame - 1; j >= Math.Max(0, currentFrame - maxFrames); j--) {
                // Verifica se o botão foi apertado (não estava pressionado antes e foi pressionado depois)
                if ((bufferList[j] & (1 << sequence[i])) == 0 && (bufferList[j + 1] & (1 << sequence[i])) != 0) {
                    found = true;
                    currentFrame = j;
                    break;
                }
            } if (!found) {
                return false;
            }
        }
        return true;
    }

}
public static class RawInput
{
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(Keys vKey);

    public static int ReadKeyboardState(Dictionary<Keys, int> keyMap)
    {
        int state = 0;

        foreach (var key in keyMap)
        {
            if ((GetAsyncKeyState(key.Key) & 0x8000) != 0)
            {
                state |= (1 << key.Value);
            }
        }

        return state;
    }
}

public class JoystickInput
{
    [DllImport("xinput1_4.dll")]
    private static extern int XInputGetState(int dwUserIndex, out XINPUT_STATE pState);

    [DllImport("xinput1_4.dll")]
    private static extern int XInputSetState(int dwUserIndex, ref XINPUT_VIBRATION pVibration);

    private const int ERROR_DEVICE_NOT_CONNECTED = 1167;

    [StructLayout(LayoutKind.Sequential)]
    private struct XINPUT_STATE
    {
        public uint dwPacketNumber;
        public XINPUT_GAMEPAD Gamepad;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XINPUT_GAMEPAD
    {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XINPUT_VIBRATION
    {
        public ushort wLeftMotorSpeed;
        public ushort wRightMotorSpeed;
    }

    public static int ReadJoystickState(Dictionary<int, int> joystickMap)
    {
        XINPUT_STATE state;
        int result = XInputGetState(0, out state);

        if (result == 0)
        {
            int stateValue = 0;
            foreach (var button in joystickMap)
            {
                if ((state.Gamepad.wButtons & button.Key) != 0)
                {
                    stateValue |= (1 << button.Value);
                }
            }
            return stateValue;
        }
        return 0;
    }

    public static bool IsJoystickConnected()
    {
        XINPUT_STATE state;
        int result = XInputGetState(0, out state);

        return result == 0;
    }
}

}
