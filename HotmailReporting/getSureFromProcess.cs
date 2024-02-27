using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HotmailReporting
{
    public partial class getSureFromProcess : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private string process;
        private string yesOrNo = "";

        public getSureFromProcess(string process)
        {
            InitializeComponent();

            this.process = process;
        }

        public string YesOrNo { get => yesOrNo; set => yesOrNo = value; }
        public static decimal profileNums = 1;

        private void getSureFromProcess_Load(object sender, EventArgs e)
        {
            label5.Text = this.process;
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            yesOrNo = "Yes";
            this.Close();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            yesOrNo = "No";
            this.Close();
        }

        private void getSureFromProcess_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            profileNums = numericUpDown1.Value;
        }

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            if (numericUpDown1.Value > 30)
            {
                numericUpDown1.Value = 30;
            }

            if (numericUpDown1.Value < 1)
            {
                numericUpDown1.Value = 1;
            }
        }
    }
}