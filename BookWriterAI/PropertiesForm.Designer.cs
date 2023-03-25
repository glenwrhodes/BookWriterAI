namespace BookWriterAI
{
    partial class PropertiesForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            TemperatureNumUpDown = new NumericUpDown();
            ModeTextBox = new TextBox();
            ApiKeyTextBox = new TextBox();
            button1 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)TemperatureNumUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 19);
            label1.Name = "label1";
            label1.Size = new Size(72, 25);
            label1.TabIndex = 0;
            label1.Text = "API Key";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 69);
            label2.Name = "label2";
            label2.Size = new Size(63, 25);
            label2.TabIndex = 0;
            label2.Text = "Model";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 116);
            label3.Name = "label3";
            label3.Size = new Size(110, 25);
            label3.TabIndex = 0;
            label3.Text = "Temperature";
            // 
            // TemperatureNumUpDown
            // 
            TemperatureNumUpDown.DecimalPlaces = 2;
            TemperatureNumUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            TemperatureNumUpDown.Location = new Point(134, 120);
            TemperatureNumUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            TemperatureNumUpDown.Name = "TemperatureNumUpDown";
            TemperatureNumUpDown.Size = new Size(180, 31);
            TemperatureNumUpDown.TabIndex = 1;
            TemperatureNumUpDown.Value = new decimal(new int[] { 8, 0, 0, 65536 });
            TemperatureNumUpDown.ValueChanged += TemperatureNumUpDown_ValueChanged;
            // 
            // ModeTextBox
            // 
            ModeTextBox.Location = new Point(134, 74);
            ModeTextBox.Name = "ModeTextBox";
            ModeTextBox.Size = new Size(639, 31);
            ModeTextBox.TabIndex = 2;
            ModeTextBox.TextChanged += ModeTextBox_TextChanged;
            // 
            // ApiKeyTextBox
            // 
            ApiKeyTextBox.Location = new Point(135, 23);
            ApiKeyTextBox.Name = "ApiKeyTextBox";
            ApiKeyTextBox.Size = new Size(638, 31);
            ApiKeyTextBox.TabIndex = 3;
            ApiKeyTextBox.TextChanged += ApiKeyTextBox_TextChanged;
            // 
            // button1
            // 
            button1.DialogResult = DialogResult.OK;
            button1.Location = new Point(661, 147);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 4;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.DialogResult = DialogResult.Cancel;
            button2.Location = new Point(543, 149);
            button2.Name = "button2";
            button2.Size = new Size(112, 34);
            button2.TabIndex = 4;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // PropertiesForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(787, 195);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(ApiKeyTextBox);
            Controls.Add(ModeTextBox);
            Controls.Add(TemperatureNumUpDown);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "PropertiesForm";
            Text = "PropertiesForm";
            Load += PropertiesForm_Load;
            ((System.ComponentModel.ISupportInitialize)TemperatureNumUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown TemperatureNumUpDown;
        private TextBox ModeTextBox;
        private TextBox ApiKeyTextBox;
        private Button button1;
        private Button button2;
    }
}