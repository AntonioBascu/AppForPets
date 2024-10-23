using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppForPets.Models.EsteticaViewModels;
using AppForPets.UT.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;

namespace AppForPets.UT.Controllers.EsteticasController_test
{
    public class Estetica_Create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext esteticaContext;


        public Estetica_Create_test()
        {

            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbServiciosForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            esteticaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            esteticaContext.User = identity;
        }


        [Fact]
        public async Task Create_Get_WithSelectedServicios()
        {
            using (context)
            {

                // Arrange
                var controller = new EsteticasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = esteticaContext;

                String[] ids = new string[1] { "1" };
                SelectedServiciosForEsteticaViewModels servicios = new SelectedServiciosForEsteticaViewModels() { IdsToAdd = ids };
                Servicio expectedServicio = Utilities.GetServicios(0, 1).First();
                Cliente expectedCliente = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<EsteticaItemViewModel> expectedEsteticaItems = new EsteticaItemViewModel[1] {
                                new EsteticaItemViewModel {Tiempo_Duracion=0, ServicioId = expectedServicio.ServicioID, Nombre_Servicio = expectedServicio.Nombre_Servicio,
                                    PrecioCompra = expectedServicio.Precio_Servicio, Tipo_Servicio = expectedServicio.Tipo_Servicio.Nombre} };

                EsteticaCreateViewModel expectedPurchase = new EsteticaCreateViewModel
                {
                    EsteticaItems = expectedEsteticaItems,
                    Nombre = expectedCliente.Nombre,
                    Primer_Apellido = expectedCliente.PrimerApellido,
                    Segundo_Apellido = expectedCliente.SegundoApellido
                };

                // Act
                var result = controller.Create(servicios);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                EsteticaCreateViewModel currentPurchase = viewResult.Model as EsteticaCreateViewModel;

                Assert.Equal(currentPurchase, expectedPurchase);

            }
        }


