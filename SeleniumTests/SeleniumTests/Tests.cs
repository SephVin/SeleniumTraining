using System;
using System.Linq;
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
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
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
                product
                    .FindElements(By.CssSelector(".sticker"))
                    .Count
                    .Should()
                    .Be(1);
        }

        [Test]
        public void CountriesInAlphabeticOrderTest()
        {
            webDriver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            webDriver
                .FindElement(By.CssSelector("input[name ='username']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("input[name ='password']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("button[name='login']"))
                .Click();
            wait.Until(driver => driver.Title.Equals("Countries | My Store"));

            var countryNames = webDriver
                .FindElements(By.CssSelector("tr.row a:not([title=Edit])"))
                .Select(country => country.Text)
                .ToList();
            var expectedCountryNames = countryNames.OrderBy(x => x, StringComparer.Ordinal);

            countryNames.Should().BeEquivalentTo(expectedCountryNames);
        }

        [Test]
        public void ZonesOfCountriesInAlphabeticOrderTest()
        {
            webDriver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            webDriver
                .FindElement(By.CssSelector("input[name ='username']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("input[name ='password']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("button[name='login']"))
                .Click();
            wait.Until(driver => driver.Title.Equals("Countries | My Store"));

            var zoneUrls = webDriver
                .FindElements(By.XPath("//tr[contains(@class, 'row')]/td[6][not(contains(., '0'))]/../td[5]/a"))
                .Select(x => x.GetAttribute("href"))
                .ToList();

            foreach (var url in zoneUrls)
            {
                webDriver.Url = url;
                wait.Until(driver => driver.Title.Equals("Edit Country | My Store"));

                var zoneNames = webDriver
                    .FindElements(By.CssSelector("tr [name^='zones'][name$='[name]']"))
                    .Select(zone => zone.Text)
                    .ToList();
                var expectedZoneNames = zoneNames.OrderBy(x => x, StringComparer.Ordinal);

                zoneNames.Should().BeEquivalentTo(expectedZoneNames);
            }
        }

        [Test]
        public void GeoZonesInAlphabeticOrderTest()
        {
            webDriver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            webDriver
                .FindElement(By.CssSelector("input[name ='username']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("input[name ='password']"))
                .SendKeys("admin");
            webDriver
                .FindElement(By.CssSelector("button[name='login']"))
                .Click();
            wait.Until(driver => driver.Title.Equals("Geo Zones | My Store"));

            var geoZoneUrls = webDriver
                .FindElements(By.CssSelector("tr.row a:not([title='Edit'])"))
                .Select(zone => zone.GetAttribute("href"))
                .ToList();

            foreach (var url in geoZoneUrls)
            {
                webDriver.Url = url;
                wait.Until(driver => driver.Title.Equals("Edit Geo Zone | My Store"));

                var geoZoneNames = webDriver
                    .FindElements(By.CssSelector("td [name$='[zone_code]'] [selected]"))
                    .Select(zone => zone.Text)
                    .ToList();
                var expectedGeoZoneNames = geoZoneNames.OrderBy(x => x, StringComparer.Ordinal);

                geoZoneNames.Should().BeEquivalentTo(expectedGeoZoneNames);
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