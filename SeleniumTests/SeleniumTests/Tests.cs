using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    public class Tests
    {
        private WebDriverWait wait;
        private IWebDriver webDriver;

        [SetUp]
        public void SetUp()
        {
            webDriver = new ChromeDriver();
            wait = new WebDriverWait(webDriver, TimeSpan.FromMilliseconds(10));
        }

        [Test]
        public void MyFirstTest()
        {
            webDriver.Url = "http://software-testing.ru";
        }

        [Test]
        public void LoginAsAdminTest()
        {
            webDriver.Url = "http://localhost/litecart/admin/login.php";
            wait.Until(driver => driver.Title.Equals("My Store"));

            webDriver
                .FindElement(By.CssSelector("input[name ='username']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("input[name ='password']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("button[name='login']"))
                .Click();
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
            webDriver = null;
        }
    }
}