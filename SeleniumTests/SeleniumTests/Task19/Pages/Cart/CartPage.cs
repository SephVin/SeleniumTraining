using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using SeleniumTests.Task19.TestSystem;

namespace SeleniumTests.Task19.Pages.Cart
{
    public class CartPage : PageBase
    {
        public readonly string Title = "Checkout | My Store";

        public CartPage(Application application) : base(application)
        {
        }

        public int UniqueProductsInCart => webDriver
            .FindElements(By.CssSelector(".dataTable tr:not(.header) .item"))
            .Count;

        public string GetEmptyCartText()
        {
            waitDriver.Until(x => x.FindElement(By.CssSelector("div#content em")));
            return webDriver.FindElement(By.CssSelector("div#content em")).Text;
        }

        public void RemoveAllProducts()
        {
            if (UniqueProductsInCart > 1)
                RemoveProductFromCart(UniqueProductsInCart - 1);

            RemoveLastProductFromCart();
        }

        private void RemoveProductFromCart(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var uniqueProductsInCart = webDriver
                    .FindElements(By.CssSelector(".dataTable tr:not(.header) .item"))
                    .Count;
                webDriver
                    .FindElement(By.CssSelector("button[name='remove_cart_item']"))
                    .Click();
                waitDriver.Until(driver => driver
                    .FindElements(By.CssSelector(".dataTable tr:not(.header) .item"))
                    .Count
                    .Equals(uniqueProductsInCart - 1));
            }
        }

        private void RemoveLastProductFromCart()
        {
            webDriver
                .FindElement(By.CssSelector("button[name='remove_cart_item']"))
                .Click();
            waitDriver.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#content em")));
        }
    }
}