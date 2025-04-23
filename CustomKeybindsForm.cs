using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zxmapper
{
    public partial class CustomKeybindsForm : Form
    {
        private Mapper _mapper;
        private Dictionary<Action, int> _keyMappings;
        private Button _currentButton;
        private bool _waitingForKey;
        
        public bool SettingsChanged { get; private set; }

        private Dictionary<Button, Action> buttonActionMap;

        public CustomKeybindsForm(Mapper mapper, Dictionary<Action, int> keyMappings)
        {
            InitializeComponent();
            _mapper = mapper;
            _keyMappings = keyMappings;
            SettingsChanged = false;
            
            // Initialize button mappings
            InitializeButtonActionMap();
            
            // Update the button labels to show current key bindings
            UpdateButtonLabels();
        }

        private void InitializeButtonActionMap()
        {
            buttonActionMap = new Dictionary<Button, Action>();
            
            // Initialize all buttons and their corresponding actions
            buttonActionMap[jumpButton] = Action.Jump;
            buttonActionMap[crouchButton] = Action.Crouch;
            buttonActionMap[slideButton] = Action.Slide;
            buttonActionMap[reloadButton] = Action.Reload;
            buttonActionMap[interactButton] = Action.Interact;
            buttonActionMap[weaponSwap1Button] = Action.SwapWeapons1;
            buttonActionMap[weaponSwap2Button] = Action.SwapWeapons2;
            buttonActionMap[tacticalButton] = Action.Tactical;
            buttonActionMap[ultimateButton] = Action.Ultimate;
            buttonActionMap[grenadeButton] = Action.Grenade;
            buttonActionMap[sprintButton] = Action.Sprint;
            buttonActionMap[meleeButton] = Action.Melee;
            buttonActionMap[pingButton] = Action.Ping;
            buttonActionMap[pingWheelButton] = Action.PingWheel;
            buttonActionMap[inventoryButton] = Action.Inventory;
            buttonActionMap[mapButton] = Action.Map;
            buttonActionMap[enableButton] = Action.EnableMapping;
            buttonActionMap[disableButton] = Action.DisableMapping;
            buttonActionMap[custom1Button] = Action.Custom1;
            buttonActionMap[custom2Button] = Action.Custom2;
            buttonActionMap[custom3Button] = Action.Custom3;
            buttonActionMap[custom4Button] = Action.Custom4;
            buttonActionMap[custom5Button] = Action.Custom5;
            
            // Subscribe click event for all buttons
            foreach (var button in buttonActionMap.Keys)
            {
                button.Click += KeybindButton_Click;
            }
        }

        private void UpdateButtonLabels()
        {
            foreach (var pair in buttonActionMap)
            {
                Button button = pair.Key;
                Action action = pair.Value;
                
                if (_keyMappings.ContainsKey(action))
                {
                    button.Text = $"{action}: {GetKeyNameFromScanCode(_keyMappings[action])}";
                }
                else
                {
                    button.Text = $"{action}: Not Set";
                }
            }
        }

        private string GetKeyNameFromScanCode(int scanCode)
        {
            // This is adapted from the original code
            Keys key = (Keys)scanCode;
            return key.ToString();
        }

        private void KeybindButton_Click(object sender, EventArgs e)
        {
            if (_waitingForKey)
            {
                // Cancel previous binding operation
                _currentButton.BackColor = SystemColors.Control;
                statusLabel.Text = "Binding canceled.";
            }

            _currentButton = (Button)sender;
            _currentButton.BackColor = Color.LightBlue;
            _waitingForKey = true;
            statusLabel.Text = $"Press any key to bind to {buttonActionMap[_currentButton]}...";
            this.KeyPreview = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (_waitingForKey)
            {
                Action action = buttonActionMap[_currentButton];
                int scanCode = (int)e.KeyCode;
                
                // Update the mapping
                if (_keyMappings.ContainsKey(action))
                {
                    _keyMappings[action] = scanCode;
                }
                else
                {
                    _keyMappings.Add(action, scanCode);
                }
                
                // Update UI
                _currentButton.BackColor = SystemColors.Control;
                _currentButton.Text = $"{action}: {e.KeyCode}";
                statusLabel.Text = $"Bound {e.KeyCode} to {action}";
                
                _waitingForKey = false;
                SettingsChanged = true;
                
                // If binding F1 or F2, update the ScanCodes
                if (action == Action.EnableMapping)
                {
                    ScanCodes.ENABLE = scanCode;
                }
                else if (action == Action.DisableMapping)
                {
                    ScanCodes.DISABLE = scanCode;
                }
                
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (SettingsChanged)
            {
                // Save to configuration
                string filePath = System.IO.Path.Combine(
                    System.IO.Directory.GetCurrentDirectory(), "config.zxm");
                
                _mapper.SaveConfiguration(filePath);
                statusLabel.Text = "Settings saved.";
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            SettingsChanged = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.jumpButton = new System.Windows.Forms.Button();
            this.crouchButton = new System.Windows.Forms.Button();
            this.slideButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.interactButton = new System.Windows.Forms.Button();
            this.weaponSwap1Button = new System.Windows.Forms.Button();
            this.weaponSwap2Button = new System.Windows.Forms.Button();
            this.tacticalButton = new System.Windows.Forms.Button();
            this.ultimateButton = new System.Windows.Forms.Button();
            this.grenadeButton = new System.Windows.Forms.Button();
            this.sprintButton = new System.Windows.Forms.Button();
            this.meleeButton = new System.Windows.Forms.Button();
            this.pingButton = new System.Windows.Forms.Button();
            this.pingWheelButton = new System.Windows.Forms.Button();
            this.inventoryButton = new System.Windows.Forms.Button();
            this.mapButton = new System.Windows.Forms.Button();
            this.enableButton = new System.Windows.Forms.Button();
            this.disableButton = new System.Windows.Forms.Button();
            this.custom1Button = new System.Windows.Forms.Button();
            this.custom2Button = new System.Windows.Forms.Button();
            this.custom3Button = new System.Windows.Forms.Button();
            this.custom4Button = new System.Windows.Forms.Button();
            this.custom5Button = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // jumpButton
            this.jumpButton.Location = new System.Drawing.Point(12, 12);
            this.jumpButton.Name = "jumpButton";
            this.jumpButton.Size = new System.Drawing.Size(180, 30);
            this.jumpButton.TabIndex = 0;
            this.jumpButton.Text = "Jump: Not Set";
            this.jumpButton.UseVisualStyleBackColor = true;
            
            // crouchButton
            this.crouchButton.Location = new System.Drawing.Point(12, 48);
            this.crouchButton.Name = "crouchButton";
            this.crouchButton.Size = new System.Drawing.Size(180, 30);
            this.crouchButton.TabIndex = 1;
            this.crouchButton.Text = "Crouch: Not Set";
            this.crouchButton.UseVisualStyleBackColor = true;
            
            // slideButton
            this.slideButton.Location = new System.Drawing.Point(12, 84);
            this.slideButton.Name = "slideButton";
            this.slideButton.Size = new System.Drawing.Size(180, 30);
            this.slideButton.TabIndex = 2;
            this.slideButton.Text = "Slide: Not Set";
            this.slideButton.UseVisualStyleBackColor = true;
            
            // reloadButton
            this.reloadButton.Location = new System.Drawing.Point(12, 120);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(180, 30);
            this.reloadButton.TabIndex = 3;
            this.reloadButton.Text = "Reload: Not Set";
            this.reloadButton.UseVisualStyleBackColor = true;
            
            // interactButton
            this.interactButton.Location = new System.Drawing.Point(12, 156);
            this.interactButton.Name = "interactButton";
            this.interactButton.Size = new System.Drawing.Size(180, 30);
            this.interactButton.TabIndex = 4;
            this.interactButton.Text = "Interact: Not Set";
            this.interactButton.UseVisualStyleBackColor = true;
            
            // weaponSwap1Button
            this.weaponSwap1Button.Location = new System.Drawing.Point(12, 192);
            this.weaponSwap1Button.Name = "weaponSwap1Button";
            this.weaponSwap1Button.Size = new System.Drawing.Size(180, 30);
            this.weaponSwap1Button.TabIndex = 5;
            this.weaponSwap1Button.Text = "Swap Weapon 1: Not Set";
            this.weaponSwap1Button.UseVisualStyleBackColor = true;
            
            // weaponSwap2Button
            this.weaponSwap2Button.Location = new System.Drawing.Point(12, 228);
            this.weaponSwap2Button.Name = "weaponSwap2Button";
            this.weaponSwap2Button.Size = new System.Drawing.Size(180, 30);
            this.weaponSwap2Button.TabIndex = 6;
            this.weaponSwap2Button.Text = "Swap Weapon 2: Not Set";
            this.weaponSwap2Button.UseVisualStyleBackColor = true;
            
            // tacticalButton
            this.tacticalButton.Location = new System.Drawing.Point(12, 264);
            this.tacticalButton.Name = "tacticalButton";
            this.tacticalButton.Size = new System.Drawing.Size(180, 30);
            this.tacticalButton.TabIndex = 7;
            this.tacticalButton.Text = "Tactical: Not Set";
            this.tacticalButton.UseVisualStyleBackColor = true;
            
            // Second column
            
            // ultimateButton
            this.ultimateButton.Location = new System.Drawing.Point(212, 12);
            this.ultimateButton.Name = "ultimateButton";
            this.ultimateButton.Size = new System.Drawing.Size(180, 30);
            this.ultimateButton.TabIndex = 8;
            this.ultimateButton.Text = "Ultimate: Not Set";
            this.ultimateButton.UseVisualStyleBackColor = true;
            
            // grenadeButton
            this.grenadeButton.Location = new System.Drawing.Point(212, 48);
            this.grenadeButton.Name = "grenadeButton";
            this.grenadeButton.Size = new System.Drawing.Size(180, 30);
            this.grenadeButton.TabIndex = 9;
            this.grenadeButton.Text = "Grenade: Not Set";
            this.grenadeButton.UseVisualStyleBackColor = true;
            
            // sprintButton
            this.sprintButton.Location = new System.Drawing.Point(212, 84);
            this.sprintButton.Name = "sprintButton";
            this.sprintButton.Size = new System.Drawing.Size(180, 30);
            this.sprintButton.TabIndex = 10;
            this.sprintButton.Text = "Sprint: Not Set";
            this.sprintButton.UseVisualStyleBackColor = true;
            
            // meleeButton
            this.meleeButton.Location = new System.Drawing.Point(212, 120);
            this.meleeButton.Name = "meleeButton";
            this.meleeButton.Size = new System.Drawing.Size(180, 30);
            this.meleeButton.TabIndex = 11;
            this.meleeButton.Text = "Melee: Not Set";
            this.meleeButton.UseVisualStyleBackColor = true;
            
            // pingButton
            this.pingButton.Location = new System.Drawing.Point(212, 156);
            this.pingButton.Name = "pingButton";
            this.pingButton.Size = new System.Drawing.Size(180, 30);
            this.pingButton.TabIndex = 12;
            this.pingButton.Text = "Ping: Not Set";
            this.pingButton.UseVisualStyleBackColor = true;
            
            // pingWheelButton
            this.pingWheelButton.Location = new System.Drawing.Point(212, 192);
            this.pingWheelButton.Name = "pingWheelButton";
            this.pingWheelButton.Size = new System.Drawing.Size(180, 30);
            this.pingWheelButton.TabIndex = 13;
            this.pingWheelButton.Text = "Ping Wheel: Not Set";
            this.pingWheelButton.UseVisualStyleBackColor = true;
            
            // inventoryButton
            this.inventoryButton.Location = new System.Drawing.Point(212, 228);
            this.inventoryButton.Name = "inventoryButton";
            this.inventoryButton.Size = new System.Drawing.Size(180, 30);
            this.inventoryButton.TabIndex = 14;
            this.inventoryButton.Text = "Inventory: Not Set";
            this.inventoryButton.UseVisualStyleBackColor = true;
            
            // mapButton
            this.mapButton.Location = new System.Drawing.Point(212, 264);
            this.mapButton.Name = "mapButton";
            this.mapButton.Size = new System.Drawing.Size(180, 30);
            this.mapButton.TabIndex = 15;
            this.mapButton.Text = "Map: Not Set";
            this.mapButton.UseVisualStyleBackColor = true;
            
            // Third column
            
            // enableButton
            this.enableButton.Location = new System.Drawing.Point(412, 12);
            this.enableButton.Name = "enableButton";
            this.enableButton.Size = new System.Drawing.Size(180, 30);
            this.enableButton.TabIndex = 16;
            this.enableButton.Text = "Enable Mapping: F1";
            this.enableButton.UseVisualStyleBackColor = true;
            
            // disableButton
            this.disableButton.Location = new System.Drawing.Point(412, 48);
            this.disableButton.Name = "disableButton";
            this.disableButton.Size = new System.Drawing.Size(180, 30);
            this.disableButton.TabIndex = 17;
            this.disableButton.Text = "Disable Mapping: F2";
            this.disableButton.UseVisualStyleBackColor = true;
            
            // Custom keys
            
            // custom1Button
            this.custom1Button.Location = new System.Drawing.Point(412, 84);
            this.custom1Button.Name = "custom1Button";
            this.custom1Button.Size = new System.Drawing.Size(180, 30);
            this.custom1Button.TabIndex = 18;
            this.custom1Button.Text = "Custom 1: Not Set";
            this.custom1Button.UseVisualStyleBackColor = true;
            
            // custom2Button
            this.custom2Button.Location = new System.Drawing.Point(412, 120);
            this.custom2Button.Name = "custom2Button";
            this.custom2Button.Size = new System.Drawing.Size(180, 30);
            this.custom2Button.TabIndex = 19;
            this.custom2Button.Text = "Custom 2: Not Set";
            this.custom2Button.UseVisualStyleBackColor = true;
            
            // custom3Button
            this.custom3Button.Location = new System.Drawing.Point(412, 156);
            this.custom3Button.Name = "custom3Button";
            this.custom3Button.Size = new System.Drawing.Size(180, 30);
            this.custom3Button.TabIndex = 20;
            this.custom3Button.Text = "Custom 3: Not Set";
            this.custom3Button.UseVisualStyleBackColor = true;
            
            // custom4Button
            this.custom4Button.Location = new System.Drawing.Point(412, 192);
            this.custom4Button.Name = "custom4Button";
            this.custom4Button.Size = new System.Drawing.Size(180, 30);
            this.custom4Button.TabIndex = 21;
            this.custom4Button.Text = "Custom 4: Not Set";
            this.custom4Button.UseVisualStyleBackColor = true;
            
            // custom5Button
            this.custom5Button.Location = new System.Drawing.Point(412, 228);
            this.custom5Button.Name = "custom5Button";
            this.custom5Button.Size = new System.Drawing.Size(180, 30);
            this.custom5Button.TabIndex = 22;
            this.custom5Button.Text = "Custom 5: Not Set";
            this.custom5Button.UseVisualStyleBackColor = true;
            
            // saveButton
            this.saveButton.Location = new System.Drawing.Point(412, 300);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(85, 30);
            this.saveButton.TabIndex = 23;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            
            // cancelButton
            this.cancelButton.Location = new System.Drawing.Point(507, 300);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 30);
            this.cancelButton.TabIndex = 24;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            
            // statusLabel
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 308);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(189, 15);
            this.statusLabel.TabIndex = 25;
            this.statusLabel.Text = "Click a button to set a key binding.";
            
            // CustomKeybindsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 341);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.custom5Button);
            this.Controls.Add(this.custom4Button);
            this.Controls.Add(this.custom3Button);
            this.Controls.Add(this.custom2Button);
            this.Controls.Add(this.custom1Button);
            this.Controls.Add(this.disableButton);
            this.Controls.Add(this.enableButton);
            this.Controls.Add(this.mapButton);
            this.Controls.Add(this.inventoryButton);
            this.Controls.Add(this.pingWheelButton);
            this.Controls.Add(this.pingButton);
            this.Controls.Add(this.meleeButton);
            this.Controls.Add(this.sprintButton);
            this.Controls.Add(this.grenadeButton);
            this.Controls.Add(this.ultimateButton);
            this.Controls.Add(this.tacticalButton);
            this.Controls.Add(this.weaponSwap2Button);
            this.Controls.Add(this.weaponSwap1Button);
            this.Controls.Add(this.interactButton);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.slideButton);
            this.Controls.Add(this.crouchButton);
            this.Controls.Add(this.jumpButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomKeybindsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Keybinds";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private System.Windows.Forms.Button jumpButton;
        private System.Windows.Forms.Button crouchButton;
        private System.Windows.Forms.Button slideButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.Button interactButton;
        private System.Windows.Forms.Button weaponSwap1Button;
        private System.Windows.Forms.Button weaponSwap2Button;
        private System.Windows.Forms.Button tacticalButton;
        private System.Windows.Forms.Button ultimateButton;
        private System.Windows.Forms.Button grenadeButton;
        private System.Windows.Forms.Button sprintButton;
        private System.Windows.Forms.Button meleeButton;
        private System.Windows.Forms.Button pingButton;
        private System.Windows.Forms.Button pingWheelButton;
        private System.Windows.Forms.Button inventoryButton;
        private System.Windows.Forms.Button mapButton;
        private System.Windows.Forms.Button enableButton;
        private System.Windows.Forms.Button disableButton;
        private System.Windows.Forms.Button custom1Button;
        private System.Windows.Forms.Button custom2Button;
        private System.Windows.Forms.Button custom3Button;
        private System.Windows.Forms.Button custom4Button;
        private System.Windows.Forms.Button custom5Button;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label statusLabel;
    }
} 