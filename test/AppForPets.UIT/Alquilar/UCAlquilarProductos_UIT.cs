using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace AppForPets.UIT.Alquilars
{
    public class UCAlquilarProductos_UIT : IDisposable
    {

        IWebDriver _driver;
        string _URI;

        public UCAlquilarProductos_UIT()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            var optionsff = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            var optionsie = new InternetExplorerOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            //For pipelines use this option
            //It doesnot show the browser
            //optionsc.AddArgument("--headless");
            //optionsff.AddArgument("--headless");
            //optionsie.AddArgument("--headless");

            string browser = "Chrome";
            //string browser = "Firefox";
            //string browser = "IE";
            switch (browser)
            {
                case "Chrome":
                    _driver = new ChromeDriver(optionsc);
                    break;
                case "Firefox":
                    _driver = new FirefoxDriver(optionsff);
                    break;
                //case "IE":
                //This driver is not working
                //    _driver = new InternetExplorerDriver(optionsie);
                //    break;
                default:
                    _driver = new ChromeDriver(optionsc);
                    break;
            }

            //Added to make ChromeDriver wait when an element is not found.
            //It will wait for a maximum of 50 seconds.
            //It has been added to wait for payment method options.
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
            //For pipelines this has to be set to 
            _URI = "https://localhost:5001/";

            initial_step_opening_the_web_page();

        }

        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }

        public void initial_step_opening_the_web_page()
        {
            _driver.Navigate()
                .GoToUrl(_URI);
        }

        public void precondition_perform_login()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("gregorio@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("APassword1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }


        private void first_step_accessing_alquilar()
        {
            _driver.FindElement(By.Id("AlquilarsController")).Click();

        }

        private void second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SelectProductosForAlquiler")).Click();
        }

        private void third_filter_producto_byTitle(string titleFilter)
        {
            _driver.FindElement(By.Id("productoNombre")).SendKeys(titleFilter);

            _driver.FindElement(By.Id("filterbyTitleTipoAnimal")).Click();
        }

        private void third_filter_producto_byGenre(string genreSelected)
        {

            var tipoAnimal = _driver.FindElement(By.Id("productoAnimalSelected"));

            //create select element object 
            SelectElement selectElement = new SelectElement(tipoAnimal);
            //select Action from the dropdown menu
            selectElement.SelectByText(genreSelected);

            _driver.FindElement(By.Id("filterbyTitleTipoAnimal")).Click();

        }

        private void fourth_select_productos_and_submit()
        {

            _driver.FindElement(By.Id("Producto_3")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }


        private void fourth_alternate_not_selecting_movies()
        {
            
            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fifth_fill_in_information_and_press_create(string quantityProducto1)
        {

            _driver.FindElement(By.Id("Movie_Quantity_3")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_3")).SendKeys(quantityProducto1);

            _driver.FindElement(By.Id("CreateButton")).Click();

        }

        [Fact]
        public void UC3_1_basic_flow()
        {
            //Arrange
            string[] expectedText = { "Details - AppForPets","Details",
                "Alquiler","Gregorio","Diaz","Pickup Date", "PrecioTotal","40","Rascador"};
            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();
            fourth_select_productos_and_submit();
            fifth_fill_in_information_and_press_create("2");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }


        [Fact]
        public void UC3_2_alternate_flow_1_NoProductosDisponibles()
        {
            //Arrange
            string expectedText = "No hay productos disponibles";

            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();

            var movieRow = _driver.FindElement(By.Id("NoProductos"));

            //checks the expected row exists
            Assert.NotNull(movieRow);
            Assert.Equal(expectedText, movieRow.Text);
        }

        [Fact]
        public void UC3_3_alternate_flow_2_filteringbyTitle()
        {
            //Arrange
            string[] expectedText = { "Rascador", "20", "Perro" };

            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();
            third_filter_producto_byTitle(expectedText[0]);

            var ProductoRow = _driver.FindElements(By.Id("Producto_Title_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(ProductoRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(ProductoRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC3_4_alternate_flow_2_filteringbyTipoAnimal()
        {
            //Arrange
            string[] expectedText = { "Rascador", "20", "Perro" };

            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();
            third_filter_producto_byGenre(expectedText[2]);

            //Assert            
            var movieRow = _driver.FindElements(By.Id("Producto_Title_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));

        }

        [Fact]
        public void UC3_5_alternate_flow_3_productosNotSelected()
        {
            //Arrange
            string expectedText = "Al menos debes seleccionar uno";

            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();
            fourth_alternate_not_selecting_movies();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrores")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        //public static IEnumerable<object[]> TC_UC1_6_UC1_10_alternate_flow_4_testingErrorsMandatorydata()
        //{
        //    var allTests = new List<object[]>
        //    {
        //        new object[] {"", "2", "2", "1234567890123456", "123", "12/12/2022", "Please, set your address for delivery"},
        //        new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "", "123", "12/12/2022", "The Credit Card field is required"},
        //        new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "1234567890123456", "", "12/12/2022", "The CCV field is required"},
        //        new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "1234567890123456", "123", "", "The ExpirationDate field is required"},
        //        new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "1234567890123456", "", "12/12/2022", "The CCV field is required"},
        //    };

        //    return allTests;
        //}

        [Theory]
        [InlineData("", "The CantidadAlquiler field is required.")]

        public void UC3_6_UC1_10_alternate_flow_4_testingErrorsMandatorydata(string quantityMovie1, string expectedText)
        {

            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();
            fourth_select_productos_and_submit();
            fifth_fill_in_information_and_press_create(quantityMovie1);

            //Assert
            //the expected error is shown in the view
            var errorShown = _driver.FindElements(By.TagName("span")).FirstOrDefault(l => l.Text.Contains(expectedText));
            Assert.NotNull(errorShown);

        }


        [Fact]
        public void UC3_11_alternate_flow_5_noSuficientesProductos()
        {
            //Arrange
            string expectedText = "No hay suficiente.";

            //Act
            precondition_perform_login();
            first_step_accessing_alquilar();
            second_step_accessing_link_Create_New();
            fourth_select_productos_and_submit();
            fifth_fill_in_information_and_press_create("400");

            //Assert
            var messageError = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, messageError.Substring(0, expectedText.Count()));

        }

    }

}


