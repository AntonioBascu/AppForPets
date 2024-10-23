using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.CompraViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForPets.UT.Controllers.ComprasController_test
{
    public class Compra_create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;

        public Compra_create_test()
        {
                                    /********************PREPARACION DE LAS PRUEBAS********************/

            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbAnimalesForTests(context);




            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            compraContext.User = identity;

        }
                                        /********************PRUEBAS DEL GET********************/

        [Fact]
        public async Task Create_Get_WithSelectedAnimales()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                String[] ids = new string[1] { "1" };
                SelectedAnimalesForCompraViewModel animales = new SelectedAnimalesForCompraViewModel() { IdsToAdd = ids };
                Animal expectedAnimal = Utilities.GetAnimales(0, 1).First();
                Cliente expectedCustomer = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<L_compraViewModel> expectedL_compra = new L_compraViewModel[1] {
                    new L_compraViewModel {cantidad=0, AnimalID = expectedAnimal.AnimalID, 
                        PrecioCompra = expectedAnimal.Precio, raza = expectedAnimal.Tipo.Raza} };
                CompraCreateViewModel expectedCompra = new CompraCreateViewModel { l_compras = expectedL_compra, Nombre = expectedCustomer.Nombre, PrimerApellido = expectedCustomer.PrimerApellido, SegundoApellido = expectedCustomer.SegundoApellido };

                // Act
                var result = controller.Create(animales);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;

                Assert.Equal(currentCompra, expectedCompra);

            }
        }
        [Fact]
        public async Task Create_Get_WithoutAnimales()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;
                Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedAnimalesForCompraViewModel animales = new SelectedAnimalesForCompraViewModel();

                CompraCreateViewModel expectedCompra = new CompraCreateViewModel
                {
                    Nombre = customer.Nombre,
                    PrimerApellido = customer.PrimerApellido,
                    SegundoApellido = customer.SegundoApellido,
                    l_compras = new List<L_compraViewModel>()
                };


                // Act
                var result = controller.Create(animales);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentCompra, expectedCompra);
                Assert.Equal("Debe seleccionar al menos un animal para comprar, por favor", error.ErrorMessage);
            }
        }

        /********************PRUEBAS DEL POST********************/
            
        public static IEnumerable<object[]> TestCasesForComprasCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Animal animal = Utilities.GetAnimales(0, 1).First();
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var payment1 = new PayPal { Email = customer.Email, Telefono = customer.PhoneNumber, Prefijo = "+34" };

            //Input values
            IList<L_compraViewModel> l_compraViewModel1 = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 10, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
            CompraCreateViewModel compra1 = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = l_compraViewModel1, DireccionEnvio = "Albacete", PayPal = payment1 };

            //Expected values
            IList<L_compraViewModel> expectedL_CompraViewModel1 = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 10, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
            CompraCreateViewModel expectedCompraVM1 = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = expectedL_CompraViewModel1, DireccionEnvio = "Albacete", PayPal = payment1 };
            string expetedErrorMessage1 = "no hay suficientes animales de la raza seleccionada";


            //Second error: Create_Post_WithQuantity0ForPurchase

            //Input values
            IList<L_compraViewModel> l_compraViewModel2 = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 0, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
            CompraCreateViewModel compra2 = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = l_compraViewModel1, DireccionEnvio = "Albacete", PayPal = payment1 };


            //expected values
            IList<L_compraViewModel> expectedL_CompraViewModel2 = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 0, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
            CompraCreateViewModel expectedCompraVM2 = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = expectedL_CompraViewModel2, DireccionEnvio = "Albacete", PayPal = payment1 };
            string expetedErrorMessage2 = "Seleccione al menos una animal para comprar o cancele su compra";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { compra1, l_compraViewModel1, null , payment1, expectedCompraVM1, expetedErrorMessage1 },
                new object[] { compra2, l_compraViewModel2, null , payment1, expectedCompraVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForComprasCreatePost_WithErrors))]
        public async Task Create_Post_WithErrors(CompraCreateViewModel compra, IList<L_compraViewModel> l_CompraViewModel, Tarjeta_Credito payment1, PayPal payment2, CompraCreateViewModel expectedCompraVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.CreatePost(compra, l_CompraViewModel, payment1, payment2);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedCompraVM, currentCompra);
                Assert.Equal(errorMessage, error.ErrorMessage);
                //Do not use, if in the entity purchase the method "Equal" has been defined for the purchase item list.
                //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));


            }

        }

        //[Fact]
        //public async Task Create_Post_WithoutEnoughAnimalesToBeCompra()
        //{
        //    using (context)
        //    {

        //        // Arrange
        //        var controller = new ComprasController(context);

        //        //simulate user's connection
        //        controller.ControllerContext.HttpContext = compraContext;
        //        Animal animal = Utilities.GetAnimales(0, 1).First();
        //        Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
        //        var payment1 = new PayPal { Email = customer.Email, Telefono = customer.PhoneNumber, Prefijo = "+34" };

        //        IList<L_compraViewModel> l_compraViewModel = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 10, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
        //        CompraCreateViewModel compra = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = l_compraViewModel, DireccionEnvio = "Albacete", PayPal = payment1 };

        //        IList<L_compraViewModel> expectedL_CompraViewModel = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 10, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
        //        CompraCreateViewModel expectedCompra = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = expectedL_CompraViewModel, DireccionEnvio = "Albacete", PayPal = payment1 };


        //        // Act
        //        var result = controller.CreatePost(compra, expectedL_CompraViewModel, null, payment1);

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result.Result);
        //        CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;

        //        var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
        //        Assert.Equal(expectedCompra, currentCompra);
        //        Assert.Equal("no hay suficientes animales de la raza golden, seleccione menor o igual que 4", error.ErrorMessage);
        //        //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));

        //    }
        //}

        //[Fact]
        //public async Task Create_Post_WithCantidad0ForCompra()
        //{
        //    using (context)
        //    {

        //        // Arrange
        //        var controller = new ComprasController(context);

        //        //simulate user's connection
        //        controller.ControllerContext.HttpContext = compraContext;
        //        Animal animal = Utilities.GetAnimales(0, 1).First();
        //        Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
        //        var payment1 = Utilities.GetMetodoPago(1, 1).First() as PayPal;

        //        IList<L_compraViewModel> l_compraViewModel = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 0, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
        //        CompraCreateViewModel compra = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = l_compraViewModel, DireccionEnvio = "Albacete", PayPal = payment1 };

        //        IList<L_compraViewModel> expectedL_CompraViewModel = new L_compraViewModel[1] { new L_compraViewModel { cantidad = 0, AnimalID = animal.AnimalID, raza = animal.Tipo.Raza, PrecioCompra = animal.Precio } };
        //        CompraCreateViewModel expectedCompra = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, l_compras = expectedL_CompraViewModel, DireccionEnvio = "Albacete", PayPal = payment1 };


        //        // Act
        //        var result = controller.CreatePost(compra, l_compraViewModel, null, payment1);

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result.Result);
        //        CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;

        //        var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
        //        Assert.Equal(expectedCompra, currentCompra);
        //        Assert.Equal($"Seleccione al menos una animal para comprar o cancele su compra", error.ErrorMessage);
        //        //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));

        //    }
        //}

        public static IEnumerable<object[]> TestCasesForCompraCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Compra expectedCompra1 = Utilities.GetCompras(0, 1).First();
            Cliente expectedCustomer1 = expectedCompra1.Cliente;
            var expectedPayment1 = expectedCompra1.MetodoPago as Tarjeta_Credito;
            L_Compra expectedL_Compra1 = expectedCompra1.L_Compras.First();
            int expectedQuantityForCompra1 = Utilities.GetAnimales(0, 1).First().Cantidad - expectedL_Compra1.Cantidad;
            IList<L_compraViewModel> l_compraViewModel1 = new L_compraViewModel[1] { new L_compraViewModel {
                    cantidad = expectedL_Compra1.Cantidad, AnimalID = expectedL_Compra1.AnimalID,
                    raza=expectedL_Compra1.Animal.Tipo.Raza,
                    PrecioCompra=expectedL_Compra1.Animal.Precio} };
            CompraCreateViewModel compra1 = new CompraCreateViewModel
            {
                Nombre = expectedCustomer1.Nombre,
                PrimerApellido = expectedCustomer1.PrimerApellido,
                SegundoApellido = expectedCustomer1.SegundoApellido,
                l_compras = l_compraViewModel1,
                DireccionEnvio = expectedCompra1.DirecionEnvio,
                MetodoPago = "CreditCard",
                TarjetaCredito = expectedPayment1
            };

            //Payment with Paypal
            Compra expectedCompra2 = Utilities.GetCompras(1, 1).First();
            expectedCompra2.CompraID = 1;
            expectedCompra2.L_Compras.First().ID = 1;
            expectedCompra2.L_Compras.First().CompraID = 1;
            L_Compra expectedL_Compra2 = expectedCompra2.L_Compras.First();
            int expectedQuantityForCompra2 = Utilities.GetAnimales(1, 1).First().Cantidad - expectedL_Compra2.Cantidad;
            var expectedPayment2 = expectedCompra2.MetodoPago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCustomer2 = expectedCompra2.Cliente;

            IList<L_compraViewModel> l_compraViewModel2 = new L_compraViewModel[1] { new L_compraViewModel {
                    cantidad = expectedL_Compra2.Cantidad, AnimalID = expectedL_Compra2.AnimalID,
                    raza=expectedL_Compra2.Animal.Tipo.Raza, 
                    PrecioCompra=expectedL_Compra2.Animal.Precio} };
            CompraCreateViewModel compra2 = new CompraCreateViewModel
            {
                Nombre = expectedCustomer2.Nombre,
                PrimerApellido = expectedCustomer2.PrimerApellido,
                SegundoApellido = expectedCustomer2.SegundoApellido,
                l_compras = l_compraViewModel2,
                DireccionEnvio = expectedCompra2.DirecionEnvio,
                MetodoPago = "PayPal",
                PayPal = expectedPayment2
            };

            var allTests = new List<object[]>
            {                  //Input values                                              // expected values
                new object[] { compra1, l_compraViewModel1, expectedPayment1, null, expectedCompra1, expectedQuantityForCompra1},
                new object[] { compra2, l_compraViewModel2, null, expectedPayment2, expectedCompra2, expectedQuantityForCompra2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForCompraCreatePost_WithoutErrors))]
        public async Task Create_Post_WithoutErrors(CompraCreateViewModel compra, IList<L_compraViewModel> l_comprasViewModel, Tarjeta_Credito payment1, PayPal payment2, Compra expectedCompra, int expectedQuantityForCompra)
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.CreatePost(compra, l_comprasViewModel, payment1, payment2);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualCompra = context.Compra.Include(p => p.L_Compras).
                                    FirstOrDefault(p => p.CompraID == expectedCompra.CompraID);
                Assert.Equal(expectedCompra, actualCompra);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedQuantityForCompra,
                    context.Animal.First(m => m.AnimalID == expectedCompra.L_Compras.First().AnimalID).Cantidad);


            }

        }

        //[Fact]
        //public async Task Create_Post_HavingEnoughCantidadWithPaypal()
        //{
        //    using (context)
        //    {
        //        // Arrange
        //        var controller = new ComprasController(context);

        //        //simulate user's connection
        //        controller.ControllerContext.HttpContext = compraContext;

        //        //create the expected results
        //        Compra expectedCompra = Utilities.GetCompras(1, 1).First();
        //        expectedCompra.CompraID = 1;
        //        expectedCompra.L_Compras.First().ID = 1;
        //        expectedCompra.L_Compras.First().CompraID = 1;
        //        L_Compra expectedL_Compra = expectedCompra.L_Compras.First();
        //        int expectedCantidadForCompra = Utilities.GetAnimales(1, 1).First().Cantidad - expectedL_Compra.Cantidad;
        //        var expectedMetodoPago = expectedCompra.MetodoPago as PayPal;
        //        expectedMetodoPago.ID = 1;
        //        Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;



        //        IList<L_compraViewModel> l_compras = new L_compraViewModel[1] { new L_compraViewModel {
        //            cantidad = 2, AnimalID = expectedL_Compra.AnimalID,
        //            raza=expectedL_Compra.Animal.Tipo.Raza,
        //            PrecioCompra=expectedL_Compra.Animal.Precio} };

        //        CompraCreateViewModel compra = new CompraCreateViewModel
        //        {
        //            Nombre = customer.Nombre,
        //            PrimerApellido = customer.PrimerApellido,
        //            SegundoApellido = customer.SegundoApellido,
        //            l_compras = l_compras,
        //            DireccionEnvio = expectedCompra.DirecionEnvio,
        //            MetodoPago = "PayPal",
        //            PayPal = expectedMetodoPago
        //        };


        //        //   IList<PurchaseItem> purchaseItems = new PurchaseItem[1] { new PurchaseItem { Id = 1, Quantity = 2, Movie = movie, MovieId = 1, PurchaseId = 1, Purchase = expectedPurchase } };
        //        //   expectedPurchase.PurchaseItems = purchaseItems;



        //        // Act
        //        var result = controller.CreatePost(compra, l_compras, null, expectedMetodoPago);

        //        //Assert
        //        var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

        //        //we should check the purchase has been created in the database
        //        var actualCompra = context.Compra.Include(p => p.L_Compras).FirstOrDefault(p => p.CompraID == 1);
        //        //By checking its attributes
        //        Assert.Equal(expectedCompra, actualCompra);

        //        //And that the movies associated to the quantity for purchase 
        //        //of each associated movie has been modified accordingly 
        //        Assert.Equal(expectedCantidadForCompra,
        //            context.Animal.Where(m => m.AnimalID ==
        //         expectedCompra.L_Compras.First().AnimalID).First().Cantidad);

        //    }

    }



}

