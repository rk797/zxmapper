using Nefarius.ViGEm.Client.Targets.Xbox360;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static interceptioncs;
using System.Reflection;
using System.IO;
using System.Windows.Forms;


namespace zxmapper
{
    public static class ScanCodes
    {
        public static int FORWARD { get; set; } = 0x11; // Up
        public static int LEFT { get; set; } = 0x1E; // Left
        public static int BACKWARDS { get; set; } = 0x1F; // Down
        public static int RIGHT { get; set; } = 0x20; // Right
        public static int ESC { get; set; } = 0x01;
        public static int JUMP { get; set; } = 0x39;
        public static int SLIDE { get; set; } = 0x2E; // Slide
        public static int RELOAD { get; set; } = 0x13; // Reload
        public static int TOGGLE { get; set; } = 0x100; // Toggle
        public static int XBUTTON5 { get; set; } = 0x100; // XButton5
        public static int XBUTTON4 { get; set; } = 0x80; // XButton4
        public static int INTERACT { get; set; } = 0x12; // Interact
        public static int ABILITY1 { get; set; } = 0x10; // Ability1
        public static int WEAPON1 { get; set; } = 0x02; // Swap weapons 1
        public static int WEAPON2 { get; set; } = 0x03; // Swap weapons 2
        public static int HOLSTER { get; set; } = 0x04; // Holster weapons
        public static int INSPECT { get; set; } = 0x2D; // Inspect
        public static int SHIELD { get; set; } = 0x05; // Shield
        public static int ULTIMATE { get; set; } = 0x22; // Ultimate
        public static int CTRL { get; set; } = 0x1D;
        public static int SHIFT { get; set; } = 0x2A;
        public static int ALT { get; set; } = 0x38;
        public static int DELETE { get; set; } = 0x53;
        public static int INVENTORY { get; set; } = 0x38;
        public static int PING { get; set; } = 0x21;
        public static int GRENADE { get; set; } = 0x2F;
        public static int MAP { get; set; } = 0x0F;
    }
    internal class Mapper
    {
        private ViGEmClient _client;
        private IXbox360Controller _controller;

        private IntPtr _interceptionContext;
        private int device;
        private Stroke stroke;
        private bool _locked = false;
        private bool _southpaw;
        private bool _isContextBindedToMouse;

        enum DirectionStateX
        {
            None,
            Left,
            Right
        }
        enum DirectionStateY
        {
            None,
            Up,
            Down
        }
        private DirectionStateX _lastDirectionX = DirectionStateX.None;
        private DirectionStateY _lastDirectionY = DirectionStateY.None;


        private int delayedKeyPressTime = 400;
        private bool _isDelayedResetInProgress = false;

        private int mouseResetDelay = 15;

        private double sensX;
        private double sensY;
        private double expFactor;
        private double scaleFactor;

        private const int smoothing = 8;
        private int smoothIndex = 0;
        private double[] smoothedX = new double[smoothing];
        private double[] smoothedY = new double[smoothing];
        private int capFactor = 350;

        Xbox360Axis currAxisMoveX = Xbox360Axis.LeftThumbX;
        Xbox360Axis currAxisMoveY = Xbox360Axis.LeftThumbY;
        Xbox360Axis currAxisMouseX = Xbox360Axis.RightThumbX;
        Xbox360Axis currAxisMouseY = Xbox360Axis.RightThumbY;

        private DateTime _lastMouseMoveTime = DateTime.Now;

        KeyStroke _currKeystroke;

        private int _keyboardDevice;
        private bool firstTimeSetup = true;


        // Fitlers
        ushort mouseFilterInit = (ushort)(FilterMouseState.All);

        ushort keyboardFilterInit = (ushort)(FilterKeyState.All);

        public Mapper()
        {
            LoadConfiguration("config.zxm");
            Initialize();
        }
        public void Initialize()
        {
            InitializeVigem();
            InitializeInterception();
            SetMapperAxis();

            Task.Run(() => zxmapperTask());
            Task.Run(() => CheckForMouseInactivity());
        }

