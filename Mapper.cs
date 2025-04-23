using Nefarius.ViGEm.Client.Targets.Xbox360;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets.DualShock4;
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
using System.Threading;


namespace zxmapper
{
    // Controller type enum
    public enum ControllerType
    {
        Xbox360,
        DualShock4,
        DualSense
    }

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
        public static int ENABLE { get; set; } = 0x3B; // F1
        public static int DISABLE { get; set; } = 0x3C; // F2
    }
    internal class Mapper
    {
        private ViGEmClient _client;
        private IXbox360Controller _xbox360Controller;
        private IDualShock4Controller _dualshock4Controller;
        private IVirtualGamepad _activeController;

        private ControllerType _controllerType = ControllerType.Xbox360;

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

        private bool _leftMouseDown = false;
        private bool _rightMouseDown = false;

        private DateTime _lastLeftClickTime = DateTime.Now;
        private DateTime _lastRightClickTime = DateTime.Now;
        private const int MOUSE_BUTTON_CHECK_INTERVAL = 100; // milliseconds

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
            Task.Run(() => CheckForStuckMouseButtons());
        }

        private void InitializeVigem()
        {
            _client = new ViGEmClient();

            switch (_controllerType)
            {
                case ControllerType.Xbox360:
                    _xbox360Controller = _client.CreateXbox360Controller();
                    _activeController = _xbox360Controller;
                    _xbox360Controller.Connect();
                    break;
                case ControllerType.DualShock4:
                case ControllerType.DualSense: // DualSense uses same API as DualShock4 in ViGEm
                    _dualshock4Controller = _client.CreateDualShock4Controller();
                    _activeController = _dualshock4Controller;
                    _dualshock4Controller.Connect();
                    break;
            }
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

                        double avgX = smoothedX.Average();
                        double avgY = smoothedY.Average();

                        avgX = Math.Max(Math.Min(avgX, capFactor), -capFactor);
                        avgY = Math.Max(Math.Min(avgY, capFactor), -capFactor);

                        short joystickX = (short)Math.Round(avgX);
                        short joystickY = (short)Math.Round(avgY);

                        joystickX = (short)Math.Max(Math.Min(joystickX, short.MaxValue), short.MinValue);
                        joystickY = (short)Math.Max(Math.Min(joystickY, short.MaxValue), short.MinValue);

                        if ((mouseStroke.state & (ushort)MouseState.LeftDown) != 0)
                        {
                            _leftMouseDown = true;
                            _lastLeftClickTime = DateTime.Now;
                        }
                        else if ((mouseStroke.state & (ushort)MouseState.LeftUp) != 0)
                        {
                            _leftMouseDown = false;
                        }
                        
                        if ((mouseStroke.state & (ushort)MouseState.RightDown) != 0)
                        {
                            _rightMouseDown = true;
                            _lastRightClickTime = DateTime.Now;
                        }
                        else if ((mouseStroke.state & (ushort)MouseState.RightUp) != 0)
                        {
                            _rightMouseDown = false;
                        }

                        if (_controllerType == ControllerType.Xbox360)
                        {
                            _xbox360Controller.SetAxisValue(currAxisMouseX, joystickX);
                            _xbox360Controller.SetAxisValue(currAxisMouseY, joystickY);
                            _xbox360Controller.SubmitReport();
                        }
                        else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
                        {
                            if (currAxisMouseX == Xbox360Axis.RightThumbX)
                            {
                                _dualshock4Controller.SetAxisValue(DualShock4Axis.RightThumbX, joystickX);
                            }
                            else if (currAxisMouseX == Xbox360Axis.LeftThumbX)
                            {
                                _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, joystickX);
                            }
                            
                            if (currAxisMouseY == Xbox360Axis.RightThumbY)
                            {
                                _dualshock4Controller.SetAxisValue(DualShock4Axis.RightThumbY, joystickY);
                            }
                            else if (currAxisMouseY == Xbox360Axis.LeftThumbY)
                            {
                                _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, joystickY);
                            }
                            
                            _dualshock4Controller.SubmitReport();
                        }

                        UpdateMouseState();

                        if (!_locked)
                        {
                            interception_send(_interceptionContext, device, ref stroke, 1);
                            continue;
                        }
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
                                _xbox360Controller.SetButtonState(Xbox360Button.Y, false);
                            }
                        }

                        // Handle F1 key (Enable)
                        if (_currKeystroke.code == ScanCodes.ENABLE && (_currKeystroke.state == 1 || _currKeystroke.state == 2))
                        {
                            if (!_locked)
                            {
                                SwitchLockedState();
                                SetFilters();
                            }
                            // Let the F1 key pass through
                            interception_send(_interceptionContext, device, ref stroke, 1);
                            continue;
                        }
                        
                        // Handle F2 key (Disable)
                        if (_currKeystroke.code == ScanCodes.DISABLE && (_currKeystroke.state == 1 || _currKeystroke.state == 2))
                        {
                            if (_locked)
                            {
                                SwitchLockedState();
                                SetFilters();
                            }
                            // Let the F2 key pass through
                            interception_send(_interceptionContext, device, ref stroke, 1);
                            continue;
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
                _xbox360Controller.SetButtonState(button, true);
                await Task.Delay(delayedKeyPressTime);
                _xbox360Controller.SetButtonState(button, false);
                _isDelayedResetInProgress = false;
            }
        }

        private void UpdateControllerState()
        {
            if (_keyCtrl && _keyAlt && _keyDelete)
            {
                SwitchLockedState();
                SetFilters();
                return;
            }
            
            if (_controllerType == ControllerType.Xbox360)
            {
                _xbox360Controller.SetButtonState(Xbox360Button.B, _keySlide);
                _xbox360Controller.SetButtonState(Xbox360Button.A, _keyJump);
                _xbox360Controller.SetButtonState(Xbox360Button.X, _keyReload || _keyInteract);
                _xbox360Controller.SetButtonState(Xbox360Button.Up, _keyShield);
                _xbox360Controller.SetButtonState(Xbox360Button.Guide, _keyInventory);
                _xbox360Controller.SetButtonState(Xbox360Button.LeftShoulder, _keyAbility1 || _keyUltimate);
                _xbox360Controller.SetButtonState(Xbox360Button.RightShoulder, _keyPing || _keyUltimate);
                _xbox360Controller.SetButtonState(Xbox360Button.Right, _keyGrenade);
                _xbox360Controller.SetButtonState(Xbox360Button.LeftThumb, _keyZoom);

                performDelayedKeyPress(Xbox360Button.Y, _keyHolsterWeapons);
                if (_keySwapWeapons1 || _keySwapWeapons2)
                {
                    _xbox360Controller.SetButtonState(Xbox360Button.Y, true);
                }
                
                // Submit report with all changes
                _xbox360Controller.SubmitReport();
            }
            else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
            {
                // Map the Xbox controls to DualShock4 controls
                _dualshock4Controller.SetButtonState(DualShock4Button.Circle, _keySlide);
                _dualshock4Controller.SetButtonState(DualShock4Button.Cross, _keyJump);
                _dualshock4Controller.SetButtonState(DualShock4Button.Square, _keyReload || _keyInteract);
                _dualshock4Controller.SetButtonState(DualShock4Button.DpadUp, _keyShield);
                _dualshock4Controller.SetButtonState(DualShock4Button.Ps, _keyInventory);
                _dualshock4Controller.SetButtonState(DualShock4Button.L1, _keyAbility1 || _keyUltimate);
                _dualshock4Controller.SetButtonState(DualShock4Button.R1, _keyPing || _keyUltimate);
                _dualshock4Controller.SetButtonState(DualShock4Button.DpadRight, _keyGrenade);
                _dualshock4Controller.SetButtonState(DualShock4Button.ThumbLeft, _keyZoom);

                // Handle holster weapon
                if (_keyHolsterWeapons)
                {
                    _dualshock4Controller.SetButtonState(DualShock4Button.Triangle, true);
                    // We'll need to handle delayed press differently for DualShock
                    Task.Delay(delayedKeyPressTime).ContinueWith(_ => {
                        _dualshock4Controller.SetButtonState(DualShock4Button.Triangle, false);
                        _dualshock4Controller.SubmitReport();
                    });
                }
                else if (_keySwapWeapons1 || _keySwapWeapons2)
                {
                    _dualshock4Controller.SetButtonState(DualShock4Button.Triangle, true);
                }
                else 
                {
                    _dualshock4Controller.SetButtonState(DualShock4Button.Triangle, false);
                }
                
                // Submit report with all changes
                _dualshock4Controller.SubmitReport();
            }
        }
        void UpdateMouseState()
        {
            if (_controllerType == ControllerType.Xbox360)
            {
                if (_leftMouseDown)
                {
                    _xbox360Controller.SetSliderValue(Xbox360Slider.RightTrigger, byte.MaxValue);
                }
                else if (!_leftMouseDown)
                {
                    _xbox360Controller.SetSliderValue(Xbox360Slider.RightTrigger, byte.MinValue);
                }

                if (_rightMouseDown)
                {
                    _xbox360Controller.SetSliderValue(Xbox360Slider.LeftTrigger, byte.MaxValue);
                }
                else if (!_rightMouseDown)
                {
                    _xbox360Controller.SetSliderValue(Xbox360Slider.LeftTrigger, byte.MinValue);
                }

                _xbox360Controller.SubmitReport();
            }
            else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
            {
                if (_leftMouseDown)
                {
                    _dualshock4Controller.SetSliderValue(DualShock4Slider.RightTrigger, byte.MaxValue);
                }
                else if (!_leftMouseDown)
                {
                    _dualshock4Controller.SetSliderValue(DualShock4Slider.RightTrigger, byte.MinValue);
                }

                if (_rightMouseDown)
                {
                    _dualshock4Controller.SetSliderValue(DualShock4Slider.LeftTrigger, byte.MaxValue);
                }
                else if (!_rightMouseDown)
                {
                    _dualshock4Controller.SetSliderValue(DualShock4Slider.LeftTrigger, byte.MinValue);
                }

                _dualshock4Controller.SubmitReport();
            }
        }
        void CheckForAxisResetY()
        {
            if (_controllerType == ControllerType.Xbox360)
            {
                if (_lastDirectionY == DirectionStateY.None)
                {
                    _xbox360Controller.SetAxisValue(currAxisMoveY, 0);
                }
                else if (_keyDown && _keyUp)
                {
                    _xbox360Controller.SetAxisValue(currAxisMoveY, 0);
                }
                else
                {
                    if (_lastDirectionY == DirectionStateY.Up)
                    {
                        _xbox360Controller.SetAxisValue(currAxisMoveY, short.MaxValue);
                    }
                    else if (_lastDirectionY == DirectionStateY.Down)
                    {
                        _xbox360Controller.SetAxisValue(currAxisMoveY, short.MinValue);
                    }
                }
            }
            else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
            {
                if (_lastDirectionY == DirectionStateY.None)
                {
                    _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, 0);
                }
                else if (_keyDown && _keyUp)
                {
                    _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, 0);
                }
                else
                {
                    if (_lastDirectionY == DirectionStateY.Up)
                    {
                        _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, short.MaxValue);
                    }
                    else if (_lastDirectionY == DirectionStateY.Down)
                    {
                        _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, short.MinValue);
                    }
                }
            }
        }


        void CheckForAxisResetX()
        {
            if (_controllerType == ControllerType.Xbox360)
            {
                if (_lastDirectionX == DirectionStateX.None)
                {
                    _xbox360Controller.SetAxisValue(currAxisMoveX, 0);
                }
                else if (_keyLeft && _keyRight)
                {
                    _xbox360Controller.SetAxisValue(currAxisMoveX, 0);
                }
                else
                {
                    if (_lastDirectionX == DirectionStateX.Right)
                    {
                        _xbox360Controller.SetAxisValue(currAxisMoveX, short.MaxValue);
                    }
                    else if (_lastDirectionX == DirectionStateX.Left)
                    {
                        _xbox360Controller.SetAxisValue(currAxisMoveX, short.MinValue);
                    }
                }
            }
            else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
            {
                if (_lastDirectionX == DirectionStateX.None)
                {
                    _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, 0);
                }
                else if (_keyLeft && _keyRight)
                {
                    _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, 0);
                }
                else
                {
                    if (_lastDirectionX == DirectionStateX.Right)
                    {
                        _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, short.MaxValue);
                    }
                    else if (_lastDirectionX == DirectionStateX.Left)
                    {
                        _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, short.MinValue);
                    }
                }
            }
        }
        

        public void CheckForMouseInactivity()
        {
            while (true)
            {
                if (((DateTime.Now - _lastMouseMoveTime).TotalMilliseconds > mouseResetDelay))
                {
                    if (_controllerType == ControllerType.Xbox360)
                    {
                        _xbox360Controller.SetAxisValue(currAxisMouseX, 0);
                        if (!_leftMouseDown)
                        {
                            _xbox360Controller.SetAxisValue(currAxisMouseY, 0);
                        }
                        _xbox360Controller.SubmitReport();
                    }
                    else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
                    {
                        // Map Xbox axis to DualShock axis for inactivity reset
                        if (currAxisMouseX == Xbox360Axis.RightThumbX)
                        {
                            _dualshock4Controller.SetAxisValue(DualShock4Axis.RightThumbX, 0);
                        }
                        else if (currAxisMouseX == Xbox360Axis.LeftThumbX)
                        {
                            _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, 0);
                        }
                        
                        if (!_leftMouseDown)
                        {
                            if (currAxisMouseY == Xbox360Axis.RightThumbY)
                            {
                                _dualshock4Controller.SetAxisValue(DualShock4Axis.RightThumbY, 0);
                            }
                            else if (currAxisMouseY == Xbox360Axis.LeftThumbY)
                            {
                                _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, 0);
                            }
                        }
                        
                        _dualshock4Controller.SubmitReport();
                    }
                }
                await Task.Delay(mouseResetDelay);
            }
        }

        public void Cleanup()
        {
            interception_destroy_context(_interceptionContext);
            DisconnectController();
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
            _keyUltimate = false;
            _keyShield = false;
            _keySwapWeapons1 = false;
            _keySwapWeapons2 = false;
            _keyHolsterWeapons = false;
            _keyPing = false;
            _keyGrenade = false;
            _keyZoom = false;
            _keyMap = false;
            _keyInventory = false;
            _leftMouseDown = false;
            _rightMouseDown = false;

            if (_controllerType == ControllerType.Xbox360)
            {
                // Reset Xbox buttons
                _xbox360Controller.SetButtonState(Xbox360Button.A, false);
                _xbox360Controller.SetButtonState(Xbox360Button.B, false);
                _xbox360Controller.SetButtonState(Xbox360Button.X, false);
                _xbox360Controller.SetButtonState(Xbox360Button.Y, false);
                _xbox360Controller.SetButtonState(Xbox360Button.LeftShoulder, false);
                _xbox360Controller.SetButtonState(Xbox360Button.RightShoulder, false);
                _xbox360Controller.SetSliderValue(Xbox360Slider.LeftTrigger, 0);
                _xbox360Controller.SetSliderValue(Xbox360Slider.RightTrigger, 0);

                // Reset Xbox axes
                _xbox360Controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                _xbox360Controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                _xbox360Controller.SetAxisValue(Xbox360Axis.RightThumbX, 0);
                _xbox360Controller.SetAxisValue(Xbox360Axis.RightThumbY, 0);
                
                _xbox360Controller.SubmitReport();
            }
            else if (_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense)
            {
                // Reset DualShock buttons
                _dualshock4Controller.SetButtonState(DualShock4Button.Cross, false);
                _dualshock4Controller.SetButtonState(DualShock4Button.Circle, false);
                _dualshock4Controller.SetButtonState(DualShock4Button.Square, false);
                _dualshock4Controller.SetButtonState(DualShock4Button.Triangle, false);
                _dualshock4Controller.SetButtonState(DualShock4Button.L1, false);
                _dualshock4Controller.SetButtonState(DualShock4Button.R1, false);
                _dualshock4Controller.SetSliderValue(DualShock4Slider.LeftTrigger, 0);
                _dualshock4Controller.SetSliderValue(DualShock4Slider.RightTrigger, 0);

                // Reset DualShock axes
                _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, 0);
                _dualshock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, 0);
                _dualshock4Controller.SetAxisValue(DualShock4Axis.RightThumbX, 0);
                _dualshock4Controller.SetAxisValue(DualShock4Axis.RightThumbY, 0);
                
                _dualshock4Controller.SubmitReport();
            }
        }
        public void ConnectController()
        {
            if (_controllerType == ControllerType.Xbox360 && _xbox360Controller != null)
            {
                _xbox360Controller.Connect();
            }
            else if ((_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense) && _dualshock4Controller != null)
            {
                _dualshock4Controller.Connect();
            }
        }

        public void DisconnectController()
        {
            if (_controllerType == ControllerType.Xbox360 && _xbox360Controller != null)
            {
                _xbox360Controller.Disconnect();
            }
            else if ((_controllerType == ControllerType.DualShock4 || _controllerType == ControllerType.DualSense) && _dualshock4Controller != null)
            {
                _dualshock4Controller.Disconnect();
            }
        }

        private bool _isStreamProof = false;

        

        public void SaveConfiguration(string filePath)
        {
            try
            {
                StringBuilder config = new StringBuilder();
                foreach (var property in typeof(ScanCodes).GetProperties(BindingFlags.Public | BindingFlags.Static))
                {
                    var value = property.GetValue(null);
                    config.AppendLine($"{property.Name}={value}");
                }

                // Boolean settings
                config.AppendLine($"_southpaw={_southpaw}");
                config.AppendLine($"_streamProof={_isStreamProof}");
                
                // Controller type setting
                config.AppendLine($"_controllerType={(int)_controllerType}");

                // Sensitivity settings
                config.AppendLine($"sensX={sensX}");
                config.AppendLine($"sensY={sensY}");
                config.AppendLine($"scaleFactor={scaleFactor}");
                config.AppendLine($"expFactor={expFactor}");
                
                // Save custom keybinds
                foreach (Action action in Enum.GetValues(typeof(Action)))
                {
                    int actionValue = 0;
                    // Check if this action has a mapping in Form1's keyMappings dictionary
                    // This is a bit of a hack since we don't have direct access to Form1's keyMappings
                    if (action == Action.EnableMapping)
                    {
                        actionValue = ScanCodes.ENABLE;
                    }
                    else if (action == Action.DisableMapping)
                    {
                        actionValue = ScanCodes.DISABLE;
                    }
                    else
                    {
                        // Try to map from ScanCodes
                        switch (action)
                        {
                            case Action.Up: actionValue = ScanCodes.FORWARD; break;
                            case Action.Down: actionValue = ScanCodes.BACKWARDS; break;
                            case Action.Left: actionValue = ScanCodes.LEFT; break;
                            case Action.Right: actionValue = ScanCodes.RIGHT; break;
                            case Action.Jump: actionValue = ScanCodes.JUMP; break;
                            case Action.Slide: actionValue = ScanCodes.SLIDE; break;
                            case Action.Reload: actionValue = ScanCodes.RELOAD; break;
                            case Action.Interact: actionValue = ScanCodes.INTERACT; break;
                            case Action.Ability1: actionValue = ScanCodes.ABILITY1; break;
                            case Action.Ultimate: actionValue = ScanCodes.ULTIMATE; break;
                            case Action.Shield: actionValue = ScanCodes.SHIELD; break;
                            case Action.Grenade: actionValue = ScanCodes.GRENADE; break;
                            case Action.Ping: actionValue = ScanCodes.PING; break;
                        }
                    }
                    
                    config.AppendLine($"ACTION_{action}={actionValue}");
                }

                File.WriteAllText(filePath, config.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }
        public void LoadConfiguration(string filePath)
        {
            try
            {
                ConfigData configData = ConfigUtil.ReadConfigFile(filePath);
                
                // Load all ScanCode properties
                foreach (var keyValuePair in configData.KeyBindings)
                {
                    var property = typeof(ScanCodes).GetProperty(keyValuePair.Key);
                    
                    if (property != null && property.GetGetMethod().IsStatic)
                    {
                        object value = Convert.ChangeType((int)keyValuePair.Value, property.PropertyType);
                        property.SetValue(null, value);
                    }
                }

                // Load boolean settings
                _southpaw = configData.BooleanSettings["_southpaw"];
                _isStreamProof = configData.BooleanSettings["_streamProof"];
                
                // Load controller type setting
                if (configData.BooleanSettings.ContainsKey("_controllerType"))
                {
                    if (bool.TryParse(configData.BooleanSettings["_controllerType"].ToString(), out bool boolValue))
                    {
                        int controllerTypeValue = boolValue ? 1 : 0;
                        _controllerType = (ControllerType)controllerTypeValue;
                    }
                    else if (int.TryParse(configData.BooleanSettings["_controllerType"].ToString(), out int controllerTypeValue))
                    {
                        if (Enum.IsDefined(typeof(ControllerType), controllerTypeValue))
                        {
                            _controllerType = (ControllerType)controllerTypeValue;
                        }
                    }
                }

                // Load sensitivity settings
                sensX = configData.SensitiviySettings["sensX"];
                sensY = configData.SensitiviySettings["sensY"];
                expFactor = configData.SensitiviySettings["expFactor"];
                scaleFactor = configData.SensitiviySettings["scaleFactor"];
                
                // Set toggle
                ScanCodes.TOGGLE = configData.TOGGLE;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading configuration: {ex.Message}");
            }
        }

        private async void CheckForStuckMouseButtons()
        {
            while (true)
            {
                await Task.Delay(MOUSE_BUTTON_CHECK_INTERVAL);
                
                if (_locked)
                {
                    // Check for stuck left mouse button
                    if (_leftMouseDown && (DateTime.Now - _lastLeftClickTime).TotalMilliseconds > 5000)
                    {
                        _leftMouseDown = false;
                        _xbox360Controller.SetButtonState(Xbox360Button.RightShoulder, false);
                        _xbox360Controller.SubmitReport();
                        Debug.WriteLine("Force released stuck left mouse button");
                    }
                    
                    // Check for stuck right mouse button
                    if (_rightMouseDown && (DateTime.Now - _lastRightClickTime).TotalMilliseconds > 5000)
                    {
                        _rightMouseDown = false;
                        _xbox360Controller.SetButtonState(Xbox360Button.LeftShoulder, false);
                        _xbox360Controller.SubmitReport();
                        Debug.WriteLine("Force released stuck right mouse button");
                    }
                }
            }
        }

        // Set the controller type and reinitialize if necessary
        public void SetControllerType(ControllerType type)
        {
            if (_controllerType != type)
            {
                // Disconnect the existing controller if connected
                DisconnectController();

                _controllerType = type;
                
                // Reinitialize with the new controller type
                InitializeVigem();
            }
        }

        public ControllerType GetControllerType()
        {
            return _controllerType;
        }
    }
}
