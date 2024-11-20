using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Input_Space
{
public class InputManager {    
    public const int NONE_INPUT = 0;
    public const int KEYBOARD_INPUT = 1;
    public const int JOYSTICK_0_INPUT = 2;
    public const int JOYSTICK_1_INPUT = 3;

    public const int DEFAULT = 0;
    public const int PLAYER_A = 1;
    public const int PLAYER_B = 2;

    private static Dictionary<int, Dictionary<string, int>> keysTranslationMap = new Dictionary<int, Dictionary<string, int>>
    {
        { -1, new Dictionary<string, int>
            {
                { "A", 0 },
                { "B", 1 },
                { "C", 2 },
                { "D", 3 },
                { "LB", 4 },
                { "LT", 5 },
                { "RB", 6 },
                { "RT", 7 },
                { "Up", 8 },
                { "Down", 9 },
                { "Left", 11 },
                { "Right", 10 },
                { "Start", 12 },
                { "Select", 13 },
            }
        },
        { 1, new Dictionary<string, int>
            {
                { "A", 0 },
                { "B", 1 },
                { "C", 2 },
                { "D", 3 },
                { "LB", 4 },
                { "LT", 5 },
                { "RB", 6 },
                { "RT", 7 },
                { "Up", 8 },
                { "Down", 9 },
                { "Left", 10 },
                { "Right", 11 },
                { "Start", 12 },
                { "Select", 13 },
            }
        }
    };

    private int maxButtonIndex = 14;

    private bool autoDetectDevice;
    private int[] inputDevice = new int[3];
    public int[] buttonState = new int[3];
    public int[] buttonLastState = new int[3];

    public bool anyKey => this.buttonState[DEFAULT] > 0;

    // Mapping para teclado e mouse
    private Dictionary<Keys, int> keyMap;
    private Dictionary<int, int> joystickMap;

    // Buffer de inputs para os últimos 240 frames
    private const int BufferSize = 240;
    private readonly Queue<int> inputBuffer = new Queue<int>(BufferSize);
    private readonly Queue<int> inputBuffer_A = new Queue<int>(BufferSize);
    private readonly Queue<int> inputBuffer_B = new Queue<int>(BufferSize);
    private Queue<int>[] buffers;

    // Singleton
    private static InputManager instance;

    // Construtor
    private InputManager(bool autoDetectDevice = true)
    {
        this.autoDetectDevice = autoDetectDevice;
        for (int i = 0; i < 3; i++) {
            this.buttonState[i] = 0b0;
            this.buttonLastState[i] = 0b0;
        }
        inputDevice[0] = KEYBOARD_INPUT;
        inputDevice[1] = JOYSTICK_0_INPUT;
        inputDevice[2] = JOYSTICK_1_INPUT;

        this.keyMap = new Dictionary<Keys, int>
        {
            { Keys.A, 0 },      // A
            { Keys.S, 1 },      // B
            { Keys.Q, 2 },      // X
            { Keys.W, 3 },      // Y
            { Keys.R, 4 },      // L
            { Keys.F, 5 },      // LT
            { Keys.E, 6 },      // R
            { Keys.D, 7 },      // RT
            { Keys.Up, 8 },     // Up
            { Keys.Down, 9 },   // Down
            { Keys.Left, 10 },  // Left
            { Keys.Right, 11 }, // Right
            { Keys.Enter, 12 }, // Start
            { Keys.Space, 13 }, // Select
        };

        this.joystickMap = new Dictionary<int, int>
        {
            { 0x1000, 0 },      // A
            { 0x2000, 1 },      // B
            { 0x4000, 2 },      // X
            { 0x8000, 3 },      // Y
            { 0x0100, 4 },      // L
            { -1, 5 },          // LT
            { 0x0200, 6 },      // R
            { -2, 7 },          // RT
            { 0x0001, 8 },      // Up
            { 0x0002, 9 },      // Down
            { 0x0004, 10 },     // Esquerda
            { 0x0008, 11 },     // Direita
            { 0x0010, 12 },     // Start
            { 0x0020, 13 },     // Select
        };

        this.buffers = new Queue<int>[] {inputBuffer, inputBuffer_A, inputBuffer_B};
    }

    // Singleton
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager(true);
            }
            return instance;
        }
    }
    public static void Initialize(bool autoDetectDevice = true)
    {
        if (instance == null)
        {
            instance = new InputManager(autoDetectDevice);
        }
    }

    // Behaviour
    public void Update() {
        if (autoDetectDevice && JoystickInput.IsJoystickConnected(0) && JoystickInput.IsJoystickConnected(1)) {
            inputDevice[0] = KEYBOARD_INPUT;
            inputDevice[1] = JOYSTICK_0_INPUT;
            inputDevice[2] = JOYSTICK_1_INPUT;
        }
        else if (autoDetectDevice && JoystickInput.IsJoystickConnected(0)) {
            inputDevice[0] = JOYSTICK_0_INPUT;
            inputDevice[1] = JOYSTICK_0_INPUT;
            inputDevice[2] = KEYBOARD_INPUT;
        }
        else {
            inputDevice[0] = KEYBOARD_INPUT;
            inputDevice[1] = KEYBOARD_INPUT;
            inputDevice[2] = NONE_INPUT;
        }

        int[] currentInput =  new int[3] {0, 0, 0};
        for (int i = 0; i < 3; i++) {
            if (inputDevice[i] == KEYBOARD_INPUT) {
                currentInput[i] = RawInput.ReadKeyboardState(keyMap);
            }
            else if (inputDevice[i] == JOYSTICK_0_INPUT) {
                currentInput[i] = JoystickInput.ReadJoystickState(joystickMap, dwUserIndex: 0);
            }
            else if (inputDevice[i] == JOYSTICK_1_INPUT) {
                currentInput[i] = JoystickInput.ReadJoystickState(joystickMap, dwUserIndex: 1);
            }
        }

        for (int j = 0; j < 3; j++) {
            buttonLastState[j] = buttonState[j];
            buttonState[j] = 0;
            for (int i = 0; i < this.maxButtonIndex; i++) {
                if ((currentInput[j] & (1 << i)) != 0) {
                    buttonState[j] |= (1 << i);
                }
            }

            // Adiciona o estado atual do botão no buffer
            if (buffers[j].Count >= BufferSize) {
                buffers[j].Dequeue();
            }
            buffers[j].Enqueue(buttonState[j]);
        }


    }

    // Key Detection
    public bool Key_hold(String key, int player = DEFAULT, int facing = 1) {
        return (buttonState[player] & (1 << keysTranslationMap[facing][key])) != 0;
    }
    public bool Key_down(String key, int player = DEFAULT, int facing = 1) {
        return (buttonState[player] & (1 << keysTranslationMap[facing][key])) != 0 && (buttonLastState[player] & (1 << keysTranslationMap[facing][key])) == 0;
    }
    public bool Key_up(String key, int player = DEFAULT, int facing = 1) {
        return (buttonState[player] & (1 << keysTranslationMap[facing][key])) == 0 && (buttonLastState[player] & (1 << keysTranslationMap[facing][key])) != 0;
    }
    public bool Key_change(String key, int player = DEFAULT, int facing = 1) {
        return (buttonState[player] & (1 << keysTranslationMap[facing][key])) != (buttonLastState[player] & (1 << keysTranslationMap[facing][key]));
    }
    public bool Was_down(String rawSequenceString, int maxFrames, bool flexEntry = true, int player = DEFAULT, int facing = 1) {
        int[] sequence = rawSequenceString.Split(' ').Select(key => keysTranslationMap[facing][key]).ToArray();

        // Converter a fila para uma lista temporária para acesso por índice
        List<int> bufferList = buffers[player].ToList();

        // Começa a verificar a sequência de trás para frente
        int currentIndex = sequence.Length - 1;
        int currentFrame = inputBuffer.Count - 1;

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
    public bool Key_press(String key, int player = DEFAULT, int facing = 1) {
        List<int> bufferList = buffers[player].ToList();
        for (int i = bufferList.Count() - 1; i > (bufferList.Count() - Config.inputWindowTime); i--) {
            if ((bufferList[i] & (1 << keysTranslationMap[facing][key])) != 0 && (bufferList[i-1] & (1 << keysTranslationMap[facing][key])) == 0) {
                return true;
            };
        }
        return false;
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
                state |= 1 << key.Value;
            }
        }

        return state;
    }
}

public class JoystickInput {
    [DllImport("xinput1_3.dll")]
    private static extern int XInputGetState(int dwUserIndex, out XINPUT_STATE pState);

    [DllImport("xinput1_3.dll")]
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

    public static int ReadJoystickState(Dictionary<int, int> joystickMap, int dwUserIndex = 0)
    {
        XINPUT_STATE state;
        int result = XInputGetState(dwUserIndex, out state);

        if (result == 0)
        {
            int stateValue = 0;
            foreach (var button in joystickMap)
            {   
                if (button.Key == -1) {
                    if (state.Gamepad.bLeftTrigger > 0) stateValue |= (1 << button.Value);
                } else if (button.Key == -2) {
                    if (state.Gamepad.bRightTrigger > 0) stateValue |= (1 << button.Value);
                } else if ((state.Gamepad.wButtons & button.Key) != 0) {
                    stateValue |= (1 << button.Value);
                }
            }
            return stateValue;
        }
        return 0;
    }

    public static bool IsJoystickConnected(int dwUserIndex = 0)
    {
        XINPUT_STATE state;
        int result = XInputGetState(dwUserIndex, out state);

        return result == 0;
    }
}

}
}
