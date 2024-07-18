using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using InputInterceptorNS;
using System.Threading;
using System.Drawing;
using System.Text;
using System.IO;
using System.Net;
using System.Management;


namespace zxmapper
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowDisplayAffinity")]
        public static extern bool SetDisplayAffinity(IntPtr hwnd, uint dwAffinity);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowW(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private Mapper mapper;

        private bool _streamProof = false;
        private bool _waitingForKeyPress = false;
        private bool resumeCheckBehaviour = false;

        private Button targetButton = null;
        private Dictionary<Action, int> keyMappings;
        private Dictionary<Button, Action> buttonToActionMap;

        private Action? pendingActionUpdate = null;


        public Form1()
        {

            InitializeComponent();
            this.Text = "zxmapper";
            this.Load += new EventHandler(FormLoad);
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            SubscribeButtonClickEvents();
            InitializeKeyMappings();
            InitializeButtonToActionMapping();
        }


        /*
         * This method initializes the driver and returns a boolean value
         */
        private Boolean InitializeInterceptionDriver()
        {
            if (InputInterceptor.CheckDriverInstalled())
            {
                if (InputInterceptor.Initialize())
                {
                    driverText.Text = "initializing...";
                    return true;
                }
            }
            return false;
        }

        /*
         * If the driver is not installed, it will attempt to install it
         */
        private void InstallInterceptionDriver()
        {
            if (InputInterceptor.CheckAdministratorRights())
            {
                driverText.Text = "Installing driver...";
                if (InputInterceptor.InstallDriver())
                {
                    //nothing for now
                }
                else
                {
                    driverText.Text = "Error initializing";
                }
            }
            else
            {
                driverText.Text = "Restart exe with admin";
            }
        }

        private bool InitializeVigemDriver()
        {
            string query = "SELECT * FROM Win32_Product WHERE Name = 'Nefarius Virtual Gamepad Emulation Bus Driver'";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection results = searcher.Get())
            {
                return results.Count > 0;
            }
        }

        private void InstallVigemDriver()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string msiPath = System.IO.Path.Combine(currentDirectory, "ViGEm.msi");
            string logPath = System.IO.Path.Combine(currentDirectory, "zxmapper.log");
            string arguments = $"/I \"{msiPath}\" /QN /L*V \"{logPath}\"";

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "msiexec";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    string errorMessage = $"Installer exited with code: {process.ExitCode}\nCheck the log file at {logPath} for more details.";
                    MessageBox.Show(errorMessage, "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    driverText.Text = "ViGEm Bus Driver installed";
                    Task.Delay(1000);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormLoad(object sender, EventArgs e)
        {
            this.Show();
            if (!InitializeDrivers())
            {
                driverText.Text = "initializing...";

                panel1.Visible = false;
                mapper = new Mapper();
                UpdateUIFromConfig(ConfigUtil.ReadConfigFile("config.zxm"));
                toggleDropDown.SelectedIndex = 0;
            }
        }

        private bool InitializeDrivers()
        {
            bool isRestartRequired = false;
            if (!InitializeInterceptionDriver())
            {
                InstallInterceptionDriver();
                isRestartRequired = true;
            }
            if (!InitializeVigemDriver())
            {
                InstallVigemDriver();
                isRestartRequired = true;
            }

            if (isRestartRequired)
            {
                driverText.Text = "Restart PC!";
            }
            return isRestartRequired;
        }

        private async void applyButton_Click_1(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "config.zxm");
            RefreshKeyMappings();
            //mapper.SwitchLockedState();
            //mapper.SetFilters();
            mapper.SetXSensitivity((int)sensBoxX.Value);
            mapper.SetYSensitivity((int)sensBoxY.Value);
            mapper.SetExponentialFactor((double)expFactorBox.Value);
            mapper.SetScaleFactor((double)scaleFactorBox.Value);
            mapper.SaveConfiguration(filePath);
            applyLabel.Text = "Applied Settings";
            await Task.Delay(2000);
            applyLabel.Text = "";
        }

        private void RefreshKeyMappings()
        {
            /* These key mappings can be used for abilities
             * Since Apex permits multiple simultaneous inputs, we can just pass the keyboard keys for non movement keys
             * Uncomment this code for games that do not permit multiple simultaneous inputs (warzone, xdefiant)
            
            ScanCodes.ABILITY1 = keyMappings[Action.Ability1];
            ScanCodes.ULTIMATE = keyMappings[Action.Ultimate];
            ScanCodes.JUMP = keyMappings[Action.Jump];
            ScanCodes.SLIDE = keyMappings[Action.Slide];
            ScanCodes.RELOAD = keyMappings[Action.Reload];
            ScanCodes.SHIELD = keyMappings[Action.Shield];
            ScanCodes.INSPECT = keyMappings[Action.Inspect];
            */


        ScanCodes.FORWARD = keyMappings[Action.Up];
            ScanCodes.BACKWARDS = keyMappings[Action.Down];
            ScanCodes.LEFT = keyMappings[Action.Left];
            ScanCodes.RIGHT = keyMappings[Action.Right];

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (mapper != null)
            {
                mapper.Cleanup();
            }
        }

        private void streamProof_CheckedChanged(object sender, EventArgs e)
        {
            _streamProof = !_streamProof;
            mapper.SwitchStreamProofState();
            IntPtr hwnd = FindWindowW(null, "zxmapper");

            if (_streamProof)
            {
                SetDisplayAffinity(hwnd, 0x00000011);
            }
            else
            {
                SetDisplayAffinity(hwnd, 0x00000000);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (resumeCheckBehaviour)
            {
                mapper.SwitchSouthPawState();
                mapper.SetMapperAxis();
            }
        }

        private void tacAbilityButton_Click(object sender, EventArgs e)
        {
            _waitingForKeyPress = true;
        }

        private void InitializeKeyMappings()
        {
            keyMappings = new Dictionary<Action, int>
            {
                { Action.Up, ScanCodes.FORWARD },
                { Action.Down, ScanCodes.BACKWARDS },
                { Action.Left, ScanCodes.LEFT },
                { Action.Right, ScanCodes.RELOAD },
                { Action.Slide, ScanCodes.SLIDE },
                { Action.Jump, ScanCodes.JUMP},
                { Action.Interact, ScanCodes.INTERACT },
                { Action.Reload, ScanCodes.RELOAD },
                { Action.Ability1, ScanCodes.ABILITY1 },
                { Action.Ultimate, ScanCodes.ULTIMATE },
                { Action.Shield, ScanCodes.SHIELD },
                { Action.Inspect, ScanCodes.INSPECT },
                { Action.Ping, ScanCodes.PING },
                { Action.Grenade, ScanCodes.GRENADE },
            };
        }

        private void InitializeButtonToActionMapping()
        {
            buttonToActionMap = new Dictionary<Button, Action>
            {
                { b_backwards, Action.Down },
                { b_left, Action.Left },
                { b_right, Action.Right },
                { b_forward, Action.Up }
            };
        }

        private void SubscribeButtonClickEvents()
        {
            b_forward.Click += Button_Click;
            b_backwards.Click += Button_Click;
            b_left.Click += Button_Click;
            b_right.Click += Button_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button buttonClicked && buttonToActionMap.TryGetValue(buttonClicked, out Action action))
            {
                _waitingForKeyPress = true;
                targetButton = buttonClicked;
                pendingActionUpdate = action;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (pendingActionUpdate.HasValue)
            {
                uint scanCode = MapVirtualKey((uint)e.KeyCode, 0);

                keyMappings[pendingActionUpdate.Value] = (int)scanCode;

                if (targetButton != null)
                {
                    targetButton.Text = e.KeyCode.ToString();

                    targetButton = null;
                }

                pendingActionUpdate = null;
                e.Handled = true;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_waitingForKeyPress && targetButton != null)
            {
                string buttonText = "";
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        buttonText = "Left";
                        break;
                    case MouseButtons.Right:
                        buttonText = "Right";
                        break;
                    case MouseButtons.Middle:
                        buttonText = "Middle";
                        break;
                    case MouseButtons.XButton1:
                        buttonText = "XButton1";
                        break;
                    case MouseButtons.XButton2:
                        buttonText = "XButton2";
                        break;
                }

                targetButton.Text = buttonText;
                _waitingForKeyPress = false;
                targetButton = null;
            }
        }

        private void UpdateUIFromConfig(ConfigData configData)
        {
            b_forward.Text = GetKeyNameFromScanCode((int)configData.KeyBindings["FORWARD"]);
            b_backwards.Text = GetKeyNameFromScanCode((int)configData.KeyBindings["BACKWARDS"]);
            b_left.Text = GetKeyNameFromScanCode((int)configData.KeyBindings["LEFT"]);
            b_right.Text = GetKeyNameFromScanCode((int)configData.KeyBindings["RIGHT"]);

            southpaw.Checked = configData.BooleanSettings["_southpaw"];
            streamProof.Checked = configData.BooleanSettings["_streamProof"];
            resumeCheckBehaviour = true;

            sensBoxX.Value = (decimal)configData.SensitiviySettings["sensX"];
            sensBoxY.Value = (decimal)configData.SensitiviySettings["sensY"];
            expFactorBox.Value = (decimal)configData.SensitiviySettings["expFactor"];
            scaleFactorBox.Value = (decimal)configData.SensitiviySettings["scaleFactor"];
        }

        private string GetKeyNameFromScanCode(int scanCode)
        {
            uint virtualKeyCode = MapVirtualKey((uint)scanCode, 1);
            if (virtualKeyCode != 0)
            {
                if (Enum.IsDefined(typeof(Keys), (int)virtualKeyCode))
                {
                    Keys key = (Keys)virtualKeyCode;
                    return key.ToString();
                }
            }
            return "Unknown";
        }

        private void toggleDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateToggleButton();
        }

        private void UpdateToggleButton()
        {
            String selected = toggleDropDown.SelectedItem.ToString();
            if (selected.StartsWith("MOUSE"))
            {
                mapper.SetContextToggle(true);
                if (selected.EndsWith("X5"))
                {
                    ScanCodes.TOGGLE = (int)ScanCodes.XBUTTON5;
                }
            }
        }

        private void taskbarHide_CheckedChanged(object sender, EventArgs e)
        {
            Form1.ActiveForm.ShowInTaskbar = !taskbarHide.Checked;
        }
    }
}
