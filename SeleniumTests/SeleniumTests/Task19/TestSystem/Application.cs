using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Task19.Pages.Cart;
using SeleniumTests.Task19.Pages.Main;

namespace SeleniumTests.Task19.TestSystem
{
    public class Application
    {
        protected CartPage cartPage;
        protected IWebDriver driver;

        protected MainPage mainPage;
        protected WebDriverWait waitDriver;

        public Application()
        {
            driver = new ChromeDriver();
            waitDriver = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            mainPage = new MainPage(this);
            cartPage = new CartPage(this);
        }

        public MainPage MainPage => mainPage;

        public CartPage CartPage => cartPage;

        public IWebDriver Driver => driver;

        public WebDriverWait WaitDriver => waitDriver;

        public void Quit()
        {
            driver.Quit();
            driver = null;
        }
    }
}