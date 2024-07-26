using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Essa classe gerencia os botões virtuais, seja pra teclado ou joystick, podendo fazer
// o mapeamento automático se o joystick for conectado ou desconectado.

namespace Input_Space
{
    public class InputManager
    {
        public const int KEYBOARD_INPUT = 0;
        public const int JOYSTICK_INPUT = 1;

        private int inputDevice;
        private bool autoDetectDevice;
        private int buttonState;
        private int buttonLastState;

        private Dictionary<Keys, int> keyMap;
        private Dictionary<int, int> joystickMap;

        public InputManager(int inputDevice, bool autoDetectDevice = true)
        {
            this.inputDevice = inputDevice;
            this.autoDetectDevice = autoDetectDevice;
            this.buttonState = 0b0;
            this.buttonLastState = 0b0;

            keyMap = new Dictionary<Keys, int>
            {
                { Keys.Q, 0 },  // Mapeia tecla física Q para botão virtual 0 (A)
                { Keys.W, 1 },  // Mapeia tecla física W para botão virtual 1 (B)
                { Keys.E, 2 },  // Mapeia tecla física E para botão virtual 2 (C)
                { Keys.R, 3 },  // Mapeia tecla física R para botão virtual 3 (D)
                { Keys.Enter, 4 },  // Mapeia tecla física Enter para botão virtual Start
                { Keys.Space, 5 },  // Mapeia tecla física Space para botão virtual Select
                { Keys.Up, 6 },  // Mapeia tecla física Up para botão virtual Up
                { Keys.Down, 7 },  // Mapeia tecla física Down para botão virtual Down
                { Keys.Left, 8 },  // Mapeia tecla física Left para botão virtual Left
                { Keys.Right, 9 }   // Mapeia tecla física Right para botão virtual Right
            };

            joystickMap = new Dictionary<int, int>
            {
                { 0x1000, 0 },  // Mapeia botão A do controle para botão virtual 0 (A)
                { 0x2000, 1 },  // Mapeia botão B do controle para botão virtual 1 (B)
                { 0x4000, 2 },  // Mapeia botão X do controle para botão virtual 2 (C)
                { 0x8000, 3 },  // Mapeia botão Y do controle para botão virtual 3 (D)
                { 0x0010, 4 },  // Mapeia botão Start do controle para botão virtual Start
                { 0x0020, 5 },  // Mapeia botão Back do controle para botão virtual Select
                { 0x0001, 6 },  // Mapeia D-Pad Up do controle para botão virtual Up
                { 0x0002, 7 },  // Mapeia D-Pad Down do controle para botão virtual Down
                { 0x0004, 8 },  // Mapeia D-Pad Left do controle para botão virtual Left
                { 0x0008, 9 }   // Mapeia D-Pad Right do controle para botão virtual Right
            };
        }

        public void Update()
        {
            if (autoDetectDevice && JoystickInput.IsJoystickConnected())
            {
                inputDevice = JOYSTICK_INPUT;
            }
            else
            {
                inputDevice = KEYBOARD_INPUT;
            }

            int currentInput = 0;

            if (inputDevice == KEYBOARD_INPUT)
            {
                currentInput = RawInput.ReadKeyboardState(keyMap);
            }
            else if (inputDevice == JOYSTICK_INPUT)
            {
                currentInput = JoystickInput.ReadJoystickState(joystickMap);
            }
            else
            {
                throw new InvalidOperationException("Invalid input device");
            }

            buttonLastState = buttonState;
            buttonState = 0;

            for (int i = 0; i < 10; i++)
            {
                if ((currentInput & (1 << i)) != 0)
                {
                    buttonState |= (1 << i);
                }
            }
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
