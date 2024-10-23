using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
//Needed to find elements in ICollection or IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace AppForPets.UIT.Compras
{
    public class UCCompraAnimales_UIT : IDisposable
    {
        //hace referencia al browser
        IWebDriver _driver;
        //hace referencia a la URI de la web de los test
        string _URI;

        public UCCompraAnimales_UIT()
        {
            //Opciones para cargar la página y aceptar certificados no seguros
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
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //For pipelines this has to be set to 
            _URI = "https://localhost:5001/";

            initial_step_opening_the_web_page();

        }

        void IDisposable.Dispose()
        {
            //To close and release all the resources allocated by the web driver
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

        private void first_step_accessing_compras()
        {
            _driver.FindElement(By.Id("ComprasController")).Click();

        }

        private void second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SelectAnimalesForCompra")).Click();
        }

        private void third_filter_animales_byRaza(string razaFilter)
        {
            _driver.FindElement(By.Id("TipoAnimal")).SendKeys(razaFilter);

            _driver.FindElement(By.Id("filterbyTipoAnimal")).Click();
        }

        private void third_filter_animales_byPrecio(string precioFilter)
        {
            _driver.FindElement(By.Id("precio")).Clear();
            _driver.FindElement(By.Id("precio")).SendKeys(precioFilter);

            _driver.FindElement(By.Id("filterbyTipoAnimal")).Click();

        }

        private void fourth_select_animales_and_submit()
        {

            _driver.FindElement(By.Id("Animal_1")).Click();
            _driver.FindElement(By.Id("Animal_2")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fourth_alternate_not_selecting_animales()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fifth_fill_in_information_and_press_create(string DireccionEnvio, string quantityAnimal1, string quantityAnimal2, string paypalEmail, string prefijo, string telefono)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(DireccionEnvio);

            _driver.FindElement(By.Id("Animal_Quantity_1")).Clear();
            _driver.FindElement(By.Id("Animal_Quantity_1")).SendKeys(quantityAnimal1);

            _driver.FindElement(By.Id("Animal_Quantity_2")).Clear();
            _driver.FindElement(By.Id("Animal_Quantity_2")).SendKeys(quantityAnimal2);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("PayPal_Email")).SendKeys(paypalEmail);

            _driver.FindElement(By.Id("PayPal_Prefijo")).SendKeys(prefijo);

            _driver.FindElement(By.Id("PayPal_Telefono")).SendKeys(telefono);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }

        [Fact]
        public void UC1_1_basic_flow()
        {
            //Arrange
            string[] expectedText = { "Details - AppForPets","Details",
                "Compra","Gregorio","Diaz","Fecha de Compra","Dirección envio",
                "Calle de la Universidad 1, Albacete, 02006, España",
                "Precio del Servicio","60","golden","borde collie"};
            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();
            fourth_select_animales_and_submit();
            fifth_fill_in_information_and_press_create("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "luisa@gmail.com", "664", "313951");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }
        [Fact (Skip ="se tiene que ejecutar antes el Script de cantidad0 para el correcto funcionamiento de la prueba")]
        public void UC1_2_alternate_flow_1_NoAnimalesDisponibles()
        {
            //Arrange
            string expectedText = "No hay animales disponibles";

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();

            var animalRow = _driver.FindElement(By.Id("NoAnimales"));

            //checks the expected row exists
            Assert.NotNull(animalRow);
            Assert.Equal(expectedText, animalRow.Text);
        }

        [Fact]
        public void UC1_3_alternate_flow_2_filteringbyRaza()
        {
            //Arrange
            string[] expectedText = { "golden", "10", "1", "1" };

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();
            third_filter_animales_byRaza(expectedText[0]);

            var animalRow = _driver.FindElements(By.Id("Animal_Raza_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(animalRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(animalRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC1_3_alternate_flow_2_filteringbyPrecio()
        {
            //Arrange
            string[] expectedText = { "golden", "10", "1", "1" };

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();
            third_filter_animales_byPrecio(expectedText[1]);

            var animalRow = _driver.FindElements(By.Id("Animal_Raza_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(animalRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(animalRow.First(l => l.Text.Contains(expected)));
        }


        [Fact]
        public void UC1_5_alternate_flow_3_animalesNotSelected()
        {
            //Arrange
            string expectedText = "Debes seleccionar al menos un aminal";

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();
            fourth_alternate_not_selecting_animales();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("", "2", "2", "luisa@gmail.com", "664", "313951", "Introduce tu dirección de envio")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "", "664", "313951", "The Email field is required")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "luisa@gmail.com", "", "313951", "The Prefijo field is required")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "luisa@gmail.com", "664", "", "The Telefono field is required")]
        //[ClassData(typeof(PurchaseMoviesTestDataGenerator))]
        //[MemberData(nameof(TC_UC1_6_UC1_10_alternate_flow_4_testingErrorsMandatorydata))]

        public void UC1_6_UC1_10_alternate_flow_4_testingErrorsMandatorydata(string DireccionEnvio, string quantityAnimal1, string quantityAnimal2, string paypalEmail, string prefijo, string telefono, string expectedText)
        {

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();
            fourth_select_animales_and_submit();
            fifth_fill_in_information_and_press_create(DireccionEnvio, quantityAnimal1, quantityAnimal2, paypalEmail, prefijo, telefono);
            _driver.FindElement(By.Id("r12")).Click();

            //Assert
            //the expected error is shown in the view
            var errorShown = _driver.FindElements(By.TagName("span")).FirstOrDefault(l => l.Text.Contains(expectedText));
            Assert.NotNull(errorShown);
        }


        [Fact]
        public void UC1_11_alternate_flow_5_noEnoughAnimales()
        {
            //Arrange
            string expectedText = "no hay suficientes animales de la raza seleccionada";

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            second_step_accessing_link_Create_New();
            fourth_select_animales_and_submit();
            fifth_fill_in_information_and_press_create("Calle de la Universidad 1, Albacete, 02006, España", "400", "2", "luisa@gmail.com", "664", "313951");

            //Assert
            var messageError = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, messageError.Substring(0, expectedText.Count()));

        }

    }
}
