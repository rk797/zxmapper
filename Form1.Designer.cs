using System;
using System.Drawing;

namespace zxmapper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.applyButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.driverText = new System.Windows.Forms.Label();
            this.output = new System.Windows.Forms.Label();
            this.streamProof = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.taskbarHide = new System.Windows.Forms.CheckBox();
            this.applyLabel = new System.Windows.Forms.Label();
            this.scalefactorLabel = new System.Windows.Forms.Label();
            this.expfactorLabel = new System.Windows.Forms.Label();
            this.expFactorBox = new System.Windows.Forms.NumericUpDown();
            this.scaleFactorBox = new System.Windows.Forms.NumericUpDown();
            this.toggleDropDown = new System.Windows.Forms.ComboBox();
            this.SensYLabel = new System.Windows.Forms.Label();
            this.southpaw = new System.Windows.Forms.CheckBox();
            this.SensXLabel = new System.Windows.Forms.Label();
            this.sensBoxX = new System.Windows.Forms.NumericUpDown();
            this.sensBoxY = new System.Windows.Forms.NumericUpDown();
            this.lToggle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.l_forward = new System.Windows.Forms.Label();
            this.b_forward = new System.Windows.Forms.Button();
            this.l_right = new System.Windows.Forms.Label();
            this.b_right = new System.Windows.Forms.Button();
            this.l_left = new System.Windows.Forms.Label();
            this.b_left = new System.Windows.Forms.Button();
            this.l_backwards = new System.Windows.Forms.Label();
            this.b_backwards = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expFactorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensBoxX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensBoxY)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // applyButton
            // 
            this.applyButton.BackColor = System.Drawing.Color.Transparent;
            this.applyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.applyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.applyButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.applyButton.Location = new System.Drawing.Point(182, 476);
            this.applyButton.Margin = new System.Windows.Forms.Padding(0);
            this.applyButton.Name = "applyButton";
            this.applyButton.Padding = new System.Windows.Forms.Padding(5);
            this.applyButton.Size = new System.Drawing.Size(84, 37);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = false;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click_1);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.driverText);
            this.panel1.Controls.Add(this.output);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 570);
            this.panel1.TabIndex = 3;
            // 
            // driverText
            // 
            this.driverText.BackColor = System.Drawing.Color.Transparent;
            this.driverText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverText.Font = new System.Drawing.Font("Segoe UI", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.driverText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(204)))), ((int)(((byte)(93)))));
            this.driverText.Location = new System.Drawing.Point(296, 436);
            this.driverText.Name = "driverText";
            this.driverText.Size = new System.Drawing.Size(447, 42);
            this.driverText.TabIndex = 6;
            this.driverText.Text = "initializing...";
            this.driverText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // output
            // 
            this.output.AutoSize = true;
            this.output.BackColor = System.Drawing.Color.Transparent;
            this.output.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.output.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(204)))), ((int)(((byte)(93)))));
            this.output.Location = new System.Drawing.Point(59, 189);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(0, 15);
            this.output.TabIndex = 5;
            this.output.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // streamProof
            // 
            this.streamProof.AutoSize = true;
            this.streamProof.BackColor = System.Drawing.Color.Transparent;
            this.streamProof.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.streamProof.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.streamProof.Location = new System.Drawing.Point(27, 248);
            this.streamProof.Name = "streamProof";
            this.streamProof.Size = new System.Drawing.Size(123, 21);
            this.streamProof.TabIndex = 5;
            this.streamProof.Text = "STREAM PROOF";
            this.streamProof.UseVisualStyleBackColor = false;
            this.streamProof.CheckedChanged += new System.EventHandler(this.streamProof_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.infoLabel);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1018, 570);
            this.panel2.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.taskbarHide);
            this.groupBox2.Controls.Add(this.applyLabel);
            this.groupBox2.Controls.Add(this.scalefactorLabel);
            this.groupBox2.Controls.Add(this.expfactorLabel);
            this.groupBox2.Controls.Add(this.expFactorBox);
            this.groupBox2.Controls.Add(this.scaleFactorBox);
            this.groupBox2.Controls.Add(this.toggleDropDown);
            this.groupBox2.Controls.Add(this.SensYLabel);
            this.groupBox2.Controls.Add(this.applyButton);
            this.groupBox2.Controls.Add(this.southpaw);
            this.groupBox2.Controls.Add(this.SensXLabel);
            this.groupBox2.Controls.Add(this.sensBoxX);
            this.groupBox2.Controls.Add(this.sensBoxY);
            this.groupBox2.Controls.Add(this.streamProof);
            this.groupBox2.Controls.Add(this.lToggle);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(701, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 530);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MISC";
            // 
            // taskbarHide
            // 
            this.taskbarHide.AutoSize = true;
            this.taskbarHide.BackColor = System.Drawing.Color.Transparent;
            this.taskbarHide.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.taskbarHide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.taskbarHide.Location = new System.Drawing.Point(27, 318);
            this.taskbarHide.Name = "taskbarHide";
            this.taskbarHide.Size = new System.Drawing.Size(118, 21);
            this.taskbarHide.TabIndex = 28;
            this.taskbarHide.Text = "TASKBAR HIDE";
            this.taskbarHide.UseVisualStyleBackColor = false;
            this.taskbarHide.CheckedChanged += new System.EventHandler(this.taskbarHide_CheckedChanged);
            // 
            // applyLabel
            // 
            this.applyLabel.AutoSize = true;
            this.applyLabel.BackColor = System.Drawing.Color.Transparent;
            this.applyLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.applyLabel.Location = new System.Drawing.Point(6, 493);
            this.applyLabel.Name = "applyLabel";
            this.applyLabel.Size = new System.Drawing.Size(53, 20);
            this.applyLabel.TabIndex = 27;
            this.applyLabel.Text = "debug";
            // 
            // scalefactorLabel
            // 
            this.scalefactorLabel.AutoSize = true;
            this.scalefactorLabel.BackColor = System.Drawing.Color.Transparent;
            this.scalefactorLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scalefactorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.scalefactorLabel.Location = new System.Drawing.Point(156, 108);
            this.scalefactorLabel.Name = "scalefactorLabel";
            this.scalefactorLabel.Size = new System.Drawing.Size(51, 20);
            this.scalefactorLabel.TabIndex = 26;
            this.scalefactorLabel.Text = "SCALE";
            // 
            // expfactorLabel
            // 
            this.expfactorLabel.AutoSize = true;
            this.expfactorLabel.BackColor = System.Drawing.Color.Transparent;
            this.expfactorLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expfactorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.expfactorLabel.Location = new System.Drawing.Point(23, 108);
            this.expfactorLabel.Name = "expfactorLabel";
            this.expfactorLabel.Size = new System.Drawing.Size(35, 20);
            this.expfactorLabel.TabIndex = 25;
            this.expfactorLabel.Text = "EXP";
            // 
            // expFactorBox
            // 
            this.expFactorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expFactorBox.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expFactorBox.Location = new System.Drawing.Point(24, 136);
            this.expFactorBox.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.expFactorBox.Name = "expFactorBox";
            this.expFactorBox.Size = new System.Drawing.Size(100, 27);
            this.expFactorBox.TabIndex = 23;
            this.expFactorBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // scaleFactorBox
            // 
            this.scaleFactorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scaleFactorBox.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleFactorBox.Location = new System.Drawing.Point(157, 136);
            this.scaleFactorBox.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.scaleFactorBox.Name = "scaleFactorBox";
            this.scaleFactorBox.Size = new System.Drawing.Size(100, 27);
            this.scaleFactorBox.TabIndex = 24;
            this.scaleFactorBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // toggleDropDown
            // 
            this.toggleDropDown.FormattingEnabled = true;
            this.toggleDropDown.Items.AddRange(new object[] {
            "MOUSEX5"});
            this.toggleDropDown.Location = new System.Drawing.Point(135, 194);
            this.toggleDropDown.Name = "toggleDropDown";
            this.toggleDropDown.Size = new System.Drawing.Size(121, 28);
            this.toggleDropDown.TabIndex = 22;
            this.toggleDropDown.SelectedIndexChanged += new System.EventHandler(this.toggleDropDown_SelectedIndexChanged);
            // 
            // SensYLabel
            // 
            this.SensYLabel.AutoSize = true;
            this.SensYLabel.BackColor = System.Drawing.Color.Transparent;
            this.SensYLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SensYLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SensYLabel.Location = new System.Drawing.Point(156, 33);
            this.SensYLabel.Name = "SensYLabel";
            this.SensYLabel.Size = new System.Drawing.Size(58, 20);
            this.SensYLabel.TabIndex = 9;
            this.SensYLabel.Text = "SENS Y";
            // 
            // southpaw
            // 
            this.southpaw.AutoSize = true;
            this.southpaw.BackColor = System.Drawing.Color.Transparent;
            this.southpaw.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.southpaw.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.southpaw.Location = new System.Drawing.Point(27, 283);
            this.southpaw.Name = "southpaw";
            this.southpaw.Size = new System.Drawing.Size(104, 21);
            this.southpaw.TabIndex = 17;
            this.southpaw.Text = "SOUTH PAW";
            this.southpaw.UseVisualStyleBackColor = false;
            this.southpaw.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SensXLabel
            // 
            this.SensXLabel.AutoSize = true;
            this.SensXLabel.BackColor = System.Drawing.Color.Transparent;
            this.SensXLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SensXLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SensXLabel.Location = new System.Drawing.Point(23, 33);
            this.SensXLabel.Name = "SensXLabel";
            this.SensXLabel.Size = new System.Drawing.Size(58, 20);
            this.SensXLabel.TabIndex = 8;
            this.SensXLabel.Text = "SENS X";
            // 
            // sensBoxX
            // 
            this.sensBoxX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sensBoxX.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensBoxX.Location = new System.Drawing.Point(23, 60);
            this.sensBoxX.Name = "sensBoxX";
            this.sensBoxX.Size = new System.Drawing.Size(100, 27);
            this.sensBoxX.TabIndex = 10;
            this.sensBoxX.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // sensBoxY
            // 
            this.sensBoxY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sensBoxY.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensBoxY.Location = new System.Drawing.Point(156, 60);
            this.sensBoxY.Name = "sensBoxY";
            this.sensBoxY.Size = new System.Drawing.Size(100, 27);
            this.sensBoxY.TabIndex = 11;
            this.sensBoxY.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lToggle
            // 
            this.lToggle.AutoSize = true;
            this.lToggle.BackColor = System.Drawing.Color.Transparent;
            this.lToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lToggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lToggle.Location = new System.Drawing.Point(26, 194);
            this.lToggle.Name = "lToggle";
            this.lToggle.Size = new System.Drawing.Size(84, 20);
            this.lToggle.TabIndex = 21;
            this.lToggle.Text = "Toggle Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Location = new System.Drawing.Point(454, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Unlock Sequence";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.l_forward);
            this.groupBox1.Controls.Add(this.b_forward);
            this.groupBox1.Controls.Add(this.l_right);
            this.groupBox1.Controls.Add(this.b_right);
            this.groupBox1.Controls.Add(this.l_left);
            this.groupBox1.Controls.Add(this.b_left);
            this.groupBox1.Controls.Add(this.l_backwards);
            this.groupBox1.Controls.Add(this.b_backwards);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(34, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 530);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key Binds";
            // 
            // l_forward
            // 
            this.l_forward.AutoSize = true;
            this.l_forward.BackColor = System.Drawing.Color.Transparent;
            this.l_forward.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_forward.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.l_forward.Location = new System.Drawing.Point(26, 43);
            this.l_forward.Name = "l_forward";
            this.l_forward.Size = new System.Drawing.Size(80, 20);
            this.l_forward.TabIndex = 31;
            this.l_forward.Text = "FORWARD";
            // 
            // b_forward
            // 
            this.b_forward.BackColor = System.Drawing.Color.Transparent;
            this.b_forward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_forward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_forward.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_forward.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.b_forward.Location = new System.Drawing.Point(172, 34);
            this.b_forward.Margin = new System.Windows.Forms.Padding(0);
            this.b_forward.Name = "b_forward";
            this.b_forward.Padding = new System.Windows.Forms.Padding(5);
            this.b_forward.Size = new System.Drawing.Size(84, 37);
            this.b_forward.TabIndex = 30;
            this.b_forward.Text = ".";
            this.b_forward.UseVisualStyleBackColor = false;
            // 
            // l_right
            // 
            this.l_right.AutoSize = true;
            this.l_right.BackColor = System.Drawing.Color.Transparent;
            this.l_right.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_right.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.l_right.Location = new System.Drawing.Point(26, 203);
            this.l_right.Name = "l_right";
            this.l_right.Size = new System.Drawing.Size(51, 20);
            this.l_right.TabIndex = 19;
            this.l_right.Text = "RIGHT";
            // 
            // b_right
            // 
            this.b_right.BackColor = System.Drawing.Color.Transparent;
            this.b_right.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_right.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_right.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_right.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.b_right.Location = new System.Drawing.Point(172, 194);
            this.b_right.Margin = new System.Windows.Forms.Padding(0);
            this.b_right.Name = "b_right";
            this.b_right.Padding = new System.Windows.Forms.Padding(5);
            this.b_right.Size = new System.Drawing.Size(84, 37);
            this.b_right.TabIndex = 18;
            this.b_right.Text = ".";
            this.b_right.UseVisualStyleBackColor = false;
            // 
            // l_left
            // 
            this.l_left.AutoSize = true;
            this.l_left.BackColor = System.Drawing.Color.Transparent;
            this.l_left.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_left.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.l_left.Location = new System.Drawing.Point(26, 150);
            this.l_left.Name = "l_left";
            this.l_left.Size = new System.Drawing.Size(40, 20);
            this.l_left.TabIndex = 17;
            this.l_left.Text = "LEFT";
            // 
            // b_left
            // 
            this.b_left.BackColor = System.Drawing.Color.Transparent;
            this.b_left.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_left.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_left.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_left.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.b_left.Location = new System.Drawing.Point(172, 141);
            this.b_left.Margin = new System.Windows.Forms.Padding(0);
            this.b_left.Name = "b_left";
            this.b_left.Padding = new System.Windows.Forms.Padding(5);
            this.b_left.Size = new System.Drawing.Size(84, 37);
            this.b_left.TabIndex = 16;
            this.b_left.Text = ".";
            this.b_left.UseVisualStyleBackColor = false;
            // 
            // l_backwards
            // 
            this.l_backwards.AutoSize = true;
            this.l_backwards.BackColor = System.Drawing.Color.Transparent;
            this.l_backwards.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_backwards.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.l_backwards.Location = new System.Drawing.Point(26, 98);
            this.l_backwards.Name = "l_backwards";
            this.l_backwards.Size = new System.Drawing.Size(97, 20);
            this.l_backwards.TabIndex = 15;
            this.l_backwards.Text = "BACKWARDS";
            // 
            // b_backwards
            // 
            this.b_backwards.BackColor = System.Drawing.Color.Transparent;
            this.b_backwards.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_backwards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_backwards.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_backwards.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.b_backwards.Location = new System.Drawing.Point(172, 89);
            this.b_backwards.Margin = new System.Windows.Forms.Padding(0);
            this.b_backwards.Name = "b_backwards";
            this.b_backwards.Padding = new System.Windows.Forms.Padding(5);
            this.b_backwards.Size = new System.Drawing.Size(84, 37);
            this.b_backwards.TabIndex = 13;
            this.b_backwards.Text = ".";
            this.b_backwards.UseVisualStyleBackColor = false;
            this.b_backwards.Click += new System.EventHandler(this.tacAbilityButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.infoLabel.Location = new System.Drawing.Point(421, 488);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(164, 20);
            this.infoLabel.TabIndex = 15;
            this.infoLabel.Text = "CTRL + ALT + DELETE";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1017, 562);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximumSize = new System.Drawing.Size(1035, 609);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expFactorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensBoxX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensBoxY)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox streamProof;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label SensXLabel;
        private System.Windows.Forms.Label SensYLabel;
        private System.Windows.Forms.Label output;
        private System.Windows.Forms.Label driverText;
        private System.Windows.Forms.NumericUpDown sensBoxY;
        private System.Windows.Forms.NumericUpDown sensBoxX;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button b_backwards;
        private System.Windows.Forms.Label l_backwards;
        private System.Windows.Forms.Label l_left;
        private System.Windows.Forms.Button b_left;
        private System.Windows.Forms.Label l_right;
        private System.Windows.Forms.Button b_right;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lToggle;
        private System.Windows.Forms.CheckBox southpaw;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox toggleDropDown;
        private System.Windows.Forms.Label l_forward;
        private System.Windows.Forms.Button b_forward;
        private System.Windows.Forms.NumericUpDown expFactorBox;
        private System.Windows.Forms.NumericUpDown scaleFactorBox;
        private System.Windows.Forms.Label expfactorLabel;
        private System.Windows.Forms.Label scalefactorLabel;
        private System.Windows.Forms.Label applyLabel;
        private System.Windows.Forms.CheckBox taskbarHide;
    }
}

