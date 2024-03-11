using HotmailReporting.Reports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace HotmailReporting.Processes
{

    internal class Junk
    {
        private List<FirefoxDriver> drivers = new List<FirefoxDriver>();
        private Panel container;
        private Form parent;
        private bool ComingFromBoth = false;

        public Junk(Panel container, Form parent, bool ComingFromBoth)
        {
            this.container = container;
            this.parent = parent;
            this.ComingFromBoth = ComingFromBoth;
        }

        public object[] Run(string proxyParam, string port, string mail, string pwd)

        {

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

                    FirefoxDriver driver = new CreateWebDriver(proxy).CreateFirefoxDriver();

                    drivers.Add(driver);

                    driverLastIndex = drivers.Count - 1;

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                    wait.Until(driverz => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    Login SignIn = new Login( container , parent);

                    SignIn.Run(driver, mail, pwd);


                    driver.Navigate().GoToUrl("https://login.live.com/login.srf");

                    driver.Navigate().GoToUrl("https://outlook.live.com/mail/0/junkemail");

                    Thread.Sleep(8000);

                    var focusonsearch = driver.FindElement(By.Id("topSearchInput"));

                    focusonsearch.Click();
                 
                    Thread.Sleep(1500);

                    focusonsearch.SendKeys($"received:{System.DateTime.Now.Date.AddDays(-2):yyyy-MM-dd}..{System.DateTime.Now.Date:yyyy-MM-dd}");

                    Thread.Sleep(1500);

                    js.ExecuteScript("document.querySelectorAll(\"button[aria-label= 'Search']\")[0].click()");

                    Thread.Sleep(3500);

                    parent.Invoke(new Action(() =>
                    {
                        new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> filtred by date", -1, -1, -1, mail);

                    }));

              //      js.ExecuteScript("document.querySelectorAll(\"i[data-icon-name = 'MailUnreadRegular']\")[0].click()");

           //         Thread.Sleep(5000);

                    try
                    {
                        var maile = driver.FindElement(By.XPath("//span[@title='Junk Email']"));

                        js.ExecuteScript("arguments[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.click()", maile);

                        Thread.Sleep(5000);

                        while (maile != null)
                        {

                            js.ExecuteScript("arguments[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.click()", maile);

                            Thread.Sleep(5000);
                            try
                            {

                                driver.FindElement(By.XPath("//div[text()='Show blocked content']")).Click();

                            }

                            catch (NoSuchElementException ex)
                            {

                            }

                            //driver.FindElement(By.XPath("//button[@aria-label='Read / Unread']")).Click();

                            //parent.Invoke(new Action(() =>
                            //{
                            //    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> email has been readed", -1, -1, -1, mail);

                            //}));

                            Thread.Sleep(1500);

                            driver.FindElement(By.XPath("//button[@aria-label='Expand to see more report options']")).Click();

                            Thread.Sleep(2000);

                            driver.FindElement(By.XPath("//button[@aria-label='Not junk']")).Click();

                            parent.Invoke(new Action(() =>

                            {
                                new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> email has been reported", -1, -1, -1, mail);

                            }));

                            Thread.Sleep(5000);

                            try
                            {

                                driver.FindElement(By.XPath("//span[text()='OK']")).Click();

                                parent.Invoke(new Action(() =>
                                {
                                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> email has been added to safe senders", -1, -1, -1, mail);

                                }));
                            }
                            catch (NoSuchElementException ex)
                            {

                                Thread.Sleep(30000);

                                parent.Invoke(new Action(() =>
                                {
                                    
                                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> waiting 30 secs for safe sender modal..." +
                                        $"", -1, -1, -1, mail);


                                }));

                                driver.FindElement(By.XPath("//span[text()='OK']")).Click();

                                parent.Invoke(new Action(() =>
                                {
                                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> email has been added to safe senders", -1, -1, -1, mail);

                                }));
                            }


                            maile = driver.FindElement(By.XPath("//span[@title='Junk Email']"));

                            Thread.Sleep(8000);
                        }


                    }

                    catch (NoSuchElementException ex)
                    {


                        parent.Invoke(new Action(() =>
                        {
                            new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> Process done here , no more Junks", 29, 167, 105, mail);

                        }));

                        if (ComingFromBoth == false)
                        {

                            drivers[driverLastIndex].Quit();
                        }

                        object[] return_value = { true, drivers[driverLastIndex] };
                        return return_value;

                    }
                }
                object[] return_value_g = { true, drivers[driverLastIndex] };
                return return_value_g;

            }
            catch (ElementNotSelectableException ex)
            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> an element could  not be selected", 226, 44, 48, mail);

                }));

                object[] return_value = { false, drivers[driverLastIndex] };
                return return_value;
            }


            catch (NoSuchElementException ex)
            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> an element not found ", 226, 44, 48, mail);

                }));
                object[] return_value = { false};
                return return_value;
            }

            catch (ElementNotVisibleException ex)

            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> an element could  not be selected", 226, 44, 48, mail);

                }));

                object[] return_value = { false };
                return return_value;

            }
            catch (WebDriverException ex)

            {

              parent.Invoke(new Action(() =>
                    {
                        new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> browser error ", 226, 44, 48, mail);

                    }));


                object[] return_value = { false };
                return return_value;


            }

            catch (Exception ex)

            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> {ex.Message}.  \n Try again or contact the developer", 226, 44, 48, mail);

                }));

                object[] return_value = { false };
                return return_value;
            }

        }
    }
}
