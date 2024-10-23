using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace AppForPets.UIT.ComprasProveedor
{
    public class UCCompraProveedor_UIT : IDisposable
    {

        IWebDriver _driver;
        string _URI;

        public UCCompraProveedor_UIT()
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

        [Fact]
        public void initial_step_opening_the_web_page()
        {
            string expectedNombre = "Página de inicio - AppForPets";
            string expectedText = "Register";

            _driver.Navigate().GoToUrl(_URI);

            Assert.Equal(expectedNombre, _driver.Title);
            Assert.Contains(expectedText, _driver.PageSource);
        }

        public void precondicion_loguearse()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");

            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("elena@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("Password1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }

        private void primer_paso_accediendo_comprasproveedor()
        {
            _driver.FindElement(By.Id("CompraProveedorB")).Click();

        }

        private void segundo_paso_accediendo_link_Crear_Nuevo()
        {
            _driver.FindElement(By.Id("Nuevacompra")).Click();
        }

        private void tercero_filtrar_proveedores_porNombre(string nombre)
        {
            _driver.FindElement(By.Id("nombreProveedor")).SendKeys(nombre);

            _driver.FindElement(By.Id("filtrar")).Click();
        }

        private void tercero_filtrar_proveedores_porDireccion(string direccion)
        {
            _driver.FindElement(By.Id("direccionSeleccionada")).SendKeys(direccion);

            _driver.FindElement(By.Id("filtrar")).Click();

        }

        private void cuarto_seleccionar_proveedor_y_siguiente()
        {

            _driver.FindElement(By.Id("Proveedor_1")).Click();

            _driver.FindElement(By.Id("siguiente")).Click();

        }

        private void cuarto_alternativo_proveedor_no_seleccionado()
        {

            _driver.FindElement(By.Id("siguiente")).Click();

        }

        private void quinto_filtrar_productos_por_nombre(string nombre)
        {
            _driver.FindElement(By.Id("NombreProducto")).SendKeys(nombre);

            _driver.FindElement(By.Id("filtrar")).Click();
        }


        private void quinto_filtrar_productos_por_tipoAnimal(string tipo)
        {
            var tipoA = _driver.FindElement(By.Id("tipoAnimalSelect"));

            //create select element object 
            SelectElement selectElement = new SelectElement(tipoA);
            //select Action from the dropdown menu
            selectElement.SelectByText(tipo);

            _driver.FindElement(By.Id("filtrar")).Click();

        }

        private void sexto_seleccionar_productos_y_siguiente()
        {

            _driver.FindElement(By.Id("Producto_1")).Click();
            _driver.FindElement(By.Id("Producto_7")).Click();

            _driver.FindElement(By.Id("siguiente")).Click();

        }

        private void sexto_alternativo_productos_no_seleccionados()
        {

            _driver.FindElement(By.Id("siguiente")).Click();

        }

        private void septimo_rellenar_informacion_y_presionar_comprar(string direccionEnvio, string cantidadProducto1, string cantidadProducto2, string telefono,
    string numTarjetaCredito, string CCV, string fechaCaducidad)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(direccionEnvio);

            _driver.FindElement(By.Id("ProductoProveedor_CantidadDisponible_1")).Clear();
            _driver.FindElement(By.Id("ProductoProveedor_CantidadDisponible_1")).SendKeys(cantidadProducto1);

            _driver.FindElement(By.Id("ProductoProveedor_CantidadDisponible_7")).Clear();
            _driver.FindElement(By.Id("ProductoProveedor_CantidadDisponible_7")).SendKeys(cantidadProducto2);

            _driver.FindElement(By.Id("TelefonoContacto")).Clear();
            _driver.FindElement(By.Id("TelefonoContacto")).SendKeys(telefono);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("PayPal_Email")).SendKeys(numTarjetaCredito);

            _driver.FindElement(By.Id("PayPal_Prefijo")).SendKeys(CCV);

            _driver.FindElement(By.Id("PayPal_Telefono")).SendKeys(fechaCaducidad);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }

        [Fact]
        public void UC1_1_flujo_basico()
        {
            //Arrange
            string[] expectedText = { "Detalles Compra - AppForPets", "Detalles",
                "CompraProveedor","Elena","Navarro","Fecha de la compra","Dirección de envío",
                "643254234", "Calle de la Universidad 1, Albacete, 02006, España",
                "PrecioTotal","42","Correa de cuello","Pienso P"};

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();
            sexto_seleccionar_productos_y_siguiente();
            septimo_rellenar_informacion_y_presionar_comprar("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "643254234", "luisa@gmail.com", "664", "313951");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        /*
        [Fact]
        public void UC1_2_flujo_alternativo_1_proveedoresNoDisponibles()
        {
            //Arrange
            string expectedText = "No hay proveedores disponibles";

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();

            //Assert
            var movieRow = _driver.FindElement(By.Id("NoProveedores"));

            //checks the expected row exists
            Assert.NotNull(movieRow);
            Assert.Equal(expectedText, movieRow.Text);
        }
        */

        [Fact]
        public void UC1_2_flujo_alternativo_1_filtrarProveedorPorNombre()
        {
            //Arrange
            string[] expectedText = { "Perrefersa", "Cuenca, C/Menor 57", "perrefersa@perrefersa.es", "612341234" };

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            tercero_filtrar_proveedores_porNombre(expectedText[0]);

            //Assert 
            var filaProveedor = _driver.FindElements(By.Id("nombreProv_" + expectedText[0]));

            Assert.NotNull(filaProveedor);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(filaProveedor.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC1_3_flujo_alternativo_1_filtrarProveedorPorDireccion()
        {
            //Arrange
            string[] expectedText = { "Perrefersa", "Cuenca, C/Menor 57", "perrefersa@perrefersa.es", "612341234" };

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            tercero_filtrar_proveedores_porDireccion(expectedText[1]);

            //Assert            
            var filaProveedor = _driver.FindElements(By.Id("nombreProv_" + expectedText[0]));

            Assert.NotNull(filaProveedor);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(filaProveedor.First(l => l.Text.Contains(expected)));

        }

        [Fact]
        public void UC1_4_flujo_alternativo_2_proveedorNoSeleccionado()
        {
            //Arrange
            string expectedText = "Debes seleccionar un proveedor para comprar";

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_alternativo_proveedor_no_seleccionado();

            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact(Skip = "Primero hay que ejecutar el script dbo.ProductoProveedor.Cantidad0 para actualizar la cantidad a 0")]
        public void UC1_5_flujo_alternativo_3_productosNoDisponibles()
        {
            //Arrange
            string expectedText = "No hay productos disponibles";

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();

            //Assert
            var movieRow = _driver.FindElement(By.Id("NoProductos"));

            //checks the expected row exists
            Assert.NotNull(movieRow);
            Assert.Equal(expectedText, movieRow.Text);
        }

        [Fact]
        public void UC1_6_flujo_alternativo_4_filtrarProductosPorNombre()
        {
            //Arrange
            string[] expectedText = { "Correa de cuello", "Perro", "Perrefersa", "3,00 €" };

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();
            quinto_filtrar_productos_por_nombre(expectedText[0]);

            //Assert 
            var filaProducto = _driver.FindElements(By.Id("nombreProd_" + expectedText[0]));

            Assert.NotNull(filaProducto);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(filaProducto.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC1_7_flujo_alternativo_4_filtrarProductosPorTipo()
        {
            //Arrange
            string[] expectedText = { "Correa de cuello", "Perro", "Perrefersa", "3,00 €" };

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();
            quinto_filtrar_productos_por_tipoAnimal(expectedText[1]);

            //Assert            
            var filaProducto = _driver.FindElements(By.Id("nombreProd_" + expectedText[0]));

            Assert.NotNull(filaProducto);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(filaProducto.First(l => l.Text.Contains(expected)));

        }

        [Fact]
        public void UC1_8_flujo_alternativo_5_productosNoSeleccionados()
        {
            //Arrange
            string expectedText = "Debes seleccionar al menos un producto";

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();
            sexto_alternativo_productos_no_seleccionados();

            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("", "2", "2", "643254234", "luisa@gmail.com", "664", "313951", "Inserta tu direccion para el envío")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "", "2", "643254234", "luisa@gmail.com", "664", "313951", "The Cantidad field is required.")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "", "luisa@gmail.com", "664", "313951", "Inserta un teléfono de contacto")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "643254234", "", "664", "313951", "The Email field is required.")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "643254234", "luisa@gmail.com", "", "313951", "The Prefijo field is required.")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "643254234", "luisa@gmail.com", "664", "", "The Telefono field is required.")]
        //[ClassData(typeof(PurchaseMoviesTestDataGenerator))]
        //[MemberData(nameof(TC_UC1_6_UC1_10_alternate_flow_4_testingErrorsMandatorydata))]

        public void UC1_9_UC1_14_flujo_alternativo_6_testeandoErroresMandandodatos(string direccionEnvio, string cantidadProducto1, string cantidadProducto2, string telefono,
            string numTarjetaCredito, string CCV, string fechaCaducidad, string expectedText)
        {

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();
            sexto_seleccionar_productos_y_siguiente();
            septimo_rellenar_informacion_y_presionar_comprar(direccionEnvio, cantidadProducto1, cantidadProducto2, telefono,
            numTarjetaCredito, CCV, fechaCaducidad);

            //Assert
            //the expected error is shown in the view
            var errorShown = _driver.FindElements(By.TagName("span")).FirstOrDefault(l => l.Text.Contains(expectedText));
            Assert.NotNull(errorShown);

        }

        
        [Fact]
        public void UC1_15_flujo_alternativo_7_sinSuficientesProductos()
        {
            //Arrange
            string expectedText = "No hay suficientes unidades de";

            //Act
            precondicion_loguearse();
            primer_paso_accediendo_comprasproveedor();
            segundo_paso_accediendo_link_Crear_Nuevo();
            cuarto_seleccionar_proveedor_y_siguiente();
            sexto_seleccionar_productos_y_siguiente();
            septimo_rellenar_informacion_y_presionar_comprar("Calle de la Universidad 1, Albacete, 02006, España", "200", "2", "643254234", "luisa@gmail.com", "664", "313951");

            //Assert
            var messageError = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, messageError.Substring(0, expectedText.Count()));

        }
        

    }
}
