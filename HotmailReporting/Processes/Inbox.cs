using HotmailReporting.Reports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace HotmailReporting.Processes
{
    internal class Inbox
    {
        private List<FirefoxDriver> drivers = new List<FirefoxDriver>();
        private Panel container;
        private Form parent;
        private bool ComingFromBoth = false;
        private FirefoxDriver WebDriver;
        public Inbox(Panel container, Form parent , bool comingFromBoth, FirefoxDriver webDriver = null)
        {
            this.container = container;
            this.parent = parent;
            this.ComingFromBoth = comingFromBoth;
            WebDriver = webDriver;
        }

        public void Run(string proxyParam, string port, string mail, string pwd)
        {
            int driverLastIndex = 0;
            try
            {



                int drv = 0;

                Proxy proxy = new Proxy();

                proxy.Kind = ProxyKind.Manual;
                proxy.IsAutoDetect = false;
                proxy.SslProxy = proxyParam + ":" + port;


                proxy.SocksVersion = 5;


                if (methods.TestProxy(proxyParam, int.Parse(port)) == false)
                {
                    parent.Invoke(new Action(() =>
                    {

                        new AssignReportMessage().createReportMessage(container, $"Message from : {mail} ->   proxy : {proxyParam} is down", 226, 44, 48, mail + " -> " + proxyParam);

                    }));
                }
                else
                {
                    FirefoxDriver driver;

                    IJavaScriptExecutor js;

                    if (ComingFromBoth == false)
                    {
                    driver = new CreateWebDriver(proxy).CreateFirefoxDriver();

                    js = (IJavaScriptExecutor)driver;

                    drivers.Add(driver);

                    driverLastIndex = drivers.Count - 1;

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    wait.Until(driverz => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                    Login SignIn = new Login(container, parent);    

                    SignIn.Run(driver, mail, pwd);

                    driver.Navigate().GoToUrl("https://login.live.com/login.srf");
                      

                    

                    }
                    else
                    {
                        driver = WebDriver;

                        js = (IJavaScriptExecutor)driver;
                    }

                
                    driver.Navigate().GoToUrl("https://outlook.live.com/mail/0/");
                    drivers.Add(driver);
                    driverLastIndex = drivers.Count - 1;
                    Thread.Sleep(7000);

                    IWebElement focusonsearch;

                    try
                    {
                        focusonsearch = driver.FindElement(By.Id("topSearchInput"));

                        focusonsearch.Click();
                    }

                  catch(NoSuchElementException) 
                    
                    {

                        Thread.Sleep(4000);

                        focusonsearch = driver.FindElement(By.Id("topSearchInput"));

                        focusonsearch.Click();

                    }

                    parent.Invoke(new Action(() =>
                    {
                        new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> account connected", -1, -1, -1, mail);

                    }));


                    Thread.Sleep(1500);

                    focusonsearch.SendKeys($"received:{System.DateTime.Now.Date.AddDays(-2):yyyy-MM-dd}..{System.DateTime.Now.Date.ToString("yyyy-MM-dd")}");

                    Thread.Sleep(800);

                    driver.FindElement(By.Id("searchScopeButtonId")).Click();

                    Thread.Sleep(1000);

                    driver.FindElement(By.Id("searchScopeButtonId-list1")).Click();

                    Thread.Sleep(1500);

                    focusonsearch.SendKeys(OpenQA.Selenium.Keys.Enter);

                    Thread.Sleep(3000);

                    js.ExecuteScript(" if(document.querySelectorAll(\"button[aria-label= 'Search']\")[0] != null){ document.querySelectorAll(\"button[aria-label= 'Search']\")[0].click() ; } else {document.querySelectorAll(\"i[data-icon-name='SearchRegular']\")[0].click()  }");

                    parent.Invoke(new Action(() =>
                    {
                        new AssignReportMessage().createReportMessage(container, $"Message from : {mail} ->   filter by date done", -1, -1, -1, mail);

                    }));

                    Thread.Sleep(8000);

                    try
                    {
                        var maile = driver.FindElement(By.XPath("//span[@title='Inbox']"));

                        js.ExecuteScript("arguments[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.click()", maile);

                        while (maile != null)
                        {


                            js.ExecuteScript("arguments[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.click()", maile);

                            Thread.Sleep(5000);

                            driver.FindElement(By.XPath("//button[@aria-label='Archive']")).Click();

                            Thread.Sleep(5000);

                            maile = driver.FindElement(By.XPath("//span[@title='Inbox']"));

                            Thread.Sleep(8000);
                        }


                    }

                    catch (NoSuchElementException ex) {
                        parent.Invoke(new Action(() =>
                        {
                            new AssignReportMessage().createReportMessage(container, $"Message from : {mail} -> Process done here", 29, 167, 105, mail);


                        }));
                        drivers[driverLastIndex].Quit();
                    }
                   



                }



            }
            catch (ElementNotSelectableException ex)
            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail} ->   an element could  not be selected", 226, 44, 48, mail);

                }));
           

            }


            catch (NoSuchElementException ex)
            {
           
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail}  ->  an element not found ", 226, 44, 48, mail);

                }));
            
            }

            catch (ElementNotVisibleException ex)

            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail}  ->  an element could  not be selected", 226, 44, 48, mail);

                }));
            
            }
            catch (WebDriverException ex)

            {
                parent.Invoke(new Action(() =>
                {
                    new AssignReportMessage().createReportMessage(container, $"Message from : {mail}  -> browser error", 226, 44, 48, mail);

                }));



            }

         

        }
    }
}
