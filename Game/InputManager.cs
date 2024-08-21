using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Input_Space
{
public class InputManager {    
    public const int KEYBOARD_INPUT = 0;
    public const int JOYSTICK_INPUT = 1;

    private static Dictionary<string, int> keysTranslationMap = new Dictionary<string, int>
    {
        { "A", 0 },
        { "B", 1 },
        { "C", 2 },
        { "D", 3 },
        { "L", 4 },
        { "R", 5 },
        { "Up", 6 },
        { "Down", 7 },
        { "Left", 8 },
        { "Right", 9 },
        { "Start", 10 },
        { "Select", 11 },
    };
    private int maxButtonIndex = 12;

    private int inputDevice;
    private int inputDevice_A;
    private int inputDevice_B;
    private bool autoDetectDevice;
    public int buttonState;
    public int buttonLastState;

    public int getButtonState => this.buttonState;

    // Mapping para teclado e mouse
    private Dictionary<Keys, int> keyMap;
    private Dictionary<int, int> joystickMap;

    // Buffer de inputs para os últimos 240 frames
    private readonly Queue<int> inputBuffer;
    private readonly Queue<int> inputBuffer_A;
    private readonly Queue<int> inputBuffer_B;
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

        this.keyMap = new Dictionary<Keys, int>
        {
            { Keys.A, 0 },
            { Keys.S, 1 },
            { Keys.Q, 2 },
            { Keys.W, 3 },
            { Keys.D, 4 },
            { Keys.E, 5 },
            { Keys.Up, 6 },
            { Keys.Down, 7 },
            { Keys.Left, 8 },
            { Keys.Right, 9 },
            { Keys.Enter, 10 },
            { Keys.Select, 11 },
        };

        this.joystickMap = new Dictionary<int, int>
        {
            { 0x1000, 0 },      // A
            { 0x2000, 1 },      // B
            { 0x4000, 2 },      // X
            { 0x8000, 3 },      // Y
            { 0x0100, 4 },      // L
            { 0x0200, 5 },      // R
            { 0x0001, 6 },      // Up
            { 0x0002, 7 },      // Down
            { 0x0004, 8 },      // Esquerda
            { 0x0008, 9 },      // Direita
            { 0x0010, 10 },     // Start
            { 0x0020, 11 },     // Select
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

    // Behaviour
    public void Update() {
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

        for (int i = 0; i < this.maxButtonIndex; i++) {
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

    // Key Detection
    public bool Key_hold(String key) {
        return (buttonState & (1 << keysTranslationMap[key])) != 0;
    }
    public bool Key_down(String key) {
        return (buttonState & (1 << keysTranslationMap[key])) != 0 && (buttonLastState & (1 << keysTranslationMap[key])) == 0;
    }
    public bool Key_up(String key) {
        return (buttonState & (1 << keysTranslationMap[key])) == 0 && (buttonLastState & (1 << keysTranslationMap[key])) != 0;
    }
    public bool Key_change(String key) {
        return (buttonState & (1 << keysTranslationMap[key])) != (buttonLastState & (1 << keysTranslationMap[key]));
    }

    public bool Was_down(String[] rawSequence, int maxFrames, bool flexEntry = true) {
        int[] sequence = rawSequence.Select(key => keysTranslationMap[key]).ToArray();

        int frameCount = inputBuffer.Count;
        int sequenceLength = sequence.Length;

        // Converter a fila para uma lista temporária para acesso por índice
        List<int> bufferList = inputBuffer.ToList();

        // Começa a verificar a sequência de trás para frente
        int currentIndex = sequenceLength - 1;
        int currentFrame = frameCount - 1;

        for (int i = currentIndex; i >= 0; i--) {
            bool found = false;
            for (int j = currentFrame; j > 0 && j >= Math.Max(0, currentFrame - maxFrames); j--) {
                // Verifica se o botão foi pressionado neste frame (1 neste frame e 0 no frame anterior)
                if (i == 0 && flexEntry && (bufferList[j] & (1 << sequence[i])) != 0 ) {
                    found = true;
                    currentFrame = j - 1;
                    break;
                } else if ((bufferList[j] & (1 << sequence[i])) != 0 && (bufferList[j - 1] & (1 << sequence[i])) == 0) {
                    found = true;
                    currentFrame = j - 1;
                    break;
                } 
            }

            if (!found) {
                return false;
            }

            // Verifica se não há outros botões acionados se flexEntry é falso
            if (!flexEntry) {
                for (int j = Math.Max(0, currentFrame - maxFrames); j <= currentFrame; j++) {
                    if ((bufferList[j] & ~((1 << sequence[i]))) != 0) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

}

public static class RawInput {
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

public class JoystickInput {
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
