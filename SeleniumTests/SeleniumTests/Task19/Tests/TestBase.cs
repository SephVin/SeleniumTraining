using NUnit.Framework;
using SeleniumTests.Task19.TestSystem;

namespace SeleniumTests.Task19.Tests
{
    public class TestBase
    {
        public Application application;

        [SetUp]
        public void SetUp()
        {
            application = new Application();
        }

        [TearDown]
        public void TearDown()
        {
            application.Quit();
        }
    }
}