using FluentAssertions;
using NUnit.Framework;

namespace SeleniumTests.Task19.Tests
{
    public class Tests : TestBase
    {
        [Test]
        public void Task19()
        {
            var mainPage = application.MainPage.Open();
            mainPage.AddFirstProductToCart(3);

            var cartPage = mainPage.GoToCart();
            cartPage.RemoveAllProducts();
            cartPage.GetEmptyCartText()
                .Should()
                .Be("There are no items in your cart.");
        }
    }
}