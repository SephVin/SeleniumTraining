using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Task19.TestSystem;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SeleniumTests.Task19.Pages.Product
{
    public class ProductPage : PageBase
    {
        public ProductPage(Application application) : base(application)
        {
        }

        private static By CartQuantityLocator => By.CssSelector("#cart .quantity");
        private static By AddToCartButtonLocator => By.CssSelector("button[name='add_cart_product']");
        private static By SizeSelectLocator => By.CssSelector("select[name='options[Size]']");
        
        private int CartQuantityValue => int.Parse(webDriver.FindElement(CartQuantityLocator).Text);

        public void AddProductToCart()
        {
            if (IsElementPresent(SizeSelectLocator))
            {
                var element = webDriver.FindElement(SizeSelectLocator);
                var selector = new SelectElement(element);
                selector.SelectByIndex(1);
            }

            webDriver
                .FindElement(AddToCartButtonLocator)
                .Click();
            waitDriver.Until(ExpectedConditions.TextToBePresentInElement(
                webDriver.FindElement(CartQuantityLocator),
                $"{CartQuantityValue + 1}"));
        }
    }
}