        [Fact]
        public async Task Create_Get_WithoutServicios()
        {
            using (context)
            {

                // Arrange
                var controller = new EsteticasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = esteticaContext;
                Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedServiciosForEsteticaViewModels movies = new SelectedServiciosForEsteticaViewModels();

                EsteticaCreateViewModel expectedPurchase = new EsteticaCreateViewModel
                {
                    Nombre = cliente.Nombre,
                    Primer_Apellido = cliente.PrimerApellido,
                    Segundo_Apellido = cliente.SegundoApellido,
                    EsteticaItems = new List<EsteticaItemViewModel>()
                };


                // Act
                var result = controller.Create(movies);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                EsteticaCreateViewModel currentEsteticas = viewResult.Model as EsteticaCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentEsteticas, expectedPurchase);
                Assert.Equal("Por favor, selecciona al menos un servicio.", error.ErrorMessage);
            }
        }

        //        // ----------------------- POST -------------------------


        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Servicio servicio = Utilities.GetServicios(0, 1).First();
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            var payment1 = new PayPal { Email = cliente.Email, Telefono = cliente.PhoneNumber, Prefijo = "+34" };

            //Input values
            IList<EsteticaItemViewModel> esteticaViewModel1 = new EsteticaItemViewModel[1] { new EsteticaItemViewModel { Tiempo_Duracion = 0, ServicioId = servicio.ServicioID, Nombre_Servicio = servicio.Nombre_Servicio, Tipo_Servicio = servicio.Tipo_Servicio.Nombre, PrecioCompra = servicio.Precio_Servicio } };
            EsteticaCreateViewModel estetica1 = new EsteticaCreateViewModel { Nombre = cliente.Nombre, Primer_Apellido = cliente.PrimerApellido, Segundo_Apellido = cliente.SegundoApellido, EsteticaItems = esteticaViewModel1, Direccion_correo = "dianaalonsosaiz@gmail.com", PayPal = payment1 };

            //Expected values
            IList<EsteticaItemViewModel> expectedEsteticaItemsViewModel1 = new EsteticaItemViewModel[1] { new EsteticaItemViewModel { Tiempo_Duracion = 0, ServicioId = servicio.ServicioID, Nombre_Servicio = servicio.Nombre_Servicio, Tipo_Servicio = servicio.Tipo_Servicio.Nombre, PrecioCompra = servicio.Precio_Servicio } };
            EsteticaCreateViewModel expectedEsteticaVM1 = new EsteticaCreateViewModel { Nombre = cliente.Nombre, Primer_Apellido = cliente.PrimerApellido, Segundo_Apellido = cliente.SegundoApellido, EsteticaItems = expectedEsteticaItemsViewModel1, Direccion_correo = "dianaalonsosaiz@gmail.com", PayPal = payment1 };
            string expetedErrorMessage1 = "Por facor, selecciona al menos un servicio para comprar o cancele la compra";


            //Second error: Create_Post_WithQuantity0ForPurchase

            //Input values
            IList<EsteticaItemViewModel> esteticaItemsViewModel2 = new EsteticaItemViewModel[1] { new EsteticaItemViewModel { Tiempo_Duracion = 100, ServicioId = servicio.ServicioID, Nombre_Servicio = servicio.Nombre_Servicio, Tipo_Servicio = servicio.Tipo_Servicio.Nombre, PrecioCompra = servicio.Precio_Servicio } };
            EsteticaCreateViewModel estetica2 = new EsteticaCreateViewModel { Nombre = cliente.Nombre, Primer_Apellido = cliente.PrimerApellido, Segundo_Apellido = cliente.SegundoApellido, EsteticaItems = esteticaItemsViewModel2, Direccion_correo = "dianaalonsosaiz@gmail.com", PayPal = payment1 };

            //expected values
            IList<EsteticaItemViewModel> expectedEsteticaItemsViewModel2 = new EsteticaItemViewModel[1] { new EsteticaItemViewModel {Tiempo_Duracion=100, ServicioId = servicio.ServicioID, Nombre_Servicio = servicio.Nombre_Servicio, Tipo_Servicio = servicio.Tipo_Servicio.Nombre, PrecioCompra = servicio.Precio_Servicio } };
            EsteticaCreateViewModel expectedEsteticaVM2 = new EsteticaCreateViewModel { Nombre = cliente.Nombre, Primer_Apellido = cliente.PrimerApellido, Segundo_Apellido = cliente.SegundoApellido, EsteticaItems = expectedEsteticaItemsViewModel2, Direccion_correo = "dianaalonsosaiz@gmail.com", PayPal = payment1 };
            string expetedErrorMessage2 = "No hay suficiente tiempo para el servicio seleccionado";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { estetica1, esteticaViewModel1, null , payment1, expectedEsteticaVM1, expetedErrorMessage1 },
                new object[] { estetica2, esteticaItemsViewModel2, null , payment1, expectedEsteticaVM2, expetedErrorMessage2 }
            };
            return allTests;
        }


        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithErrors))]
        public async Task Create_Post_WithErrors(EsteticaCreateViewModel estetica, IList<EsteticaItemViewModel> esteticaItemsViewModel, Tarjeta_Credito payment1, PayPal payment2, EsteticaCreateViewModel expectedEsteticaVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new EsteticasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = esteticaContext;

                // Act
                var result = controller.CreatePost(estetica, esteticaItemsViewModel, payment1, payment2);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                EsteticaCreateViewModel currentEstetica = viewResult.Model as EsteticaCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedEsteticaVM, currentEstetica);
                Assert.Equal(errorMessage, error.ErrorMessage);
                //Do not use, if in the entity purchase the method "Equal" has been defined for the purchase item list.
                //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));

            }
        }


        public static IEnumerable<object[]> TestCasesForEsteticaCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Estetica expectedEstetica1 = Utilities.GetEsteticas(0, 1).First();
            Cliente expectedCustomer1 = expectedEstetica1.Cliente;
            var expectedPayment1 = expectedEstetica1.Metodo_Pago as Tarjeta_Credito;
            Linea_Servicio expectedEsteticaItem1 = expectedEstetica1.Linea_Servicios.First();
            IList<EsteticaItemViewModel> purchaseItemsViewModel1 = new EsteticaItemViewModel[1] { new EsteticaItemViewModel {
                    Tiempo_Duracion = expectedEsteticaItem1.Turno_Servicio, ServicioId = expectedEsteticaItem1.ServicioId,
                    Nombre_Servicio=expectedEsteticaItem1.Servicio.Nombre_Servicio, Tipo_Servicio=expectedEsteticaItem1.Servicio.Tipo_Servicio.Nombre,
                    PrecioCompra=expectedEsteticaItem1.Servicio.Precio_Servicio} };
            EsteticaCreateViewModel purchase1 = new EsteticaCreateViewModel
            {
                Nombre = expectedCustomer1.Nombre,
                Primer_Apellido = expectedCustomer1.PrimerApellido,
                Segundo_Apellido = expectedCustomer1.SegundoApellido,
                EsteticaItems = purchaseItemsViewModel1,
                Direccion_correo = expectedEstetica1.Direccion_correo,
                telefono = expectedEstetica1.telefono,
                Metodo_Pago = "Tarjeta_Credito",
                Tarjeta_Credito = expectedPayment1
            };

            //Payment with Paypal
            Estetica expectedEstetica2 = Utilities.GetEsteticas(1, 1).First();
            expectedEstetica2.EsteticaID = 1;
            expectedEstetica2.Linea_Servicios.First().LineaServicioID = 1;
            expectedEstetica2.Linea_Servicios.First().EsteticaId = 1;
            Linea_Servicio expectedEsteticaItem2 = expectedEstetica2.Linea_Servicios.First();
            var expectedPayment2 = expectedEstetica2.Metodo_Pago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCustomer2 = expectedEstetica2.Cliente;

            IList<EsteticaItemViewModel> purchaseItemsViewModel2 = new EsteticaItemViewModel[1] { new EsteticaItemViewModel {
                    Tiempo_Duracion = expectedEsteticaItem2.Turno_Servicio, ServicioId = expectedEsteticaItem2.ServicioId,
                    Nombre_Servicio=expectedEsteticaItem2.Servicio.Nombre_Servicio, Tipo_Servicio=expectedEsteticaItem2.Servicio.Tipo_Servicio.Nombre,
                    PrecioCompra=expectedEsteticaItem2.Servicio.Precio_Servicio} };
            EsteticaCreateViewModel purchase2 = new EsteticaCreateViewModel
            {
                Nombre = expectedCustomer2.Nombre,
                Primer_Apellido = expectedCustomer2.PrimerApellido,
                Segundo_Apellido = expectedCustomer2.SegundoApellido,
                EsteticaItems = purchaseItemsViewModel2,
                Direccion_correo = expectedEstetica2.Direccion_correo,
                telefono = expectedEstetica2.telefono,
                Metodo_Pago = "PayPal",
                PayPal = expectedPayment2
            };

            var allTests = new List<object[]>
            {                  //Input values                                              // expected values
                new object[] { purchase1, purchaseItemsViewModel1, expectedPayment1, null, expectedEstetica1},
                new object[] { purchase2, purchaseItemsViewModel2, null, expectedPayment2, expectedEstetica2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForEsteticaCreatePost_WithoutErrors))]
        public async Task Create_Post_WithoutErrors(EsteticaCreateViewModel estetica, IList<EsteticaItemViewModel> esteticaItemsViewModel, Tarjeta_Credito payment1, PayPal payment2, Estetica expectedEstetica)
        {
            using (context)
            {

                // Arrange
                var controller = new EsteticasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = esteticaContext;

                // Act
                var result = controller.CreatePost(estetica, esteticaItemsViewModel, payment1, payment2);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualEstetica = context.Estetica.Include(p => p.Linea_Servicios).
                                    FirstOrDefault(p => p.EsteticaID == expectedEstetica.EsteticaID);
                
                Assert.Equal(expectedEstetica, actualEstetica);

            }

        }



    }
}

