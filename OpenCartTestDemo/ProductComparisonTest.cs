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
        const string PRODUCT_FIRST_PHONE = "";
        const string PRODUCT_SECOND_PHONE = "";

        [OneTimeSetUp]
        public void OneTimeBeforeAllTests()
        {
            driver = new FirefoxDriver();//Launch browser.
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);//Implicit Wait 10 sec.
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

        //Jira Test Case: https://ssu-jira.softserveinc.com/browse/CCCXXXVIII-660?filter=-2
        [Test]
        public void ProductComparison_ClickingTwoTimesCompareButton_OneProductAdded()
        {
            //Arrange
            string actual;
            int columnsCount;

            //Act
            //Verify OpenCard home page is opened.
            driver.FindElement(By.XPath("//h3[text()='Featured']")); //step #1

            //Find all desktops.
            driver.FindElement(By.LinkText("Desktops")).Click();//step #2
            driver.FindElement(By.LinkText("Show All Desktops")).Click();//step #3
            
            //Open product page.
            driver.FindElement(By.LinkText("iMac")).Click();//step #4
            
            //Add the product for comparison twice.
            driver.FindElement(By.XPath("//div[@class='col-sm-4']//i[@class='fa fa-exchange']")).Click();//step #5
            driver.FindElement(By.XPath("//div[@class='col-sm-4']//i[@class='fa fa-exchange']")).Click();//step #5
            
            //Open 'product comparison' page.
            driver.FindElement(By.LinkText("product comparison")).Click();//step #6
            
            //Verify Comparative table and its cells are displayed
            driver.FindElement(By.CssSelector(".table.table-bordered"));
            driver.FindElement(By.XPath("//strong[text()='iMac']//ancestor::td"));
            actual = driver.FindElement(By.LinkText(PPRODUCT_PC)).Text;
            columnsCount = driver.FindElements(By.XPath("//a[text()='Remove']")).Count;

            //Assert
            Assert.AreEqual(PPRODUCT_PC, actual, "The selected product was not added to the comparison table.");
            Assert.True(columnsCount == 1, "One product is added to the comparison table several times.");
        }

        //Jira Test Case: https://ssu-jira.softserveinc.com/browse/CCCXXXVIII-660?filter=-2
        [Test]
        public void ProductComparison_ClickingTwoTimesCompareButton_OneProductAdded()
        {
            //Arrange
            string actual;
            int columnsCount;

            //Act
            //Verify OpenCard home page is opened.
            driver.FindElement(By.XPath("//h3[text()='Featured']")); //step #1

            //Find all desktops.
            driver.FindElement(By.LinkText("Desktops")).Click();//step #2
            driver.FindElement(By.LinkText("Show All Desktops")).Click();//step #3

            //Open product page.
            driver.FindElement(By.LinkText("iMac")).Click();//step #4

            //Add the product for comparison twice.
            driver.FindElement(By.XPath("//div[@class='col-sm-4']//i[@class='fa fa-exchange']")).Click();//step #5
            driver.FindElement(By.XPath("//div[@class='col-sm-4']//i[@class='fa fa-exchange']")).Click();//step #5

            //Open 'product comparison' page.
            driver.FindElement(By.LinkText("product comparison")).Click();//step #6

            //Verify Comparative table and its cells are displayed
            driver.FindElement(By.CssSelector(".table.table-bordered"));
            driver.FindElement(By.XPath("//strong[text()='iMac']//ancestor::td"));
            actual = driver.FindElement(By.LinkText(PPRODUCT_PC)).Text;
            columnsCount = driver.FindElements(By.XPath("//a[text()='Remove']")).Count;

            //Assert
            Assert.AreEqual(PPRODUCT_PC, actual, "The selected product was not added to the comparison table.");
            Assert.True(columnsCount == 1, "One product is added to the comparison table several times.");
        }
    }
}
