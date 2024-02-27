using Ini;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        private void createReportMessage(Panel Container, string message, int r = -1, int g = -1, int b = -1, string tag = "")
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
                if(errorMessage.Tag.ToString() != "")
                {
                    Clipboard.SetText(errorMessage.Tag.ToString());
                    errorMessage.ForeColor = Color.SpringGreen;
                    Thread.Sleep(2000);
                    errorMessage.ForeColor = r == -1 || g == -1 || b == -1 ? Color.White : Color.FromArgb(r, g, b);
                }
              
            };
            Container.Controls.Add(errorMessage);

        }

        private void notJunkFunc(string proxyParam, string port, string mail, string pwd)

        {
            int driverLastIndex = 0;

            try
            {
                if (methods.TestProxy(proxyParam, int.Parse(port)) == false)
                {
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} ->   proxy : {proxyParam} is down", 226, 44, 48, mail+"->"+proxyParam);

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
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> {proxyParam}:{port}", -1, -1, -1, mail);

                    }));
                }

                FirefoxOptions options = new FirefoxOptions();
                options.Proxy = proxy;
                options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.AddArgument("--width=900");
                options.AddArgument("--height=950");
                options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\WebDriver\bin\geckodriver.exe");

                FirefoxDriver driver = new FirefoxDriver(service, options);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                drivers.Add(driver);
                driverLastIndex = drivers.Count - 1;

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(driverz => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
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

                driver.Navigate().GoToUrl("https://login.live.com/login.srf");
                driver.Navigate().GoToUrl("https://outlook.live.com/mail/0/junkemail");
                Thread.Sleep(8000);



                var focusonsearch = driver.FindElement(By.Id("topSearchInput"));
                focusonsearch.Click();
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> account connected", -1, -1, -1, mail);

                }));


                Thread.Sleep(1500);
                focusonsearch.SendKeys($"received:{System.DateTime.Now.Date.ToString("yyyy-MM-dd")}..{System.DateTime.Now.Date.ToString("yyyy-MM-dd")}");
                // focusonsearch.SendKeys($"received:2023-07-01..2023-07-31");
                Thread.Sleep(1500);
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> filtred by date", -1, -1, -1, mail);

                }));
                js.ExecuteScript("document.querySelectorAll(\"button[aria-label= 'Search']\")[0].click()");
                Thread.Sleep(2000);
                js.ExecuteScript("document.querySelectorAll(\"i[data-icon-name = 'MailUnreadRegular']\")[0].click()");
                Thread.Sleep(5000);
                js.ExecuteScript("window.containere = document.getElementsByClassName('customScrollBar')[2]");
                   
                    string isMailsExist = (string)js.ExecuteScript("window.mails = window.containere.querySelectorAll(\"div[draggable = 'true']\"); if(window.mails.length <= 0 ){ return '0' ; } else {  return String(window.mails.length) ; }   ");
                int i = 0;

                if (int.Parse(isMailsExist) > 0)
                {
                    bool ListEnded = false;
                    while (ListEnded == false)
                    {
                        isMailsExist = (string)js.ExecuteScript("window.mails = window.containere.querySelectorAll(\"div[draggable = 'true']\"); if(window.mails.length <= 0 ){ return '0' ; } else {  return String(window.mails.length) ; }   ");

                        bool isThereANyJunkElements = (bool)js.ExecuteScript("for(var div of window.mails){  if(div.querySelectorAll(\"span[title = 'Junk Email']\").length > 0 ) {  return true ; break ; } else {return false ;} } ");

                        if (!isThereANyJunkElements)
                        {
                            break;
                        }


                        Random rnd = new Random();
                        i = rnd.Next(0, int.Parse(isMailsExist));
                        bool isJunk = (bool)js.ExecuteScript("var isJunk = window.mails[" + i.ToString() + "].querySelectorAll(\"span[title = 'Junk Email']\"); if ( isJunk.length > 0){ window.mails[" + i.ToString() + "].click(); return true; } else{ return false ; } ");
                      
                            Thread.Sleep(5000);

                            js.ExecuteScript("document.querySelectorAll(\"button[aria-label='More actions']\")[0].click()");
                            Thread.Sleep(2000);
                          
                            js.ExecuteScript("document.querySelectorAll(\"button[aria-label='Mark as read']\")[0].click()");
                      


                        


                            string notJunkLinkElements = (string)js.ExecuteScript(" var body = document.getElementsByClassName('wide-content-host')[0]; if(body != null ) { window.notJunkLink = body.querySelectorAll('div') ; return String(window.notJunkLink.length) } else {return '0' } ");
                   

                            if (int.Parse(notJunkLinkElements) > 0)
                        {

                            for (int notJunkLinkIndex = 0; notJunkLinkIndex < int.Parse(notJunkLinkElements); notJunkLinkIndex++)
                            {
                                 

                                    js.ExecuteScript("window.notJunkLinkHasbeenClicked  = false; if (window.notJunkLink[" + notJunkLinkIndex.ToString() + "].textContent ==" + "'Show blocked content'" + ") {  window.notJunkLink[" + notJunkLinkIndex.ToString() + "].click() ;  }    ");
                                js.ExecuteScript("window.notJunkLinkHasbeenClicked  = false; if (window.notJunkLink[" + notJunkLinkIndex.ToString() + "].textContent ==" + "\"It's not junk\"" + ") {  window.notJunkLink[" + notJunkLinkIndex.ToString() + "].click(); window.notJunkLinkHasbeenClicked =true;  }    ");
  
                                    bool clicked = (bool)js.ExecuteScript("return window.notJunkLinkHasbeenClicked");
                                if (clicked)
                                {
                                    Thread.Sleep(2000);
                                    js.ExecuteScript("if(window.notJunkLinkHasbeenClicked ==true ){document.querySelectorAll(\"button[id^='ok-']\")[0].click();}");
                                }

                                 
                                }
                        }
                        Thread.Sleep(1000);
                        isMailsExist = (string)js.ExecuteScript("var stillAnEmail = document.querySelectorAll(\"div[draggable = 'true']\"); if(stillAnEmail <= 0 ){ return '0' ; } else {  return String(stillAnEmail.length) ; }   ");

                        if (int.Parse(isMailsExist) <= 0)
                        {
                            ListEnded = true;
                        }
                    }
                }

                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> reported not junk", -1, -1, -1, mail);

                    }));
                    Console.WriteLine("Done");
                driver.Quit();

            }
            }
            catch (ElementNotSelectableException ex)
            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> an element could  not be selected", 226, 44, 48, mail);

                }));
         //      drivers[driverLastIndex].Quit();

            }


            catch (NoSuchElementException ex)
            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> an element not found ", 226, 44, 48, mail);

                }));
                // drivers[driverLastIndex].Quit();
            }

            catch (ElementNotVisibleException ex)

            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> an element could  not be selected", 226, 44, 48, mail);

                }));
              //  drivers[driverLastIndex].Quit();
            }
            catch (WebDriverException ex)

            {

                if (ex.Message.Contains("undefined"))

                {
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> Process done here", 29, 167, 105, mail);


                    }));
                    drivers[driverLastIndex].Quit();
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> error accessing the link retry again please", 226, 44, 48, mail);

                    }));
               //     drivers[driverLastIndex].Quit();

                }

            }

            catch (Exception ex)

            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> {ex.Message}.  \n Try again or contact the developer", 226, 44, 48, mail);

                }));
            }

        }

        private void inboxProcess(string proxyParam, string port, string mail, string pwd)
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
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} ->   proxy : {proxyParam} is down", 226, 44, 48, mail + " -> " + proxyParam);

                    }));
                }
                else
                {
                    FirefoxProfile profile = new FirefoxProfile();
                    profile.SetPreference("browser.download.folderList", 2);  // 0: Desktop, 1: Downloads, 2: Custom Location
                    profile.SetPreference("browser.download.dir", "C:\\MyDownloads");  // Set your custom download directory
                    profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");  // Set allowed MIME types




                    FirefoxOptions options = new FirefoxOptions();

                    options.Proxy = proxy;
                    options.PageLoadStrategy = PageLoadStrategy.Normal;
                    options.AddArgument("--width=800");
                    options.AddArgument("--height=950");
                    options.Profile = profile;
                    options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\WebDriver\bin\geckodriver.exe");
                    //service, options
                    FirefoxDriver driver = new FirefoxDriver(service, options);
                    drivers.Add(driver);
                    driverLastIndex = drivers.Count - 1;

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(driverz => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
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


                    driver.Navigate().GoToUrl("https://login.live.com/login.srf");
                    driver.Navigate().GoToUrl("https://outlook.live.com/mail/0/");
                    Thread.Sleep(5000);


                    PointerInputDevice mouse;
                    ActionBuilder actionBuilder = new ActionBuilder();

                    var focusonsearch = driver.FindElement(By.Id("topSearchInput"));
                    focusonsearch.Click();
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> account connected", -1, -1, -1, mail);

                    }));


                    Thread.Sleep(1500);
                    focusonsearch.SendKeys($"received:{System.DateTime.Now.Date.ToString("yyyy-MM-dd")}..{System.DateTime.Now.Date.ToString("yyyy-MM-dd")}");
                    Thread.Sleep(800);
                    focusonsearch.SendKeys(OpenQA.Selenium.Keys.Enter);


                    Thread.Sleep(3000);

                    js.ExecuteScript(" if(document.querySelectorAll(\"button[aria-label= 'Search']\")[0] != null){ document.querySelectorAll(\"button[aria-label= 'Search']\")[0].click() ; } else {document.querySelectorAll(\"i[data-icon-name='SearchRegular']\")[0].click()  }");

                    Thread.Sleep(3000);
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} ->   filter by date done", -1, -1, -1, mail);

                    }));

                    //js.ExecuteScript("document.querySelectorAll(\"i[data-icon-name = 'MailUnreadRegular']\")[0].click()");
                    //Thread.Sleep(3000);

                    js.ExecuteScript("window.containere = document.getElementsByClassName('customScrollBar')[2]");
                    Thread.Sleep(2000);

                    string isMailsExist = (string)js.ExecuteScript("window.mails = window.containere.querySelectorAll(\"div[draggable = 'true']\"); if(window.mails.length <= 0 ){ return '0' ; } else {  return String(window.mails.length) ; }   ");
                    int nextmmail = 0;

                    if (int.Parse(isMailsExist) > 0)
                    {
                        bool ListEnded = false;
                        while (ListEnded == false)
                        {
                            bool isThereANyInboxElements = (bool)js.ExecuteScript("for(var div of window.mails){  if(div.querySelectorAll(\"span[title = 'Inbox']\").length > 0 ) {  return true ; break ; } else {return false ;} } ");

                            if (!isThereANyInboxElements)
                            {
                                break;
                            }

                            bool isFocused = (bool)js.ExecuteScript("var isFocused = window.mails[" + nextmmail.ToString() + "].querySelectorAll(\"span[title = 'Inbox']\"); if ( isFocused.length > 0){ window.mails[" + nextmmail.ToString() + "].click(); return true; } else{ return false ; } ");

                            Thread.Sleep(5000);
                            string originalWindow = driver.CurrentWindowHandle;
                            mouse = new PointerInputDevice(PointerKind.Mouse, "default mouse");
                            actionBuilder = new ActionBuilder();
                            //actionBuilder.AddAction(mouse.CreatePointerMove(CoordinateOrigin.Viewport, 650, 400, TimeSpan.Zero));
                            //actionBuilder.AddAction(mouse.CreatePointerDown(MouseButton.Left));
                            //actionBuilder.AddAction(mouse.CreatePointerUp(MouseButton.Left));
                        //    ((IActionExecutor)driver).PerformActions(actionBuilder.ToActionSequenceList());
                            Thread.Sleep(2000);
                            foreach (string window in driver.WindowHandles)
                            {
                                if (originalWindow != window)
                                {
                                    driver.SwitchTo().Window(window);
                                    driver.Close();
                                    break;
                                }
                                Thread.Sleep(500);
                            }
                            driver.SwitchTo().Window(originalWindow);

                            Thread.Sleep(2000);
                            js.ExecuteScript("document.getElementById('RibbonOverflowMenu-overflow').click();");

                            string[] processes = { "flag", "pin", "archive", "category" };
                            Random rnd = new Random();
                            int i = rnd.Next(0, 3);
                            Thread.Sleep(2000);
                            switch (i)
                            {
                                case 0:

                                    bool flagUnflag = (bool)js.ExecuteScript("if(document.querySelectorAll(\"button[name='Flag / Unflag']\")[0] == null ){ return false ;  } else { return true } ");

                                    if (flagUnflag)
                                    {

                                        js.ExecuteScript("document.querySelectorAll(\"button[name='Flag / Unflag']\")[0].click()");
                                        Invoke(new Action(() =>
                                        {
                                            createReportMessage(reports.messagesContainer, $"Message from : {mail} -> Flag action done", -1, -1, -1, mail);

                                        }));
                                        //flagUnflag = (bool)js.ExecuteScript("if(document.querySelectorAll(\"button[name='Flag / Unflag']\")[0] == null ){ return false ;  } else { return true } ");
                                    }

                                    break;

                                case 1:
                                    bool pinUnpin = (bool)js.ExecuteScript("if(document.querySelectorAll(\"button[name='Pin / Unpin']\")[0] == null ){ return false ;  } else { return true } ");

                                    if (pinUnpin)
                                    {

                                        js.ExecuteScript("document.querySelectorAll(\"button[name='Pin / Unpin']\")[0].click()");
                                        Invoke(new Action(() =>
                                        {
                                            createReportMessage(reports.messagesContainer, $"Message from : {mail} -> Pin action done", -1, -1, -1, mail);

                                        }));
                                        //pinUnpin = (bool)js.ExecuteScript("if(document.querySelectorAll(\"button[name='Pin / Unpin']\")[0] == null ){ return false ;  } else { return true } ");
                                    }

                                    break;

                                case 2:

                                    bool archive = (bool)js.ExecuteScript("if(document.querySelectorAll(\"button[aria-label = 'Archive']\")[0] == null ){ return false ;  } else { return true } ");

                                    if (archive)
                                    {
                                        js.ExecuteScript("document.querySelectorAll(\"button[aria-label = 'Archive']\")[0].click()");
                                        Invoke(new Action(() =>
                                        {
                                            createReportMessage(reports.messagesContainer, $"Message from : {mail} -> Arcive action done", -1, -1, -1, mail);

                                        }));
                                        //archive = (bool)js.ExecuteScript("if(document.querySelectorAll(\"button[aria-label = 'Archive']\")[0] == null ){ return false ;  } else { return true } ");
                                    }

                                    nextmmail--;
                                    isMailsExist = (string)js.ExecuteScript("window.mails = window.containere.querySelectorAll(\"div[draggable = 'true']\"); if(window.mails.length <= 0 ){ return '0' ; } else {  return String(window.mails.length) ; }   ");
                                    break;

                                case 3:

                                    break;
                            }

                            if (nextmmail == int.Parse(isMailsExist) - 1)
                            {
                                bool isVerticalScrollEnd = (bool)js.ExecuteScript("var scrollableDiv = document.getElementsByClassName('customScrollBar')[2]; var isVerticalScrollEnd = scrollableDiv.scrollTop + scrollableDiv.clientHeight >= scrollableDiv.scrollHeight; return isVerticalScrollEnd ");
                                if (!isVerticalScrollEnd)
                                {
                                    isMailsExist = (string)js.ExecuteScript("window.mails = window.containere.querySelectorAll(\"div[draggable = 'true']\"); if(window.mails.length <= 0 ){ return '0' ; } else {  return String(window.mails.length) ; }   ");
                                    nextmmail = 0;

                                    bool scrollBar = (bool)js.ExecuteScript("if(document.getElementsByClassName('customScrollBar')[2] == null ){ return false ;  } else { return true } ");

                                    while (!scrollBar)
                                    {
                                        scrollBar = (bool)js.ExecuteScript("if(document.getElementsByClassName('customScrollBar')[2] == null ){ return false ;  } else { return true } ");
                                    }

                                    js.ExecuteScript("var scrollableDiv = document.getElementsByClassName('customScrollBar')[2];scrollableDiv.scrollBy(0, scrollableDiv.clientHeight);");
                                }
                                else
                                {
                                    ListEnded = false;
                                }
                            }

                            nextmmail++;
                            Thread.Sleep(1000);
                        }
                    }

                    driver.Quit();
                }
            }
            catch (ElementNotSelectableException ex)
            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} ->   an element could  not be selected", 226, 44, 48, mail);

                }));
            //    drivers[driverLastIndex].Quit();

            }


            catch (NoSuchElementException ex)
            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail}  ->  an element not found ", 226, 44, 48, mail);

                }));
         //       drivers[driverLastIndex].Quit();
            }

            catch (ElementNotVisibleException ex)

            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail}  ->  an element could  not be selected", 226, 44, 48, mail);

                }));
           //     drivers[driverLastIndex].Quit();
            }
            catch (WebDriverException ex)

            {

                if (ex.Message.Contains("undefined"))

                {
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> Process done here", 29, 167, 105, mail);


                    }));
                    drivers[driverLastIndex].Quit();
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        createReportMessage(reports.messagesContainer, $"Message from : {mail} -> error accessing the link retry again please", 226, 44, 48, mail);

                    }));
                 //   drivers[driverLastIndex].Quit();

                }

            }

            catch (Exception ex)

            {
                Invoke(new Action(() =>
                {
                    createReportMessage(reports.messagesContainer, $"Message from : {mail} -> {ex.Message} \n try again or contact the developer", 226, 44, 48, mail);

                }));
            }

            }
            private void launcher(string process)
        {
            int howManyRounds = 0;
            int rowsCount = seeds.Rows.Count;
            for (int dr = 1; dr <= seeds.Rows.Count; dr++)
            {
                if (dr % 5 == 0)
                {
                    rowsCount -= 5;
                    howManyRounds++;
                }
            }

            if (howManyRounds < 1)
            {
                foreach (DataRow dr in seeds.Rows)
                {
                    if (process == "Archiving")
                    {
                        Thread inboxProcessThread = new Thread(() =>
                        {
                            inboxProcess(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                        });
                        inboxProcessThread.Start();
                    }
                    else if (process == "Not Junk")
                    {
                        Thread notJunk = new Thread(() =>
                        {
                            notJunkFunc(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                        });
                        notJunk.Start();
                    }
                    else
                    {
                        MessageBox.Show("Something goes wrong");
                    }
                }
            }
            else
            {
                for (int hmr = 0; hmr < howManyRounds; hmr++)
                {
                    List<Thread> thrs = new List<Thread>();
                    Thread inboxProcessThread = null;
                    Thread notJunk = null;
                    for (int hmb = hmr * 5; hmb < (hmr * 5) + 5; hmb++)
                    {
                        DataRow dr = seeds.Rows[hmb];

                        if (process == "Archiving")
                        {
                            inboxProcessThread = new Thread(() =>
                            {
                                inboxProcess(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                            });
                            thrs.Add(inboxProcessThread);
                            inboxProcessThread.Start();
                        }
                        else if (process == "Not Junk")
                        {
                            notJunk = new Thread(() =>
                            {
                                notJunkFunc(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
                            });
                            thrs.Add(notJunk);
                            notJunk.Start();
                        }
                        else
                        {
                            MessageBox.Show("Something goes wrong");
                        }
                    }

                    if (thrs.Count > 0)
                    {
                        foreach (Thread thr in thrs)
                        {
                            thr.Join();
                        }
                    }
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
                            createReportMessage(reports.messagesContainer, $"The not junk proccess starting....", -1, -1, -1);

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
                            createReportMessage(reports.messagesContainer, $"The inbox proccess starting....",-1,-1,-1);

                        }));
                        launcher("Archiving");
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
    }
}
