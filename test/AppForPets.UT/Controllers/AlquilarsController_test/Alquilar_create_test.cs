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
using AppForPets.Models.AlquilarViewModel;
using AppForPets.UT.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;

namespace AppForPets.UT.Controllers.AlquilarsController_test
{
    public class Alquilar_create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext alquilarContext;

        public Alquilar_create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbProductosForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            alquilarContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            alquilarContext.User = identity;

        }

        [Fact]
        public async Task Create_Get_WithSelectedProductos()
        {
            using (context)
            {

                // Arrange
                var controller = new AlquilarsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = alquilarContext;

                String[] ids = new string[1] { "1" };
                SelectedProductosForAlquilerViewModel productos = new SelectedProductosForAlquilerViewModel() { IdsToAdd = ids };
                Producto expectedProducto = Utilities.GetProductos(0, 1).First();
                Cliente expectedCliente = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<AlquilarProductoViewModel> expectedAlquilarProducto = new AlquilarProductoViewModel[1] {
                    new AlquilarProductoViewModel {CantidadAlquiler=0, ProductoID = expectedProducto.ProductoID, NombreProducto = expectedProducto.NombreProducto,
                        PrecioAlquiler = expectedProducto.PrecioAlquiler, TipoAnimal = expectedProducto.TipoAnimal.NombreAnimal} };
                AlquilarCreateViewModel expectedAlquilar = new AlquilarCreateViewModel { AlquilarProductos = expectedAlquilarProducto, Nombre= expectedCliente.Nombre, PrimerApellido = expectedCliente.PrimerApellido, SegundoApellido = expectedCliente.SegundoApellido};

                // Act
                var result = controller.Create(productos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                AlquilarCreateViewModel currentAlquilar = viewResult.Model as AlquilarCreateViewModel;

                Assert.Equal(currentAlquilar, expectedAlquilar);

            }
        }
        [Fact]
        public async Task Create_Get_WithoutProducto()
        {
            using (context)
            {

                // Arrange
                var controller = new AlquilarsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = alquilarContext;
                Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedProductosForAlquilerViewModel productos = new SelectedProductosForAlquilerViewModel();

                AlquilarCreateViewModel expectedAlquilar = new AlquilarCreateViewModel
                {
                    Nombre = cliente.Nombre,
                    PrimerApellido = cliente.PrimerApellido,
                    SegundoApellido = cliente.SegundoApellido,
                    AlquilarProductos = new List<AlquilarProductoViewModel>()
                };


                // Act
                var result = controller.Create(productos);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                AlquilarCreateViewModel currentAlquilar = viewResult.Model as AlquilarCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentAlquilar, expectedAlquilar);
                Assert.Equal("Debes seleccionar al menos un producto para alquilar, por favor", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForAlquilerCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Producto producto = Utilities.GetProductos(0, 1).First();
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
           

            //Input values
            IList<AlquilarProductoViewModel> alquilarProductosViewModel1 = new AlquilarProductoViewModel[1] { new AlquilarProductoViewModel{ CantidadAlquiler = 10, ProductoID = producto.ProductoID, NombreProducto = producto.NombreProducto, TipoAnimal = producto.TipoAnimal.NombreAnimal, PrecioAlquiler = producto.PrecioAlquiler } };
            AlquilarCreateViewModel alquilar1 = new AlquilarCreateViewModel { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, AlquilarProductos = alquilarProductosViewModel1 };

            //Expected values
            IList<AlquilarProductoViewModel> expectedAlquilarProductosViewModel1 = new AlquilarProductoViewModel[1] { new AlquilarProductoViewModel { CantidadAlquiler = 10, ProductoID = producto.ProductoID, NombreProducto = producto.NombreProducto, TipoAnimal = producto.TipoAnimal.NombreAnimal, PrecioAlquiler = producto.PrecioAlquiler } };
            AlquilarCreateViewModel expectedAlquilarVM1 = new AlquilarCreateViewModel { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, AlquilarProductos = expectedAlquilarProductosViewModel1};
            string expetedErrorMessage1 = "No hay suficiente.";

            //Second error: Create_Post_WithQuantity0ForPurchase

            //Input values
            IList<AlquilarProductoViewModel> alquilarProductosViewModel2 = new AlquilarProductoViewModel[1] { new AlquilarProductoViewModel { CantidadAlquiler = 0, ProductoID = producto.ProductoID, NombreProducto = producto.NombreProducto, TipoAnimal = producto.TipoAnimal.NombreAnimal, PrecioAlquiler = producto.PrecioAlquiler } };
            AlquilarCreateViewModel alquilar2 = new AlquilarCreateViewModel { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, AlquilarProductos = alquilarProductosViewModel2};

            //expected values
            IList<AlquilarProductoViewModel> expectedAlquilarProductosViewModel2 = new AlquilarProductoViewModel[1] { new AlquilarProductoViewModel { CantidadAlquiler = 10, ProductoID = producto.ProductoID, NombreProducto = producto.NombreProducto, TipoAnimal = producto.TipoAnimal.NombreAnimal, PrecioAlquiler = producto.PrecioAlquiler } };
            AlquilarCreateViewModel expectedAlquilarVM2 = new AlquilarCreateViewModel { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, AlquilarProductos = expectedAlquilarProductosViewModel2};
            string expetedErrorMessage2 = "Please select at least a producto to be alquilado or cancel your alquiler";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { alquilar1, alquilarProductosViewModel1, expectedAlquilarVM1, expetedErrorMessage1 },
                new object[] { alquilar2, alquilarProductosViewModel2, expectedAlquilarVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForAlquilerCreatePost_WithErrors))]
        public async Task Create_Post_WithErrors(AlquilarCreateViewModel alquilar, IList<AlquilarProductoViewModel> alquilarProductoViewModels, AlquilarCreateViewModel expectedAlquilarVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new AlquilarsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = alquilarContext;

                // Act
                var result = controller.CreatePost(alquilar, alquilarProductoViewModels);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                AlquilarCreateViewModel currentAlquiler = viewResult.Model as AlquilarCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedAlquilarVM, currentAlquiler);
                Assert.Equal(errorMessage, error.ErrorMessage);
                //Do not use, if in the entity purchase the method "Equal" has been defined for the purchase item list.
                //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));

            }

        }

        /*
        [Fact]
        public async Task Create_Post_WithoutEnoughMoviesToBePurchased()
        {
            using (context)
            {

                // Arrange
                var controller = new PurchasesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;
                Movie movie = Utilities.GetMovies(0, 1).First();
                Customer customer = Utilities.GetUsers(0,1).First() as Customer;
                var payment1 = new PayPal { Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

                IList<PurchaseItemViewModel> purchaseItemsViewModel = new PurchaseItemViewModel[1] { new PurchaseItemViewModel { Quantity = 10, MovieID = movie.MovieID, Title = movie.Title, Genre = movie.Genre.Name, PriceForPurchase = movie.PriceForPurchase } };
                PurchaseCreateViewModel purchase = new PurchaseCreateViewModel { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname, PurchaseItems = purchaseItemsViewModel, DeliveryAddress = "Albacete", PayPal = payment1 };

                IList<PurchaseItemViewModel> expectedPurchaseItemsViewModel = new PurchaseItemViewModel[1] { new PurchaseItemViewModel { Quantity = 10, MovieID = movie.MovieID, Title = movie.Title, Genre = movie.Genre.Name, PriceForPurchase = movie.PriceForPurchase } };
                PurchaseCreateViewModel expectedPurchase = new PurchaseCreateViewModel { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname , PurchaseItems = expectedPurchaseItemsViewModel, DeliveryAddress = "Albacete", PayPal = payment1 };

                // Act
                var result = controller.CreatePost(purchase, purchaseItemsViewModel, null, payment1);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                PurchaseCreateViewModel currentPurchase = viewResult.Model as PurchaseCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedPurchase, currentPurchase);
                Assert.Equal("There are no enough movies titled The lord of the rings, please select less or equal than 5", error.ErrorMessage);
             //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));

            }
        }
        

        
        [Fact]
        public async Task Create_Post_WithQuantity0ForPurchase()
        {
            using (context)
            {

                // Arrange
                var controller = new PurchasesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;
                Movie movie = Utilities.GetMovies(0, 1).First();
                Customer customer = Utilities.GetUsers(0, 1).First() as Customer;
                var payment1 = Utilities.GetPaymentMethod(1,1).First() as PayPal;

                IList<PurchaseItemViewModel> purchaseItemsViewModel = new PurchaseItemViewModel[1] { new PurchaseItemViewModel { Quantity = 0, MovieID = movie.MovieID, Title = movie.Title, Genre = movie.Genre.Name, PriceForPurchase = movie.PriceForPurchase } };
                PurchaseCreateViewModel purchase = new PurchaseCreateViewModel { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname, PurchaseItems = purchaseItemsViewModel, DeliveryAddress = "Albacete", PayPal = payment1 };

                IList<PurchaseItemViewModel> expectedPurchaseItems = new PurchaseItemViewModel[1] { new PurchaseItemViewModel { Quantity = 0, MovieID = movie.MovieID, Title = movie.Title, Genre = movie.Genre.Name, PriceForPurchase = movie.PriceForPurchase } };
                PurchaseCreateViewModel expectedPurchase = new PurchaseCreateViewModel { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname, PurchaseItems = expectedPurchaseItems, DeliveryAddress = "Albacete", PayPal = payment1 };

                // Act
                var result = controller.CreatePost(purchase, purchaseItemsViewModel, null, payment1);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                PurchaseCreateViewModel currentPurchase = viewResult.Model as PurchaseCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();//["MovieForPurchase0"].Errors.FirstOrDefault();
                Assert.Equal(expectedPurchase, currentPurchase);
                Assert.Equal($"Please select at least a movie to be bought or cancel your purchase", error.ErrorMessage);
             //   Assert.Equal(currentPurchase.PurchaseItems[0].Movie, expectedPurchaseItems[0].Movie, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));

            }
        }
        */

        public static IEnumerable<object[]> TestCasesForAlquilerCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Alquilar expectedAlquilar1 = Utilities.GetAlquilars(0, 1).First();
            Cliente expectedCliente1 = expectedAlquilar1.Cliente;
            AlquilarProductos expectedAlquilarProducto1 = expectedAlquilar1.AlquilarProductos.First();
            int expectedCantidadAlquilar1 = Utilities.GetProductos(0, 1).First().CantidadAlquilar- expectedAlquilarProducto1.Cantidad;
            IList<AlquilarProductoViewModel> alquilarProductoViewModel1= new AlquilarProductoViewModel[1] { new AlquilarProductoViewModel{
                    CantidadAlquiler = expectedAlquilarProducto1.Cantidad, ProductoID = expectedAlquilarProducto1.Producto.ProductoID,
                    NombreProducto=expectedAlquilarProducto1.Producto.NombreProducto, TipoAnimal=expectedAlquilarProducto1.Producto.TipoAnimal.NombreAnimal,
                    PrecioAlquiler=expectedAlquilarProducto1.Producto.PrecioAlquiler} };
            AlquilarCreateViewModel alquilar1= new AlquilarCreateViewModel
            {
                Nombre = expectedCliente1.Nombre,
                PrimerApellido = expectedCliente1.PrimerApellido,
                SegundoApellido = expectedCliente1.SegundoApellido,
                AlquilarProductos = alquilarProductoViewModel1
            };

            //Payment with Paypal
            Alquilar expectedAlquilar2 = Utilities.GetAlquilars(1, 1).First();
            expectedAlquilar2.AlquilarID = 1;
            expectedAlquilar2.AlquilarProductos.First().Id = 1;
            expectedAlquilar2.AlquilarProductos.First().Alquilar.AlquilarID = 1;
            AlquilarProductos expectedAlquilarProducto2 = expectedAlquilar2.AlquilarProductos.First();
            int expectedCantidadAlquilar2 = Utilities.GetProductos(1, 1).First().CantidadAlquilar- expectedAlquilarProducto2.Cantidad;
            Cliente expectedCliente2 = expectedAlquilar2.Cliente;

            IList<AlquilarProductoViewModel> alquilarProductoViewModel2= new AlquilarProductoViewModel[1] { new AlquilarProductoViewModel{
                    CantidadAlquiler = expectedAlquilarProducto2.Cantidad, ProductoID= expectedAlquilarProducto2.Producto.ProductoID,
                    NombreProducto=expectedAlquilarProducto2.Producto.NombreProducto, TipoAnimal=expectedAlquilarProducto2.Producto.TipoAnimal.NombreAnimal,
                    PrecioAlquiler=expectedAlquilarProducto2.Producto.PrecioAlquiler} };
            AlquilarCreateViewModel alquilar2= new AlquilarCreateViewModel
            {
                Nombre = expectedCliente2.Nombre,
                PrimerApellido = expectedCliente2.PrimerApellido,
                SegundoApellido = expectedCliente2.SegundoApellido,
                AlquilarProductos = alquilarProductoViewModel2
            };

            var allTests = new List<object[]>
            {                  //Input values                                              // expected values
                new object[] { alquilar1, alquilarProductoViewModel1, expectedAlquilar1, expectedCantidadAlquilar1},
                new object[] { alquilar2, alquilarProductoViewModel2, expectedAlquilar2, expectedCantidadAlquilar2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForAlquilerCreatePost_WithoutErrors))]
        public async Task Create_Post_WithoutErrors(AlquilarCreateViewModel alquilar, IList<AlquilarProductoViewModel> alquilarProductoViewModels, Alquilar expectedAlquilar, int expectedCantidadAlquiler)
        {
            using (context)
            {

                // Arrange
                var controller = new AlquilarsController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = alquilarContext;

                // Act
                var result = controller.CreatePost(alquilar, alquilarProductoViewModels);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualAlquilar = context.Alquilar.Include(p => p.AlquilarProductos).
                                    FirstOrDefault(p => p.AlquilarID == expectedAlquilar.AlquilarID);

                
                Assert.Equal(expectedAlquilar, actualAlquilar);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedCantidadAlquiler,
                    context.Producto.First(m => m.ProductoID== expectedAlquilar.AlquilarProductos.First().Producto.ProductoID).CantidadAlquilar);

            }

        }

        /*
        [Fact]
        public async Task Create_Post_HavingEnoughQuantityWithCreditCard()
        {
            using (context)
            {

                // Arrange
                var controller = new PurchasesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                Purchase expectedPurchase = Utilities.GetPurchases(0, 1).First();
                Customer expectedCustomer = expectedPurchase.Customer;
                var expectedPayment = expectedPurchase.PaymentMethod as CreditCard;
                PurchaseItem expectedPurchaseItem = expectedPurchase.PurchaseItems.First();
                int expectedQuantityForPurchase = Utilities.GetMovies(0, 1).First().QuantityForPurchase - expectedPurchaseItem.Quantity;

                IList<PurchaseItemViewModel> purchaseItems = new PurchaseItemViewModel[1] { new PurchaseItemViewModel {
                    Quantity = expectedPurchaseItem.Quantity, MovieID = expectedPurchaseItem.MovieId,
                    Title=expectedPurchaseItem.Movie.Title, Genre=expectedPurchaseItem.Movie.Genre.Name,
                    PriceForPurchase=expectedPurchaseItem.Movie.PriceForPurchase} };
                PurchaseCreateViewModel purchase = new PurchaseCreateViewModel
                {
                    Name = expectedCustomer.Name,
                    FirstSurname = expectedCustomer.FirstSurname,
                    SecondSurname = expectedCustomer.SecondSurname,
                    PurchaseItems = purchaseItems,
                    DeliveryAddress = expectedPurchase.DeliveryAddress,
                    PaymentMethod = "CreditCard",
                    CreditCard = expectedPayment
                };

                // Act
                var result = controller.CreatePost(purchase, purchaseItems, expectedPayment, null);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualPurchase = context.Purchase.Include(p => p.PurchaseItems).
                    FirstOrDefault(p => p.PurchaseId == expectedPurchase.PurchaseId);
                Assert.Equal(expectedPurchase, actualPurchase);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedQuantityForPurchase,
                    context.Movie.First(m => m.MovieID == expectedPurchaseItem.MovieId).QuantityForPurchase);

            }

        }

        [Fact]
        public async Task Create_Post_HavingEnoughQuantityWithPaypal()
        {
            using (context)
            {

                // Arrange
                var controller = new PurchasesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                //create the expected results
                Purchase expectedPurchase = Utilities.GetPurchases(1, 1).First();
                expectedPurchase.PurchaseId = 1;
                expectedPurchase.PurchaseItems.First().Id = 1;
                expectedPurchase.PurchaseItems.First().PurchaseId = 1;
                PurchaseItem expectedPurchaseItem = expectedPurchase.PurchaseItems.First();
                int expectedQuantityForPurchase = Utilities.GetMovies(1, 1).First().QuantityForPurchase - expectedPurchaseItem.Quantity;
                var expectedPayment = expectedPurchase.PaymentMethod as PayPal;
                expectedPayment.ID = 1;
                Customer expectedCustomer = expectedPurchase.Customer;              

                IList<PurchaseItemViewModel> purchaseItems = new PurchaseItemViewModel[1] { new PurchaseItemViewModel {
                    Quantity = expectedPurchaseItem.Quantity, MovieID = expectedPurchaseItem.MovieId,
                    Title=expectedPurchaseItem.Movie.Title, Genre=expectedPurchaseItem.Movie.Genre.Name,
                    PriceForPurchase=expectedPurchaseItem.Movie.PriceForPurchase} };
                PurchaseCreateViewModel purchase = new PurchaseCreateViewModel { Name = expectedCustomer.Name, 
                    FirstSurname = expectedCustomer.FirstSurname, SecondSurname = expectedCustomer.SecondSurname, 
                    PurchaseItems = purchaseItems, DeliveryAddress = expectedPurchase.DeliveryAddress, 
                    PaymentMethod = "PayPal", PayPal = expectedPayment };

                // Act
                var result = controller.CreatePost(purchase, purchaseItems, null, expectedPayment);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                // we should check it is redirected to details
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualPurchase = context.Purchase.Include(p=>p.PurchaseItems)
                    .FirstOrDefault(p=>p.PurchaseId==expectedPurchase.PurchaseId);
                //By checking its attributes
                Assert.Equal(expectedPurchase, actualPurchase);

                //And that the movies associated to the quantity for purchase 
                //of each associated movie has been modified accordingly 
               Assert.Equal(expectedQuantityForPurchase, 
                   context.Movie.First(m=>m.MovieID==expectedPurchaseItem.MovieId).QuantityForPurchase);

            }
            
        }
        */

    }
}


