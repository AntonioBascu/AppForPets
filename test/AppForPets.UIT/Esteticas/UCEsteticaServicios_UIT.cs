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

namespace AppForPets.UIT.Esteticas
{
    public class UCEsteticaServicios_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public UCEsteticaServicios_UIT()
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
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //For pipelines this has to be set to 
            _URI = "https://localhost:5001/";

            initial_step_opening_the_web_page();

        }

        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }


        [Fact]
        public void initial_step_opening_the_web_page()
        {
           
            //Act
            //El navegador cargará la URI indicada
            _driver.Navigate().GoToUrl(_URI);

        }



        public void precondition_perform_login()
        {
            _driver.Navigate().GoToUrl(_URI + "Identity/Account/Login");
            
            _driver.FindElement(By.Id("Input_Email")).SendKeys("gregorio@uclm.com");

            _driver.FindElement(By.Id("Input_Password")).SendKeys("APassword1234%");

            _driver.FindElement(By.Id("login-submit")).Click();

        }

        // Primer paso para comprar Servicios
        private void first_step_accessing_purchases()
        {
            _driver.FindElement(By.Id("Esteticas")).Click();

        }

        private void second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SelectServiciosForEstetica")).Click();
        }



        private void third_filter_servicios_byTiempoDuracion(string tiempoDuracionFiltro)
        {
            _driver.FindElement(By.Id("Tiempo_Duracion")).Clear();
            _driver.FindElement(By.Id("Tiempo_Duracion")).SendKeys(tiempoDuracionFiltro);

            _driver.FindElement(By.Id("filterbyTipoServicio")).Click();
        }


        private void third_filter_servicios_byNombre(string tipoSelected)
        {

            var tipoServicio = _driver.FindElement(By.Id("Nombre"));

            //create select element object 
            SelectElement selectElement = new SelectElement(tipoServicio);
            //select Action from the dropdown menu
            selectElement.SelectByText(tipoSelected);

            _driver.FindElement(By.Id("filterbyTipoServicio")).Click();

        }

        private void fourth_select_servicios_and_submit()
        {

            _driver.FindElement(By.Id("Servicio_1")).Click();
            _driver.FindElement(By.Id("Servicio_2")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }


        private void fourth_alternate_not_selecting_servicios()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }



        private void fifth_fill_in_information_and_press_create(string Direccion_correo, string telefono, string hora1, string hora2, string paypalEmail, string prefijo, string telf)
        {

            _driver.FindElement(By.Id("Direccion_correo")).SendKeys(Direccion_correo);
            _driver.FindElement(By.Id("telefono")).SendKeys(telefono);

            _driver.FindElement(By.Id("Servicio_Tiempo_Duracion_1")).SendKeys(hora1);
            _driver.FindElement(By.Id("Servicio_Tiempo_Duracion_2")).SendKeys(hora2);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("PayPal_Email")).SendKeys(paypalEmail);

            _driver.FindElement(By.Id("PayPal_Prefijo")).SendKeys(prefijo);

            _driver.FindElement(By.Id("PayPal_Telefono")).SendKeys(telf);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }


        [Fact]
        public void UC1_1_basic_flow()
        {
            //Arrange
            string[] expectedText = {"Estetica","Gregorio","Diaz", "35"};

            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            fourth_select_servicios_and_submit();
            fifth_fill_in_information_and_press_create("dianaalonsosaiz@gmail.com", "123456789","1","1", "dianaalonsosaiz@gmail.com", "34", "123456789");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }




        //[Fact(Skip = "Firs execute script dbo.Movie.Quantity0")]
        [Fact]
        public void UC1_2_alternate_flow_1_NoServiciosAvailable()
        {
            //Arrange
            string expectedText = "No hay servicios disponibles";

            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();

            var movieRow = _driver.FindElement(By.Id("NoServicios"));

            //checks the expected row exists
            Assert.NotNull(movieRow);
            Assert.Equal(expectedText, movieRow.Text);
        }


        [Fact]
        public void UC1_3_alternate_flow_2_filteringbyTiempoDuracion()
        {
            //Arrange
            string[] expectedText = { "Tratamientos", "Tratamiento Antiparasitos", "25,00 €", "2"};

            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            third_filter_servicios_byTiempoDuracion(expectedText[3]);

            var esteticaRow = _driver.FindElements(By.Id("Tipo_Servicio_Nombre_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(esteticaRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(esteticaRow.First(l => l.Text.Contains(expected)));
        }


        [Fact]
        public void UC1_4_alternate_flow_2_filteringbyTipoServicio()
        {
            //Arrange
            string[] expectedText = { "Tratamientos", "Tratamiento Antiparasitos", "25,00 €", "2" };

            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            third_filter_servicios_byNombre(expectedText[0]);

            //Assert            
            var movieRow = _driver.FindElements(By.Id("Tipo_Servicio_Nombre_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));

        }


        [Fact]
        public void UC1_5_alternate_flow_3_serviciosNotSelected()
        {
            //Arrange
            string expectedText = "Debes seleccionar al menos un servicio";

            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            fourth_alternate_not_selecting_servicios();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }


        [Theory]
        [InlineData("", "123456789", "1", "1", "dianaalonsosaiz@gmail.com", "34", "22740521", "Por favor, ponga su dirección de correo electrónico")]
        [InlineData("dianaalonsosaiz@gmail.com", "", "1", "1", "dianaalonsosaiz@gmail.com", "34", "22740521", "Por favor, ponga su teléfono de contacto")]
        [InlineData("dianaalonsosaiz@gmail.com", "123456789", "1", "1", "", "34", "22740521", "The Email field is required.")]
        [InlineData("dianaalonsosaiz@gmail.com", "123456789", "1", "1", "dianaalonsosaiz@gmail.com", "", "22740521", "The Prefijo field is required.")]
        [InlineData("dianaalonsosaiz@gmail.com", "123456789", "1", "1", "dianaalonsosaiz@gmail.com", "34", "", "The Telefono field is required.")]
        public void UC1_6_UC1_10_alternate_flow_4_testingErrorsMandatorydata(string Direccion_correo, string telefono, string hora1, string hora2, string paypalEmail, string prefijo, string telf, string expectedText)
        {

            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            fourth_select_servicios_and_submit();
            fifth_fill_in_information_and_press_create(Direccion_correo, telefono, hora1, hora2, paypalEmail, prefijo, telf);

            //Assert
            //the expected error is shown in the view
            var errorShown = _driver.FindElements(By.TagName("span")).FirstOrDefault(l => l.Text.Contains(expectedText));
            Assert.NotNull(errorShown);


        }






    }
}