        private void InitializeVigem()
        {
            _client = new ViGEmClient();
            _controller = _client.CreateXbox360Controller();
            _controller.Connect();
        }

        private void InitializeInterception()
        {
            _interceptionContext = interception_create_context();

            interception_set_filter(_interceptionContext, interception_is_mouse, mouseFilterInit);
            interception_set_filter(_interceptionContext, interception_is_keyboard, keyboardFilterInit);
        }


        public void SetContextToggle(bool bln)
        {
            _isContextBindedToMouse = bln; 
        }

        public void SetMapperAxis()
        {
            if (_southpaw)
            {
                currAxisMoveX = Xbox360Axis.RightThumbX;
                currAxisMoveY = Xbox360Axis.RightThumbY;
                currAxisMouseX = Xbox360Axis.LeftThumbX;
                currAxisMouseY = Xbox360Axis.LeftThumbY;
            }
            else
            {
                currAxisMoveX = Xbox360Axis.LeftThumbX;
                currAxisMoveY = Xbox360Axis.LeftThumbY;
                currAxisMouseX = Xbox360Axis.RightThumbX;
                currAxisMouseY = Xbox360Axis.RightThumbY;
            }
        }

        public void SwitchSouthPawState()
        {
            _southpaw = !_southpaw;
        }


        private bool _keyLeft = false;
        private bool _keyRight = false;
        private bool _keyUp = false;
        private bool _keyDown = false;
        private bool _keySlide = false;
        private bool _keyJump = false;
        private bool _keyReload = false;
        private bool _keyInteract = false;
        private bool _keyAbility1 = false;
        private bool _keyUltimate = false;
        private bool _keyShield = false;
        private bool _keySwapWeapons1 = false;
        private bool _keySwapWeapons2 = false;
        private bool _keyHolsterWeapons = false;
        private bool _keyPing = false;
        private bool _keyGrenade = false;
        private bool _keyZoom = false;
        private bool _keyMap = false;
        private bool _keyInventory = false;

        private bool _leftMouseDown = false;
        private bool _rightMouseDown = false;

        private bool _keyCtrl = false;
        private bool _keyAlt = false;
        private bool _keyDelete = false;

        private bool _ctrl = false;
        private bool _alt = false;
        private bool _delete = false;

        public void SetFilters()
        {
            ushort mouseFilter = _locked ? (ushort)(FilterMouseState.Button5Down) : mouseFilterInit;
            ushort keyboardFilter = _locked ? (ushort)FilterKeyState.None : keyboardFilterInit;
            interception_set_filter(_interceptionContext, interception_is_mouse, mouseFilter);
            interception_set_filter(_interceptionContext, interception_is_keyboard, keyboardFilter);
        }

        public void ReleaseFilters()
        {
            ushort mouseFilter = (ushort)(FilterMouseState.None);
            ushort keyboardFilter = (ushort)FilterKeyState.None;
            interception_set_filter(_interceptionContext, interception_is_mouse, mouseFilter);
            interception_set_filter(_interceptionContext, interception_is_keyboard, keyboardFilter);
        }

        private double ExponentialMapping(double value, double exponent, double scale)
        {
            double result = scale * Math.Pow(Math.Abs(value), exponent);
            return Math.Sign(value) * result;
        }

