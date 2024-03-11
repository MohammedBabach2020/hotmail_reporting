using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HotmailReporting.Reports
{
    internal class AssignReportMessage
    {



        public int hell;
        public void createReportMessage(Panel Container, string message, int r = -1, int g = -1, int b = -1, string tag = "")
        {
            var src = DateTime.Now;
            var hm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            Label errorMessage = new Label();
            errorMessage.ForeColor = r == -1 || g == -1 || b == -1 ? Color.White : Color.FromArgb(r, g, b);
            errorMessage.Text = hm.ToString() + " " + message;
            errorMessage.Dock = DockStyle.Top;
            errorMessage.AutoSize = true;
            errorMessage.Tag = tag;
            errorMessage.Font = new Font("Verdana", 8, FontStyle.Bold);
            errorMessage.Padding = new Padding(5, 0, 0, 15);
            errorMessage.Cursor = Cursors.Hand;
            errorMessage.DoubleClick += (sender, e) =>
            {
                if (errorMessage.Tag.ToString() != "")
                {
                    Clipboard.SetText(errorMessage.Tag.ToString());
                    errorMessage.ForeColor = Color.SpringGreen;
                    Thread.Sleep(2000);
                    errorMessage.ForeColor = r == -1 || g == -1 || b == -1 ? Color.White : Color.FromArgb(r, g, b);
                }

            };
            Container.Controls.Add(errorMessage);

        }
    }
}
