using System;
using FluentAssertions;
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

        [Test]
        public void MenuSectionTest()
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
            wait.Until(driver => driver.FindElement(By.CssSelector(".notice.success")));

            var sectionsCount = webDriver
                .FindElements(By.CssSelector("li#app-"))
                .Count;

            for (var i = 0; i < sectionsCount; i++)
            {
                webDriver
                    .FindElements(By.CssSelector("li#app-"))[i]
                    .Click();

                IsElementPresent(By.CssSelector("td#content h1")).Should().BeTrue();

                var subSectionsCount = webDriver.FindElements(By.CssSelector("li#app- li")).Count;

                for (var j = 0; j < subSectionsCount; j++)
                {
                    webDriver
                        .FindElements(By.CssSelector("li#app- li"))[j]
                        .Click();

                    IsElementPresent(By.CssSelector("td#content h1")).Should().BeTrue();
                }
            }
        }

        [Test]
        public void ProductsStickerTest()
        {
            webDriver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.Equals("Online Store | My Store"));

            var products = webDriver.FindElements(By.CssSelector("li.product"));

            foreach (var product in products)
            {
                product
                    .FindElements(By.CssSelector(".sticker"))
                    .Count
                    .Should()
                    .Be(1);
            }
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
            webDriver = null;
        }

        private bool IsElementPresent(By locator)
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