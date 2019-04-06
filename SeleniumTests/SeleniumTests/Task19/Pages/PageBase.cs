using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Task19.TestSystem;

namespace SeleniumTests.Task19.Pages
{
    public class PageBase
    {
        protected Application application;
        protected IWebDriver webDriver;
        protected WebDriverWait waitDriver;

        public PageBase(Application application)
        {
            this.application = application;
            webDriver = application.Driver;
            waitDriver = application.WaitDriver;
        }

        public bool IsElementPresent(By locator)
        {
            try
            {
                webDriver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}