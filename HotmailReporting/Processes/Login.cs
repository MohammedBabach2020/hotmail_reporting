using HotmailReporting.Reports;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace HotmailReporting.Processes
{
    internal class Login
    {

        private Panel container;
        private Form parent;

        public Login(Panel container, Form parent)
        {
            this.container = container;
            this.parent = parent;
        }
        public  void Run(WebDriver driver , string mail , string pwd) 
        {
            driver.Navigate().GoToUrl("https://login.live.com/login.srf");

            Thread.Sleep(3000);


            var usernameFied = driver.FindElement(By.Id("i0116"));
            usernameFied.SendKeys(mail);

            Thread.Sleep(500);
            var next = driver.FindElement(By.Id("idSIButton9"));
            next.Click();

            Thread.Sleep(1500);
            var pwdFied = driver.FindElement(By.Id("i0118"));
            pwdFied.SendKeys(pwd);

            Thread.Sleep(500);
            next = driver.FindElement(By.Id("idSIButton9"));
            next.Click();

            Thread.Sleep(500);


          parent.Invoke(new Action(() =>
            {
                new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> account connected", -1, -1, -1, mail);
            }));


        }
    }
}
