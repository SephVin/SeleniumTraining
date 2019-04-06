using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Task19.Pages.Cart;
using SeleniumTests.Task19.Pages.Product;
using SeleniumTests.Task19.TestSystem;

namespace SeleniumTests.Task19.Pages.Main
{
    public class MainPage : PageBase
    {
        private readonly string title = "Online Store | My Store";
        private readonly string url = "http://localhost/litecart";

        private static By CartLinkLocator => By.CssSelector("#cart a.link");

        public MainPage(Application application) : base(application)
        {
        }

        public MainPage Open()
        {
            webDriver.Navigate().GoToUrl(url);
            waitDriver.Until(ExpectedConditions.TitleIs(title));

            return this;
        }

        public CartPage GoToCart()
        {
            webDriver
                .FindElement(CartLinkLocator)
                .Click();
            var cartPage = new CartPage(application);
            waitDriver.Until(ExpectedConditions.TitleIs(cartPage.Title));

            return cartPage;
        }

        public void AddFirstProductToCart(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var firstProduct = webDriver.FindElements(By.CssSelector("li.product"))[0];
                var productPage = OpenProductPage(firstProduct);

                productPage.AddProductToCart();

                webDriver.Url = "http://localhost/litecart/";
            }
        }

        private ProductPage OpenProductPage(IWebElement product)
        {
            product.Click();
            waitDriver.Until(ExpectedConditions.ElementExists(
                By.CssSelector("button[name='add_cart_product']")));

            return new ProductPage(application);
        }
    }
}