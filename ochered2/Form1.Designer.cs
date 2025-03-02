namespace ochered2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnClear = new Button();
            textBoxResults = new TextBox();
            lblCashier1 = new Label();
            lblCashier2 = new Label();
            lblCashier3 = new Label();
            lblStats = new Label();
            SuspendLayout();
            // 
            // btnClear
            // 
            btnClear.Location = new Point(514, 64);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(90, 28);
            btnClear.TabIndex = 0;
            btnClear.Text = "очистить";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // textBoxResults
            // 
            textBoxResults.Location = new Point(514, 167);
            textBoxResults.Multiline = true;
            textBoxResults.Name = "textBoxResults";
            textBoxResults.ScrollBars = ScrollBars.Vertical;
            textBoxResults.Size = new Size(120, 26);
            textBoxResults.TabIndex = 1;
            textBoxResults.TextChanged += textBox1_TextChanged;
            // 
            // lblCashier1
            // 
            lblCashier1.AutoSize = true;
            lblCashier1.Location = new Point(82, 38);
            lblCashier1.Name = "lblCashier1";
            lblCashier1.Size = new Size(50, 20);
            lblCashier1.TabIndex = 2;
            lblCashier1.Text = "label1";
            // 
            // lblCashier2
            // 
            lblCashier2.AutoSize = true;
            lblCashier2.Location = new Point(217, 38);
            lblCashier2.Name = "lblCashier2";
            lblCashier2.Size = new Size(50, 20);
            lblCashier2.TabIndex = 3;
            lblCashier2.Text = "label2";
            // 
            // lblCashier3
            // 
            lblCashier3.AutoSize = true;
            lblCashier3.Location = new Point(314, 38);
            lblCashier3.Name = "lblCashier3";
            lblCashier3.Size = new Size(50, 20);
            lblCashier3.TabIndex = 4;
            lblCashier3.Text = "label3";
            // 
            // lblStats
            // 
            lblStats.AutoSize = true;
            lblStats.Location = new Point(69, 358);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(50, 20);
            lblStats.TabIndex = 5;
            lblStats.Text = "label4";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblStats);
            Controls.Add(lblCashier3);
            Controls.Add(lblCashier2);
            Controls.Add(lblCashier1);
            Controls.Add(textBoxResults);
            Controls.Add(btnClear);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxResults;
        private System.Windows.Forms.Label lblCashier1;
        private System.Windows.Forms.Label lblCashier2;
        private System.Windows.Forms.Label lblCashier3;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Button btnClear;
    }
}
