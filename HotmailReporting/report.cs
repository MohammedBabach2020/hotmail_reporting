using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

using System.Windows.Forms;
namespace HotmailReporting
{
    public partial class report : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public report()
        {
            InitializeComponent();
        }

        private void report_Load(object sender, EventArgs e)
        {


            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            textBox1.Text = "search by typing email ...";
        }

        private void report_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        static List<Label> GetAllLabels(Control control)
        {
            List<Label> labels = new List<Label>();
            Thread th = new Thread(() =>
            {
                foreach (Control childControl in control.Controls)
                {

                    if (childControl is Label)
                    {
                        labels.Add((Label)childControl);
                    }
                    else
                    {
                        labels.AddRange(GetAllLabels(childControl));
                    }
                }
            });
            th.Start();
            return labels;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            List<Label> GetAllLabelsLocal = GetAllLabels(messagesContainer);
            if (textBox1.Text == "")
            {
                Thread th = new Thread(() =>
                {
                    foreach (Label lbl in GetAllLabelsLocal)
                    {
                        lbl.Visible = true;
                    }
                });
                th.Start();
            }
            else
            {
                Thread th = new Thread(() =>
                {
                    foreach (Label lbl in GetAllLabelsLocal)
                    {
                        if (lbl.Text.Contains(textBox1.Text))
                        {
                            lbl.Visible = true;
                        }
                        else
                        {
                            lbl.Visible = false;
                        }
                    }
                });

                th.Start();


            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "search by typing email ...")
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "search by typing email ...";
            }
        }

        private void errorsRadio_CheckedChanged(object sender, EventArgs e)
        {
            List<Label> GetAllLabelsLocal = GetAllLabels(messagesContainer);
            if (errorsRadio.Checked)
            {
                errorsRadio.ForeColor = Color.FromArgb(249, 0, 63);

                Thread th = new Thread(() =>
                {


                    foreach (Label lbl in GetAllLabelsLocal)
                    {
                        if (lbl.ForeColor == Color.FromArgb(226, 44, 48))
                        {
                            Invoke(new Action(() =>
                            {

                                lbl.Visible = true;
                            }));

                        }
                        else
                        {

                            Invoke(new Action(() =>
                            {

                                lbl.Visible = false;
                            }));
                        }
                    }
                });

                th.Start();


            }
            else
            {
                errorsRadio.ForeColor = Color.White;
                Thread th = new Thread(() =>
                {
                    foreach (Label lbl in GetAllLabelsLocal)
                    {
                        Invoke(new Action(() =>
                        {

                            lbl.Visible = true;
                        }));
                    }
                });
                th.Start();

            }
        }

        private void all_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void messagesContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
