using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;


namespace HotmailReporting.Processes
{
    internal class CreateWebDriver
    {
        private Proxy proxy;
        public CreateWebDriver(Proxy proxy)
        {
            this.proxy = proxy;
        }


        public FirefoxDriver CreateFirefoxDriver()
        {

            FirefoxOptions options = new FirefoxOptions();

            options.Proxy = proxy;
            options.PageLoadStrategy = PageLoadStrategy.Normal;
       
        
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\WebDriver\bin\geckodriver.exe");
            service.HideCommandPromptWindow = true;
            //service, options
            FirefoxDriver driver = new FirefoxDriver(service ,options);
            driver.Manage().Window.Maximize();

            return driver;

        }

    }
}
