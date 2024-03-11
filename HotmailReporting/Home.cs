using HotmailReporting.Processes;
using HotmailReporting.Reports;
using OfficeOpenXml;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HotmailReporting
{
    public partial class Home : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessageKey(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_SETTEXT = 0x000C;
        private List<FirefoxDriver> drivers = new List<FirefoxDriver>();
        private string[] files = new string[10];
        private int len = 0;
        private DataTable seeds = new DataTable();
        public Home()
        {
            InitializeComponent();
        }
        string thisPcUsername = "";

        report reports = new report();

        public bool IsExcelFileEmpty(string filePath)
        {
            bool isEmpty = true;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension != null)
                    {
                        isEmpty = false; // The worksheet has content
                        break;
                    }
                }
            }

            return isEmpty;
        }


        private bool CheckIfThreadsExist(List<Thread> ListThreads)
        {
            bool ThreadTxist = false;
             foreach (Thread thread in ListThreads)
            {

                if (thread.IsAlive)
                {
                    ThreadTxist = true;
                }
            }


             return ThreadTxist;
        }



        private void launcher(string process)
        {
            
            List<Thread> thrs = new List<Thread>();
            for (int j = 0; j < int.Parse(round.Text); j++)
            {

                int i = 0;

                 thrs = new List<Thread>();
                foreach (DataRow dr in seeds.Rows)
                {

                    i++;
                    if (process == "Archiving")
                    {
                        Thread inboxProcessThread = new Thread(() =>
                        {
                            new Inbox(reports.messagesContainer, this, false).Run(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                        });
                        inboxProcessThread.Start();
                        thrs.Add(inboxProcessThread);
                    }
                    else if (process == "Not Junk")
                    {
                        Thread notJunk = new Thread(() =>
                        {
                            new Junk(reports.messagesContainer, this, false).Run(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                        });
                        notJunk.Start();
                        thrs.Add(notJunk);
                    }
                    else if (process == "Both")
                    {

                        Thread both = new Thread(() =>
                        {
                            new Both(reports.messagesContainer, this).Run(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                        });
                        both.Start();
                        thrs.Add(both);

                    }
                    else
                    {
                        MessageBox.Show("Something goes wrong");
                    }
                    if (i % int.Parse(batch.Text) == 0)
                    {
                        while (CheckIfThreadsExist(thrs))
                        {
                            Console.WriteLine("Threads still working");
                            Thread.Sleep(1000);
                        }

                    }
                }
                while (CheckIfThreadsExist(thrs))
                {
                    Console.WriteLine("Threads still working");
                    Thread.Sleep(1000);
                }

            }

        
        }









        private void Home_Load(object sender, EventArgs e)
        {
            thisPcUsername = Environment.GetEnvironmentVariable("USERNAME");

            reports.Show(this);


        }






        private void button1_Click(object sender, EventArgs e)
        {
         
            string showProcess = "";
            if (inboxing.Checked)
            {
                showProcess = "Go to junk ->  click on email -> Show blocked content -> click not junk -> add to safe sender";
            }
            else if (archiving.Checked)
            {
                showProcess = "Go to focused ->  click on email -> click on image -> [ choose a flag Or Archive Or Pin ]";
            }

            else if (both.Checked)
            {
                showProcess = "Starting with the junk  process and directelly switch to the Inbox process";
            }



            getSureFromProcess show = new getSureFromProcess(showProcess);



            show.ShowDialog();

            if (show.YesOrNo == "Yes")
            {
                Thread process;

                if (inboxing.Checked)
                {
                    process = new Thread(() =>
                    {
                        Invoke(new Action(() =>
                        {
                            new AssignReportMessage().createReportMessage(reports.messagesContainer, $"The not junk proccess starting....", -1, -1, -1);

                        }));
                        launcher("Not Junk");
                    });
                    process.Start();
                }

                if (archiving.Checked)
                {
                    process = new Thread(() =>
                    {
                        Invoke(new Action(() =>
                        {
                            new AssignReportMessage().createReportMessage(reports.messagesContainer, $"The inbox proccess starting....", -1, -1, -1);

                        }));
                        launcher("Archiving");
                    });
                    process.Start();
                }

                if (both.Checked)
                {
                    process = new Thread(() =>
                    {
                        Invoke(new Action(() =>
                        {
                            new AssignReportMessage().createReportMessage(reports.messagesContainer, $"The both proccess starting....", -1, -1, -1);

                        }));
                        launcher("Both");
                    });
                    process.Start();
                }


            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            seeds.Rows.Clear(); seeds.Columns.Clear();
            Thread uploadFile = new Thread(() =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        Invoke(new Action(() =>
                        {
                            filename.Text = openFileDialog.SafeFileName;
                        }));

                        // Read Excel file using EPPlus
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                        {
                            if (!IsExcelFileEmpty(filePath))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming you want the first worksheet

                                // Load Excel data into a DataTable

                                foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                                {
                                    seeds.Columns.Add(firstRowCell.Text);
                                }

                                for (var row = 1; row <= worksheet.Dimension.End.Row; row++)
                                {
                                    var rowCells = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
                                    DataRow dataRow = seeds.Rows.Add();
                                    foreach (var cell in rowCells)
                                    {
                                        dataRow[cell.Start.Column - 1] = cell.Text;
                                    }
                                }
                            }
                        }
                    }
                }
            });

            uploadFile.SetApartmentState(ApartmentState.STA);
            uploadFile.Start();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            inboxing.Checked = true;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            archiving.Checked = true;
        }

        private void inboxing_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void sideBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Home_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            both.Checked = true;
        }
    }
}