        private void zxmapperTask()
        {
            try
            {
                while (interception_receive(_interceptionContext, device = interception_wait(_interceptionContext), ref stroke, 1) > 0)
                {
                    if (interception_is_mouse(device) == 1)
                    {

                        MouseStroke mouseStroke = (MouseStroke)stroke;

                        if (((mouseStroke.state & (ushort)ScanCodes.TOGGLE) != 0))
                        {
                            SwitchLockedState();
                            SetFilters();
                        }

                        double scaledX = mouseStroke.x * sensX;
                        double scaledY = mouseStroke.y * sensY;

                        double expScaledX = ExponentialMapping(scaledX, expFactor / 100 /*exponent*/, scaleFactor / 100 /*scale*/);
                        double expScaledY = ExponentialMapping(scaledY, expFactor / 100 /*exponent*/, scaleFactor / 100 /*scale*/);

                        smoothedX[smoothIndex] = expScaledX;
                        smoothedY[smoothIndex] = expScaledY;

                        smoothIndex = (smoothIndex + 1) % smoothing;

                        double averageX = smoothedX.Average();
                        double averageY = smoothedY.Average();

                        averageX = Math.Max(Math.Min(averageX, capFactor), -capFactor);
                        averageY = Math.Max(Math.Min(averageY, capFactor), -capFactor);


                        short joystickX = (short)(averageX * short.MaxValue / capFactor);
                        short joystickY = (short)(averageY * short.MaxValue / -capFactor);
                        

                        joystickX = (short)Math.Max(Math.Min(joystickX, short.MaxValue), short.MinValue);
                        joystickY = (short)Math.Max(Math.Min(joystickY, short.MaxValue), short.MinValue);

                        //if ((mouseStroke.state & (ushort)MouseState.LeftDown) != 0)
                        //{
                        //    Debug.WriteLine("Left mouse down");

                        //}


                        if (mouseStroke.state == 1)
                        {
                            _leftMouseDown = true;
                        }
                        else if (mouseStroke.state == 2)
                        {
                            _leftMouseDown = false;
                        }
                        else if (mouseStroke.state == 4)
                        {
                            _rightMouseDown = true;
                        }
                        else if (mouseStroke.state == 8)
                        {
                            _rightMouseDown = false;
                        }

                        //if (_leftMouseDown)
                        //{
                        //    joystickY *= (short)2.2;
                        //}



                        _controller.SetAxisValue(currAxisMouseX, joystickX);
                        _controller.SetAxisValue(currAxisMouseY, joystickY);

                        UpdateMouseState();

                    }
                    else if (interception_is_keyboard(device) == 1)
                    {

                        if (firstTimeSetup)
                        {
                            _keyboardDevice = device;
                            firstTimeSetup = false;
                        }
                        KeyStroke keyStroke = (KeyStroke)stroke;

                        _currKeystroke = keyStroke;
                        int code = keyStroke.code;
                        bool isKeyDown = keyStroke.state == 0;
                        if (_ctrl && _alt && _delete)
                        {
                            SwitchLockedState();
                            SetFilters();
                        }

                        
                        //should probably use switch for speed but too lazy
                        if (code == ScanCodes.JUMP) _keyJump = isKeyDown;
                        else if (code == ScanCodes.LEFT)
                        { 
                            _keyLeft = isKeyDown;
                            if (isKeyDown) _lastDirectionX = DirectionStateX.Left;
                            else if (_lastDirectionX == DirectionStateX.Left && !_keyRight) _lastDirectionX = DirectionStateX.None;
                            else if (_lastDirectionX == DirectionStateX.Left && _keyRight) _lastDirectionX = DirectionStateX.Right;
                        }
                        else if (code == ScanCodes.RIGHT)
                        {
                            _keyRight = isKeyDown;
                            if (isKeyDown) _lastDirectionX = DirectionStateX.Right;
                            else if (_lastDirectionX == DirectionStateX.Right && !_keyLeft) _lastDirectionX = DirectionStateX.None;
                            else if (_lastDirectionX == DirectionStateX.Right && _keyLeft) _lastDirectionX = DirectionStateX.Left;
                        }
                        else if (code == ScanCodes.FORWARD)
                        {
                            _keyUp = isKeyDown;
                            if (isKeyDown) _lastDirectionY = DirectionStateY.Up;
                            else if (_lastDirectionY == DirectionStateY.Up) _lastDirectionY = DirectionStateY.None;
                            else if (_lastDirectionY == DirectionStateY.Up && _keyDown) _lastDirectionY = DirectionStateY.Down;

                        }
                        else if (code == ScanCodes.BACKWARDS)
                        {
                            _keyDown = isKeyDown;
                            if (isKeyDown) _lastDirectionY = DirectionStateY.Down;
                            else if (_lastDirectionY == DirectionStateY.Down && !_keyUp) _lastDirectionY = DirectionStateY.None;
                            else if (_lastDirectionY == DirectionStateY.Down && _keyUp) _lastDirectionY = DirectionStateY.Up;
                        }
                        else
                        {
                            byte[] strokeBytes = getBytes(keyStroke);
                            interception_send(_interceptionContext, device, strokeBytes, 1);
                        }

                        //else if (code == ScanCodes.SLIDE) _keySlide = isKeyDown;
                        //else if (code == ScanCodes.INTERACT) _keyInteract = isKeyDown;
                        //else if (code == ScanCodes.RELOAD) _keyReload = isKeyDown;
                        //else if (code == ScanCodes.ABILITY1) _keyAbility1 = isKeyDown;
                        //else if (code == ScanCodes.ULTIMATE) _keyUltimate = isKeyDown;
                        //else if (code == ScanCodes.WEAPON1) _keySwapWeapons1 = isKeyDown;
                        //else if (code == ScanCodes.WEAPON2) _keySwapWeapons2 = isKeyDown;
                        //else if (code == ScanCodes.HOLSTER) _keyHolsterWeapons = isKeyDown;
                        //else if (code == ScanCodes.CTRL) _keyCtrl = isKeyDown;
                        //else if (code == ScanCodes.ALT) _keyAlt = isKeyDown;
                        //else if (code == ScanCodes.DELETE) _keyDelete = isKeyDown;
                        //else if (code == ScanCodes.SHIELD) _keyShield = isKeyDown;
                        //else if (code == ScanCodes.INVENTORY) _keyInventory = isKeyDown;
                        //else if (code == ScanCodes.PING) _keyPing = isKeyDown;
                        //else if (code == ScanCodes.GRENADE) _keyGrenade = isKeyDown;
                        //else if (code == ScanCodes.SHIFT) _keyZoom = isKeyDown;
                        //else if (code == ScanCodes.MAP) _keyMap = isKeyDown;


                        if (code == 29) _ctrl = true;
                        if (code == 56) _alt = true;
                        if (code == 83) _delete = true;


                        UpdateControllerState();
                        CheckForAxisResetX();
                        CheckForAxisResetY();

                        if (!isKeyDown)
                        {
                            if ((!_keySwapWeapons1 || !_keySwapWeapons2) && !_isDelayedResetInProgress)
                            {
                                _controller.SetButtonState(Xbox360Button.Y, false);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in zxmapperTask: {ex.Message}");
                ReleaseFilters();
            }
        }

        private void PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            Debug.WriteLine(sb.ToString());
        }
        private async void performDelayedKeyPress(Xbox360Button button, bool state)
        {
            if (state)
            {
                _isDelayedResetInProgress = true;
                _controller.SetButtonState(button, true);
                await Task.Delay(delayedKeyPressTime);
                _controller.SetButtonState(button, false);
                _isDelayedResetInProgress = false;
            }
        }

        private void UpdateControllerState()
        {
            if (_keyCtrl && _keyAlt && _keyDelete)
            {
                SwitchLockedState();
                SetFilters();
            }
            _controller.SetButtonState(Xbox360Button.B, _keySlide);
            _controller.SetButtonState(Xbox360Button.A, _keyJump);
            _controller.SetButtonState(Xbox360Button.X, _keyReload || _keyInteract);
            _controller.SetButtonState(Xbox360Button.Up, _keyShield);
            _controller.SetButtonState(Xbox360Button.Guide, _keyInventory);
            _controller.SetButtonState(Xbox360Button.LeftShoulder, _keyAbility1 || _keyUltimate);
            _controller.SetButtonState(Xbox360Button.RightShoulder, _keyPing || _keyUltimate);
            _controller.SetButtonState(Xbox360Button.Right, _keyGrenade);
            _controller.SetButtonState(Xbox360Button.LeftThumb, _keyZoom);

            performDelayedKeyPress(Xbox360Button.Y, _keyHolsterWeapons);
            if (_keySwapWeapons1 || _keySwapWeapons2)
            {
                _controller.SetButtonState(Xbox360Button.Y, true);
            }
        }
        void UpdateMouseState()
        {

            if (_leftMouseDown)
            {
                _controller.SetSliderValue(Xbox360Slider.RightTrigger, byte.MaxValue);
            }
            else if (!_leftMouseDown)
            {
                _controller.SetSliderValue(Xbox360Slider.RightTrigger, byte.MinValue);
            }

            if (_rightMouseDown)
            {
                _controller.SetSliderValue(Xbox360Slider.LeftTrigger, byte.MaxValue);
            }
            else if (!_rightMouseDown)
            {
                _controller.SetSliderValue(Xbox360Slider.LeftTrigger, byte.MinValue);
            }

        }
        void CheckForAxisResetY()
        {
            if (_lastDirectionY == DirectionStateY.None)
            {
                _controller.SetAxisValue(currAxisMoveY, 0);
            }
            else if (_keyDown && _keyUp)
            {
                _controller.SetAxisValue(currAxisMoveY, 0);
            }
            else
            {
                if (_lastDirectionY == DirectionStateY.Up)
                {
                    _controller.SetAxisValue(currAxisMoveY, short.MaxValue);
                }
                else if (_lastDirectionY == DirectionStateY.Down)
                {
                    _controller.SetAxisValue(currAxisMoveY, short.MinValue);
                }
            }
        }


        void CheckForAxisResetX()
        {
                if (_lastDirectionX == DirectionStateX.None)
                {
                    _controller.SetAxisValue(currAxisMoveX, 0);
                }
                else if (_keyLeft && _keyRight)
                {
                    _controller.SetAxisValue(currAxisMoveX, 0);
                }
                else
                {
                    if (_lastDirectionX == DirectionStateX.Right)
                    {
                        _controller.SetAxisValue(currAxisMoveX, short.MaxValue);
                    }
                    else if (_lastDirectionX == DirectionStateX.Left)
                    {
                        _controller.SetAxisValue(currAxisMoveX, short.MinValue);
                    }
                }
        }
        

        public void CheckForMouseInactivity()
        {
            while (true)
            {
                if (((DateTime.Now - _lastMouseMoveTime).TotalMilliseconds > mouseResetDelay))
                {
                    _controller.SetAxisValue(currAxisMouseX, 0);
                    if (!_leftMouseDown)
                    {
                        _controller.SetAxisValue(currAxisMouseY, 0);
                    }
                }
                Task.Delay(1).Wait();
            }
        }

        public void Cleanup()
        {
            interception_destroy_context(_interceptionContext);
            _controller.Disconnect();
            _client.Dispose();
        }

        public void SetXSensitivity(int value)
        {
            sensX = value;
        }
        public void SetYSensitivity(int value)
        {
            sensY = value;
        }
        public void SetExponentialFactor(double value)
        {
            expFactor = value;
        }
        public void SetScaleFactor(double value)
        {
            scaleFactor = value;
        }
        public void SwitchLockedState()
        {
            ReleaseInterceptedControllerInputs();
            bool wasLocked = _locked;
            _locked = !_locked;
            
        }
        public void SwitchStreamProofState()
        {
            _isStreamProof = !_isStreamProof;
        }

        byte[] wUpByteArray = new byte[] { 17, 0, 1, 0, 0, 0, 0, 0, };
        byte[] sUpByteArray = new byte[] { 31, 0, 1, 0, 0, 0, 0, 0, };
        byte[] aUpByteArray = new byte[] { 30, 0, 1, 0, 0, 0, 0, 0, };
        byte[] dUpByteArray = new byte[] { 32, 0, 1, 0, 0, 0, 0, 0, };


        public void ReleaseInterceptedControllerInputs()
        {
            _keyLeft = false;
            _keyRight = false;
            _keyUp = false;
            _keyDown = false;
            _keySlide = false;
            _keyJump = false;
            _keyReload = false;
            _keyInteract = false;
            _keyAbility1 = false;

            _keySwapWeapons1 = false;
            _keySwapWeapons2 = false;
            _keyHolsterWeapons = false;

            _leftMouseDown = false;
            _rightMouseDown = false;

            _controller.SetButtonState(Xbox360Button.A, false);
            _controller.SetButtonState(Xbox360Button.B, false);
            _controller.SetButtonState(Xbox360Button.X, false);
            _controller.SetButtonState(Xbox360Button.Y, false);
            _controller.SetButtonState(Xbox360Button.LeftShoulder, false);
            _controller.SetButtonState(Xbox360Button.RightShoulder, false);
            _controller.SetSliderValue(Xbox360Slider.LeftTrigger, 0);
            _controller.SetSliderValue(Xbox360Slider.RightTrigger, 0);


            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
            _controller.SetAxisValue(Xbox360Axis.RightThumbX, 0);
            _controller.SetAxisValue(Xbox360Axis.RightThumbY, 0);


            interception_send(_interceptionContext, _keyboardDevice, wUpByteArray, 1);
            interception_send(_interceptionContext, _keyboardDevice, sUpByteArray, 1);
            interception_send(_interceptionContext, _keyboardDevice, aUpByteArray, 1);
            interception_send(_interceptionContext, _keyboardDevice, dUpByteArray, 1);
            _lastDirectionX = DirectionStateX.None;
            _lastDirectionY = DirectionStateY.None;
            
        }
        public void ConnectController()
        {
            _controller.Connect();
        }

        public void DisconnectController()
        {
            _controller.Disconnect();
        }

        private bool _isStreamProof = false;

        

        public void SaveConfiguration(string filePath)
        {
            StringBuilder configData = new StringBuilder();

            var scanCodeProperties = typeof(ScanCodes).GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (var property in scanCodeProperties)
            {
                var value = property.GetValue(null);
                configData.AppendLine($"{property.Name}={value}");
            }

            // bools
            configData.AppendLine($"_southpaw={_southpaw}");
            configData.AppendLine($"_streamProof={_isStreamProof}");

            //sens
            configData.AppendLine($"sensX={sensX}");
            configData.AppendLine($"sensY={sensY}");
            configData.AppendLine($"scaleFactor={scaleFactor}");
            configData.AppendLine($"expFactor={expFactor}");

            //toggle
            configData.AppendLine($"TOGGLE={ScanCodes.TOGGLE}");

            File.WriteAllText(filePath, configData.ToString());
        }
        public void LoadConfiguration(string filePath)
        {
            try
            {
                ConfigData configData = ConfigUtil.ReadConfigFile(filePath);

                foreach (var keyValuePair in configData.KeyBindings)
                {
                    var property = typeof(ScanCodes).GetProperty(keyValuePair.Key);

                    if (property != null && property.GetGetMethod().IsStatic)
                    {
                        object value = Convert.ChangeType(keyValuePair.Value, property.PropertyType);
                        property.SetValue(null, value);
                    }
                }

                //bools
                configData.BooleanSettings.TryGetValue("_southpaw", out _southpaw);

                //sens
                configData.SensitiviySettings.TryGetValue("sensX", out sensX);
                configData.SensitiviySettings.TryGetValue("sensY", out sensY);
                configData.SensitiviySettings.TryGetValue("scaleFactor", out scaleFactor);
                configData.SensitiviySettings.TryGetValue("expFactor", out expFactor);

                //toggle
                configData.TOGGLE = ScanCodes.TOGGLE;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration: {ex.Message}", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
