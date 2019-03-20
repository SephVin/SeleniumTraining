using System;
using System.Collections.Generic;
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

        [Test]
        public void ProductPageIsCorrectTest()
        {
            webDriver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.Equals("Online Store | My Store"));

            var firstProduct = webDriver.FindElements(By.CssSelector("#box-campaigns li.product"))[0];
            var regularPriceOnMainPage = firstProduct.FindElement(By.CssSelector(".regular-price"));
            var campaignPriceOnMainPage = firstProduct.FindElement(By.CssSelector(".campaign-price"));

            var productNameOnMainPage = firstProduct
                .FindElement(By.CssSelector(".name"))
                .Text;
            var regularPriceTextOnMainPage = regularPriceOnMainPage.Text;
            var regularPriceTagNameOnMainPage = regularPriceOnMainPage.TagName;
            var regularPriceFrontSizeOnMainPage = regularPriceOnMainPage.Size.Height;
            var regularPriceColorOnMainPage = CleanUpColor(regularPriceOnMainPage.GetCssValue("color"));

            var campaignPriceTextOnMainPage = campaignPriceOnMainPage.Text;
            var campaignPriceTagNameOnMainPage = campaignPriceOnMainPage.TagName;
            var campaignPriceFrontSizeOnMainPage = campaignPriceOnMainPage.Size.Height;
            var campaignPriceColorOnMainPage = CleanUpColor(campaignPriceOnMainPage.GetCssValue("color"));

            firstProduct.Click();

            var regularPriceOnProductPage = webDriver.FindElement(By.CssSelector(".regular-price"));
            var campaignPriceOnProductPage = webDriver.FindElement(By.CssSelector(".campaign-price"));

            var productNameOnProductPage = webDriver
                .FindElement(By.CssSelector("h1.title"))
                .Text;
            var regularPriceTextOnProductPage = regularPriceOnProductPage.Text;
            var regularPriceTagNameOnProductPage = regularPriceOnProductPage.TagName;
            var regularPriceFrontSizeOnProductPage = regularPriceOnProductPage.Size.Height;
            var regularPriceColorOnProductPage = CleanUpColor(regularPriceOnProductPage.GetCssValue("color"));

            var campaignPriceTextOnProductPage = campaignPriceOnProductPage.Text;
            var campaignPriceTagNameOnProductPage = campaignPriceOnProductPage.TagName;
            var campaignPriceFrontSizeOnProductPage = campaignPriceOnProductPage.Size.Height;
            var campaignPriceColorOnProductPage = CleanUpColor(campaignPriceOnProductPage.GetCssValue("color"));


            productNameOnMainPage.Should().Be(productNameOnProductPage);
            regularPriceTextOnMainPage.Should().Be(regularPriceTextOnProductPage);
            campaignPriceTextOnMainPage.Should().Be(campaignPriceTextOnProductPage);

            regularPriceTagNameOnMainPage
                .Should()
                .Be(regularPriceTagNameOnProductPage)
                .And
                .Be("s");
            campaignPriceTagNameOnMainPage
                .Should()
                .Be(campaignPriceTagNameOnProductPage)
                .And
                .Be("strong");

            regularPriceColorOnMainPage
                .Distinct()
                .Count()
                .Should()
                .Be(1);
            regularPriceColorOnProductPage
                .Distinct()
                .Count()
                .Should()
                .Be(1);

            campaignPriceColorOnMainPage[1]
                .Should()
                .Be(campaignPriceColorOnMainPage[2])
                .And
                .Be("0");
            campaignPriceColorOnProductPage[1]
                .Should()
                .Be(campaignPriceColorOnProductPage[2])
                .And
                .Be("0");

            campaignPriceFrontSizeOnMainPage
                .Should()
                .BeGreaterThan(regularPriceFrontSizeOnMainPage);
            campaignPriceFrontSizeOnProductPage
                .Should()
                .BeGreaterThan(regularPriceFrontSizeOnProductPage);
        }

        [Test]
        public void CreateAccountTest()
        {
            webDriver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.Equals("Online Store | My Store"));

            webDriver.FindElement(By.CssSelector("form[name='login_form'] a")).Click();
            wait.Until(driver => driver.Title.Equals("Create Account | My Store"));

            var login = $"{Guid.NewGuid()}@test.ru";
            var password = "123456";

            Type(By.CssSelector("input[name='firstname']"), "John");
            Type(By.CssSelector("input[name='lastname']"), "Doe");
            Type(By.CssSelector("input[name='address1']"), "Address");
            Type(By.CssSelector("input[name='postcode']"), "12345");
            Type(By.CssSelector("input[name='city']"), "Ekb");
            SelectByValue(By.CssSelector("select[name='country_code']"), "US");

            wait.Until(driver =>
                driver.FindElement(By.CssSelector("select[name='zone_code']")).Enabled);

            SelectByValue(By.CssSelector("select[name='zone_code']"), "TX");
            Type(By.CssSelector("input[name='email']"), login);
            Type(By.CssSelector("input[name='phone']"), "+71234567890");
            Type(By.CssSelector("input[name='password']"), password);
            Type(By.CssSelector("input[name='confirmed_password']"), password);

            webDriver.FindElement(By.CssSelector("button[name='create_account']")).Click();

            wait.Until(driver => driver.Title.Equals("Online Store | My Store"));

            webDriver.FindElement(By.XPath("//div[@id='box-account']//a[contains(.,'Logout')]")).Click();

            Type(By.CssSelector("input[name='email']"), login);
            Type(By.CssSelector("input[name='password']"), password);

            webDriver.FindElement(By.CssSelector("button[name='login']")).Click();
            webDriver.FindElement(By.XPath("//div[@id='box-account']//a[contains(.,'Logout')]")).Click();
        }

        private void SelectByValue(By locator, string value)
        {
            var element = webDriver.FindElement(locator);
            var selector = new SelectElement(element);
            selector.SelectByValue(value);
        }

        private void Type(By locator, string text)
        {
            if (text != null)
            {
                webDriver.FindElement(locator).Clear();
                webDriver.FindElement(locator).SendKeys(text);
            }
        }

        private static List<string> CleanUpColor(string colorString)
        {
            var cleanColor = colorString
                .Replace("rgb(", "")
                .Replace("rgba(", "")
                .Replace(")", "")
                .Replace(" ", "")
                .Split(',')
                .ToList();

            if (cleanColor.Count == 4) cleanColor.RemoveAt(3);

            return cleanColor;
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