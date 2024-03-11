using HotmailReporting.Reports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotmailReporting.Processes
{
    internal class Both
    {
        private List<FirefoxDriver> drivers = new List<FirefoxDriver>();
        private Panel container;
        private Form parent;

        public Both(Panel container, Form parent) {
            this.container = container;
            this.parent = parent;
        }




        public void Run(string proxyParam, string port, string mail, string pwd) {
            int driverLastIndex = 0;

            try
            {
                if (methods.TestProxy(proxyParam, int.Parse(port)) == false)
                {
                    parent.Invoke(new Action(() =>
                    {
                        new AssignReportMessage().createReportMessage(container, $"Message from : {mail} ->   proxy : {proxyParam} is down", 226, 44, 48, mail + "->" + proxyParam);

                    }));
                }
                else
                {
                    Proxy proxy = new Proxy();

                    proxy.Kind = ProxyKind.Manual;

                    proxy.IsAutoDetect = false;

                    proxy.SslProxy = proxyParam + ":" + port;

                    if (!methods.TestProxy(proxyParam, int.Parse(port)))
                    {
                        parent.Invoke(new Action(() =>
                        {
                            new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> {proxyParam}:{port}", -1, -1, -1, mail);

                        }));
                    }

        
                    Junk JunkProcess = new Junk(this.container , this.parent , true);

                    object[] result =  JunkProcess.Run(proxyParam, port, mail, pwd);

                    if ((bool)result[0] == true)
                    {
                        Inbox InboxProcess = new Inbox(container,parent , true , (FirefoxDriver)result[1]);

                        InboxProcess.Run(proxyParam, port, mail, pwd);
                    }
                }
            }

            catch { 
            
            }


                }
    }

}
