using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace OpenCartTestDemo
{
    [TestFixture]
    public class ProductComparisonTest
    {
        IWebDriver driver;

        const string HOME_PAGE = "http://atqc-shop.epizy.com/";
        const string PPRODUCT_PC = "iMac";
        const string PRODUCT_PHONE = "iphone";
        const string PRODUCT_FIRST_PHONE = "iPhone";
        const string PRODUCT_SECOND_PHONE = "Apple iPhone SE 64GB";
        const string COMPARISON_MESSAGE = "Success: You have added iMac to your product comparison!\r\n×";
        const string REMOVE_MESSAGE = "You have not chosen any products to compare.";

        [OneTimeSetUp]
        public void OneTimeBeforeAllTests()
        {
            driver = new FirefoxDriver();//Launch browser.
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);//Implicit Wait 4 sec.
            driver.Manage().Window.Maximize();//Maximize window size.

            #region Other way for chrome
            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");//Without opening window.
            //options.AddArgument("--window-size=768,1366");//Set windows size : 1366x768.
            //driver = new ChromeDriver(options); 
            #endregion
        }

        [OneTimeTearDown]
        public void OneTimeAfterAllTests()
        {
            driver.Quit();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            driver.Navigate().GoToUrl(HOME_PAGE);
        }

        [TearDown]
        public void AfterEachTest()
        {
            //driver.Navigate().GoToUrl("http://atqc-shop.epizy.com/index.php?route=account/logout/");//Logout.
            driver.Manage().Cookies.DeleteAllCookies();//Clear cookies.
        }

        //Jira Test Case: https://ssu-jira.softserveinc.com/browse/CCCXXXVIII-660
        [Test]
        public void ProductComparison_ClickingTwoTimesCompareButton_OneProductAdded()
        {
            //Arrange
            string actual;
            int columnsCount;
            string actualComparisonMessage;

            //Act
            //Step #1 Verify OpenCard home page is opened. 
            driver.FindElement(By.XPath("//h3[text()='Featured']"));

            //Steps #2,#3 Find all desktops.
            driver.FindElement(By.LinkText("Desktops")).Click();
            driver.FindElement(By.LinkText("Show All Desktops")).Click();

            //Step #4 Add the product for comparison first time.
            driver.FindElement(By.CssSelector(".fa.fa-exchange")).Click();
            actualComparisonMessage = driver.FindElement(By.CssSelector(".alert.alert-success")).Text;
            //Verify comparison message.
            Assert.AreEqual(actualComparisonMessage, COMPARISON_MESSAGE, "An invalid comparison message is displayed.");

            //Step #5 Open product page.
            driver.FindElement(By.LinkText("iMac")).Click();

            //Step #6 Add the product for comparison second time.
            driver.FindElement(By.XPath("//div[@class='col-sm-4']//i[@class='fa fa-exchange']")).Click();

            //Step #7 Open 'product comparison' page.
            driver.FindElement(By.LinkText("product comparison")).Click();

            //Verify Comparative table and its cells are displayed
            driver.FindElement(By.CssSelector(".table.table-bordered"));
            driver.FindElement(By.XPath("//strong[text()='iMac']//ancestor::td"));
            actual = driver.FindElement(By.LinkText("iMac")).Text;
            columnsCount = driver.FindElements(By.XPath("//a[text()='Remove']")).Count;

            //Assert
            Assert.AreEqual(PPRODUCT_PC, actual, "The selected product was not added to the comparison table.");
            Assert.True(columnsCount == 1, "One product is added to the comparison table several times.");
        }

        //Jira Test Case: https://ssu-jira.softserveinc.com/browse/CCCXXXVIII-673
        [Test]
        public void ProductComparison_AddedPreviouslyProduct_RemovedFromComparison()
        {
            //Arrange
            string actual;
            int columnsCount;
            string actualComparisonMessage;
            string actualRemoveMessage;
            bool tableDisplayed;

            //Act
            //Step #1 Verify OpenCard home page is opened. 
            driver.FindElement(By.XPath("//h3[text()='Featured']"));

            //Steps #2,#3 Find all desktops.
            driver.FindElement(By.LinkText("Desktops")).Click();
            driver.FindElement(By.LinkText("Show All Desktops")).Click();

            //Step #4 Open product page.
            driver.FindElement(By.LinkText("iMac")).Click();

            //Step #5 Add the product for comparison.
            driver.FindElement(By.XPath("//div[@class='col-sm-4']//i[@class='fa fa-exchange']")).Click();
            actualComparisonMessage = driver.FindElement(By.CssSelector(".alert.alert-success")).Text;
            //Verify comparison message.
            Assert.AreEqual(actualComparisonMessage, COMPARISON_MESSAGE, "An invalid comparison message is displayed.");

            //Step #6 Open 'product comparison' page.
            driver.FindElement(By.LinkText("product comparison")).Click();

            //Verify Comparative table and its cells are displayed.
            driver.FindElement(By.CssSelector(".table.table-bordered"));
            driver.FindElement(By.XPath("//strong[text()='iMac']//ancestor::td"));
            actual = driver.FindElement(By.LinkText("iMac")).Text;
            Assert.AreEqual(PPRODUCT_PC, actual, "The selected product was not added to the comparison table.");

            //Step #7 Remove product from the comparison list.
            driver.FindElement(By.LinkText("Remove")).Click();
            columnsCount = driver.FindElements(By.XPath("//a[text()='Remove']")).Count;
            tableDisplayed = Methods.IsElementPresent(driver, By.CssSelector(".table.table-bordered"));
            actualRemoveMessage = driver.FindElement(By.CssSelector("#content>p")).Text;
            Console.WriteLine(actualRemoveMessage);

            //Assert
            Assert.True(columnsCount == 0, "A comparison table with at least one column " +
                "is present on the page after the product is removed.");
            Assert.False(tableDisplayed, "The comparison table is present on the page after the product is removed.");
            Assert.AreEqual(actualRemoveMessage, REMOVE_MESSAGE, "An invalid comparison message is displayed.");
        }

        //Jira Test Case: https://ssu-jira.softserveinc.com/browse/CCCXXXVIII-674
        [Test]
        public void ProductComparison_TwoDifferentProducts_AddedToComparisonTable()
        {
            //Arrange
            string actualFirst;
            string actualSecond;
            int columnsCount;        

            //Act
            //Step #1 Verify OpenCard home page is opened. 
            driver.FindElement(By.XPath("//h3[text()='Featured']"));

            //Steps #2,#3 Find all desktops.
            driver.FindElement(By.CssSelector(".form-control.input-lg")).Clear();
            driver.FindElement(By.CssSelector(".form-control.input-lg")).SendKeys("iphone");
            driver.FindElement(By.CssSelector("#search .btn.btn-default.btn-lg")).Click();

            //Steps #4,#5 Add the products for comparison.
            driver.FindElement(By.XPath("//a[text()='Apple iPhone SE 64GB']/../../../..//i[@class='fa fa-exchange']")).Click();
            driver.FindElement(By.XPath("//a[text()='iPhone']/../../../..//i[@class='fa fa-exchange']")).Click();
            #region Other way
            //driver.FindElement(By.XPath("//a[contains(@href,'id=65')]/../..//i[@class='fa fa-exchange']")).Click();
            //driver.FindElement(By.XPath("//a[contains(@href,'id=40')]/../..//i[@class='fa fa-exchange']")).Click(); 
            #endregion

            //Step #6 Open 'product comparison' page.
            driver.FindElement(By.LinkText("product comparison")).Click();

            //Verify Comparative table and its cells are displayed.
            driver.FindElement(By.CssSelector(".table.table-bordered"));
            driver.FindElement(By.XPath("//strong[text()='iPhone']//ancestor::td"));
            driver.FindElement(By.XPath("//strong[text()='Apple iPhone SE 64GB']//ancestor::td"));
            actualFirst = driver.FindElement(By.LinkText("iPhone")).Text;
            actualSecond = driver.FindElement(By.LinkText("Apple iPhone SE 64GB")).Text;
            columnsCount = driver.FindElements(By.XPath("//a[text()='Remove']")).Count;
            #region 
            ////Step #7 Remove product from the comparison list.
            //driver.FindElement(By.LinkText("Remove")).Click();
            //tableDisplayed = Methods.IsElementPresent(driver, By.CssSelector(".table.table-bordered"));
            //actualRemoveMessage = driver.FindElement(By.CssSelector("#content>p")).Text;
            //Console.WriteLine(actualRemoveMessage); 
            #endregion

            //Assert
            Assert.AreEqual(PRODUCT_FIRST_PHONE, actualFirst, "The selected product was not added to the comparison table.");
            Assert.AreEqual(PRODUCT_SECOND_PHONE, actualSecond, "The selected product was not added to the comparison table.");
            Assert.True(columnsCount == 2, "All or one of products isn't added to the comparison table.");
        }
    }
}
