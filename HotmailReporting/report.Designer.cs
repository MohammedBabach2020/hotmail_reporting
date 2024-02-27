
namespace HotmailReporting
{
    partial class report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(report));
            this.messagesContainer = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.iconButton3 = new FontAwesome.Sharp.IconButton();
            this.label2 = new System.Windows.Forms.Label();
            this.all = new HotmailReporting.BabachRadioButton();
            this.errorsRadio = new HotmailReporting.BabachRadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // messagesContainer
            // 
            this.messagesContainer.AutoScroll = true;
            this.messagesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesContainer.Location = new System.Drawing.Point(0, 80);
            this.messagesContainer.Name = "messagesContainer";
            this.messagesContainer.Size = new System.Drawing.Size(813, 386);
            this.messagesContainer.TabIndex = 1;
            this.messagesContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.messagesContainer_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Location = new System.Drawing.Point(3, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(613, 16);
            this.panel2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(76)))), ((int)(((byte)(119)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Verdana", 8F);
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(613, 13);
            this.textBox1.TabIndex = 25;
            this.textBox1.Text = "search by typing email ...";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.iconButton3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.all);
            this.panel1.Controls.Add(this.errorsRadio);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 80);
            this.panel1.TabIndex = 0;
            // 
            // iconButton3
            // 
            this.iconButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton3.FlatAppearance.BorderSize = 0;
            this.iconButton3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(76)))), ((int)(((byte)(119)))));
            this.iconButton3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(76)))), ((int)(((byte)(119)))));
            this.iconButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(76)))), ((int)(((byte)(119)))));
            this.iconButton3.IconChar = FontAwesome.Sharp.IconChar.Minus;
            this.iconButton3.IconColor = System.Drawing.Color.White;
            this.iconButton3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton3.IconSize = 24;
            this.iconButton3.Location = new System.Drawing.Point(787, 4);
            this.iconButton3.Name = "iconButton3";
            this.iconButton3.Size = new System.Drawing.Size(20, 20);
            this.iconButton3.TabIndex = 28;
            this.iconButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton3.UseVisualStyleBackColor = true;
            this.iconButton3.Click += new System.EventHandler(this.iconButton3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(1, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(409, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "You can always double click the message to copy the associated email";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // all
            // 
            this.all.AutoSize = true;
            this.all.Checked = true;
            this.all.CheckedColor = System.Drawing.Color.White;
            this.all.Cursor = System.Windows.Forms.Cursors.Hand;
            this.all.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.all.ForeColor = System.Drawing.Color.White;
            this.all.Location = new System.Drawing.Point(678, 39);
            this.all.MinimumSize = new System.Drawing.Size(0, 21);
            this.all.Name = "all";
            this.all.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.all.Size = new System.Drawing.Size(52, 21);
            this.all.TabIndex = 23;
            this.all.TabStop = true;
            this.all.Text = "All";
            this.all.UnCheckedColor = System.Drawing.Color.White;
            this.all.UseVisualStyleBackColor = true;
            this.all.CheckedChanged += new System.EventHandler(this.all_CheckedChanged);
            // 
            // errorsRadio
            // 
            this.errorsRadio.AutoSize = true;
            this.errorsRadio.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(0)))), ((int)(((byte)(63)))));
            this.errorsRadio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.errorsRadio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorsRadio.ForeColor = System.Drawing.Color.White;
            this.errorsRadio.Location = new System.Drawing.Point(734, 38);
            this.errorsRadio.MinimumSize = new System.Drawing.Size(0, 21);
            this.errorsRadio.Name = "errorsRadio";
            this.errorsRadio.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.errorsRadio.Size = new System.Drawing.Size(76, 21);
            this.errorsRadio.TabIndex = 22;
            this.errorsRadio.Text = "Errors";
            this.errorsRadio.UnCheckedColor = System.Drawing.Color.White;
            this.errorsRadio.UseVisualStyleBackColor = true;
            this.errorsRadio.CheckedChanged += new System.EventHandler(this.errorsRadio_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 21;
            this.label1.Text = "Reports";
            // 
            // report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(76)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(813, 466);
            this.Controls.Add(this.messagesContainer);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "report";
            this.Load += new System.EventHandler(this.report_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.report_MouseDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel messagesContainer;
        private System.Windows.Forms.Panel panel2;
       
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private BabachRadioButton errors;
        private BabachRadioButton errorsRadio;
        private BabachRadioButton all;
        private System.Windows.Forms.Label label2;
        private FontAwesome.Sharp.IconButton iconButton3;
        private System.Windows.Forms.TextBox textBox1;
    }
}