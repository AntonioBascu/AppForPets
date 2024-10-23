using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.CompraProveedorViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForPets.UT.Controllers.CompraProveedorController_test
{
    public class CompraProveedor_Create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;


        public CompraProveedor_Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbProductosProveedorForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            compraContext.User = identity;

        }

        [Fact]
        public async Task Create_Get_ConProductosSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                String[] ids = new string[1] { "1" };
                SelectedProductosProveedorForCompraViewModel productos = new SelectedProductosProveedorForCompraViewModel() { IdsProductos = ids, Proveedor= ids.First() };
                ProductoProveedor expectedProducto = Utilities.GetProductosProveedor(0, 1).First();
                ApplicationUser expectedUsuario = Utilities.GetUsers(1, 1).First() as ApplicationUser;

                IList<CompraProvItemViewModel> expectedCompraItems = new CompraProvItemViewModel[1] {
                    new CompraProvItemViewModel {Cantidad=0, ProductoProvId = expectedProducto.IdProductoProv, NombreProductoProv = expectedProducto.Nombre,
                        PrecioCompra = expectedProducto.Precio, Proveedor = expectedProducto.Proveedor.Nombre, TipoAnimal = expectedProducto.Producto.TipoAnimal.NombreAnimal} };
                CompraProvCreateViewModel expectedCompra = new CompraProvCreateViewModel { CompraItems = expectedCompraItems, Nombre = expectedUsuario.Nombre, PrimerApellido = expectedUsuario.PrimerApellido, SegundoApellido = expectedUsuario.SegundoApellido };

                // Act
                var result = controller.Create(productos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraProvCreateViewModel currentPurchase = viewResult.Model as CompraProvCreateViewModel;

                Assert.Equal(currentPurchase, expectedCompra);

            }
        }
        [Fact]
        public async Task Create_Get_SinProducto()
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;
                ApplicationUser expectedUsuario = Utilities.GetUsers(1, 1).First() as ApplicationUser;
                SelectedProductosProveedorForCompraViewModel productos = new SelectedProductosProveedorForCompraViewModel();

                CompraProvCreateViewModel expectedCompra = new CompraProvCreateViewModel
                {
                    Nombre = expectedUsuario.Nombre,
                    PrimerApellido = expectedUsuario.PrimerApellido,
                    SegundoApellido = expectedUsuario.SegundoApellido,
                    CompraItems = new List<CompraProvItemViewModel>()
                };


                // Act
                var result = controller.Create(productos);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraProvCreateViewModel currentCompra = viewResult.Model as CompraProvCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentCompra, expectedCompra);
                Assert.Equal("Debes seleccionar al menos un producto para la compra", error.ErrorMessage);
            }
        }


        public static IEnumerable<object[]> TestCasesForComprasProveedorCreatePost_ConErrores()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_SinSuficientesProductosParaComprar

            ProductoProveedor producto = Utilities.GetProductosProveedor(0, 1).First();
            ApplicationUser usuario = Utilities.GetUsers(1, 1).First() as ApplicationUser;
            var payment1 = new PayPal { Email = usuario.Email, Telefono = usuario.PhoneNumber, Prefijo = "+34" };

            //Input values
            IList<CompraProvItemViewModel> compraItemsViewModel1 = new CompraProvItemViewModel[1] { new CompraProvItemViewModel { Cantidad = 310, ProductoProvId = producto.IdProductoProv, NombreProductoProv = producto.Nombre,
                        PrecioCompra = producto.Precio, Proveedor = producto.Proveedor.Nombre, TipoAnimal = producto.Producto.TipoAnimal.NombreAnimal} };
            CompraProvCreateViewModel compra1 = new CompraProvCreateViewModel { Nombre = usuario.Nombre, PrimerApellido = usuario.PrimerApellido, SegundoApellido = usuario.SegundoApellido, CompraItems = compraItemsViewModel1, DireccionEnvio = "Albacete", PayPal = payment1 };

            //Expected values
            IList<CompraProvItemViewModel> expectedCompraItemsViewModel1 = new CompraProvItemViewModel[1] { new CompraProvItemViewModel { Cantidad = 310, ProductoProvId = producto.IdProductoProv, NombreProductoProv = producto.Nombre,
                        PrecioCompra = producto.Precio, Proveedor = producto.Proveedor.Nombre, TipoAnimal = producto.Producto.TipoAnimal.NombreAnimal} };
            CompraProvCreateViewModel expectedCompraVM1 = new CompraProvCreateViewModel { Nombre = usuario.Nombre, PrimerApellido = usuario.PrimerApellido, SegundoApellido = usuario.SegundoApellido, CompraItems = expectedCompraItemsViewModel1, DireccionEnvio = "Albacete", PayPal = payment1 };
            string expetedErrorMessage1 = "No hay suficientes unidades de Correa de cuello, selecciona menos cantidad o 300 exactamente";


            //Second error: Create_Post_ConCantidad0ParaComprar

            //Input values
            IList<CompraProvItemViewModel> compraItemsViewModel2 = new CompraProvItemViewModel[1] { new CompraProvItemViewModel { Cantidad = 0, ProductoProvId = producto.IdProductoProv, NombreProductoProv = producto.Nombre,
                        PrecioCompra = producto.Precio, Proveedor = producto.Proveedor.Nombre, TipoAnimal = producto.Producto.TipoAnimal.NombreAnimal} };
            CompraProvCreateViewModel compra2 = new CompraProvCreateViewModel { Nombre = usuario.Nombre, PrimerApellido = usuario.PrimerApellido, SegundoApellido = usuario.SegundoApellido, CompraItems = compraItemsViewModel2, DireccionEnvio = "Albacete", PayPal = payment1 };

            //expected values
            IList<CompraProvItemViewModel> expectedCompraItemsViewModel2 = new CompraProvItemViewModel[1] { new CompraProvItemViewModel { Cantidad = 0, ProductoProvId = producto.IdProductoProv, NombreProductoProv = producto.Nombre,
                        PrecioCompra = producto.Precio, Proveedor = producto.Proveedor.Nombre, TipoAnimal = producto.Producto.TipoAnimal.NombreAnimal} };
            CompraProvCreateViewModel expectedCompraVM2 = new CompraProvCreateViewModel { Nombre = usuario.Nombre, PrimerApellido = usuario.PrimerApellido, SegundoApellido = usuario.SegundoApellido, CompraItems = expectedCompraItemsViewModel2, DireccionEnvio = "Albacete", PayPal = payment1 };
            string expetedErrorMessage2 = "Selecciona al menos un producto para comprar";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { compra1, compraItemsViewModel1, null , payment1, expectedCompraVM1, expetedErrorMessage1 },
                new object[] { compra2, compraItemsViewModel2, null , payment1, expectedCompraVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForComprasProveedorCreatePost_ConErrores))]
        public async Task Create_Post_ConErrores(CompraProvCreateViewModel compra, IList<CompraProvItemViewModel> compraItemsViewModel, Tarjeta_Credito payment1, PayPal payment2, CompraProvCreateViewModel expectedCompraVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new CompraProveedorsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.CreatePost(compra, compraItemsViewModel, payment1, payment2);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraProvCreateViewModel currentCompra = viewResult.Model as CompraProvCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedCompraVM, currentCompra);
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

        public static IEnumerable<object[]> TestCasesForComprasProveedorCreatePost_SinErrores()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            CompraProveedor expectedCompra1 = Utilities.GetComprasProveedor(0, 1).First();
            ApplicationUser expectedUsuario1 = expectedCompra1.Usuario;
            var expectedPago1 = expectedCompra1.MetodoPago as Tarjeta_Credito;
            CompraProvItem expectedCompraItem1 = expectedCompra1.CompraItems.First();
            int expectedCantidad1 = Utilities.GetProductosProveedor(0, 1).First().CantidadDisponible - expectedCompraItem1.Cantidad;
            IList<CompraProvItemViewModel> compraItemsViewModel1 = new CompraProvItemViewModel[1] { new CompraProvItemViewModel {
                    Cantidad = expectedCompraItem1.Cantidad, ProductoProvId = expectedCompraItem1.ProductoProveedor.IdProductoProv, NombreProductoProv = expectedCompraItem1.ProductoProveedor.Nombre,
                        PrecioCompra = expectedCompraItem1.ProductoProveedor.Precio, Proveedor = expectedCompraItem1.ProductoProveedor.Proveedor.Nombre, TipoAnimal = expectedCompraItem1.ProductoProveedor.Producto.TipoAnimal.NombreAnimal} };
            CompraProvCreateViewModel compra1 = new CompraProvCreateViewModel
            {
                Nombre = expectedUsuario1.Nombre,
                PrimerApellido = expectedUsuario1.PrimerApellido,
                SegundoApellido = expectedUsuario1.SegundoApellido,
                CompraItems = compraItemsViewModel1,
                DireccionEnvio = expectedCompra1.DireccionEnvio,
                TelefonoContacto = expectedCompra1.TelefonoContacto,
                MetodoPago = "Tarjeta_Credito",
                TarjetaCredito = expectedPago1
            };

            //Payment with Paypal
            CompraProveedor expectedCompra2 = Utilities.GetComprasProveedor(1, 1).First();
            expectedCompra2.IdCompraProveedor = 1;
            expectedCompra2.CompraItems.First().Id = 2;
            expectedCompra2.CompraItems.First().Compra.IdCompraProveedor = 1;
            CompraProvItem expectedCompraItem2 = expectedCompra2.CompraItems.First();
            int expectedCantidad2 = Utilities.GetProductosProveedor(1, 1).First().CantidadDisponible - expectedCompraItem2.Cantidad;
            var expectedPago2 = expectedCompra2.MetodoPago as PayPal;
            expectedPago2.ID = 2;
            ApplicationUser expectedUsuario2 = expectedCompra2.Usuario;

            IList<CompraProvItemViewModel> compraItemsViewModel2 = new CompraProvItemViewModel[1] { new CompraProvItemViewModel {
                   Cantidad = expectedCompraItem2.Cantidad, ProductoProvId = expectedCompraItem2.ProductoProveedor.IdProductoProv, NombreProductoProv = expectedCompraItem2.ProductoProveedor.Nombre,
                        PrecioCompra = expectedCompraItem2.ProductoProveedor.Precio, Proveedor = expectedCompraItem2.ProductoProveedor.Proveedor.Nombre, TipoAnimal = expectedCompraItem2.ProductoProveedor.Producto.TipoAnimal.NombreAnimal} };
            CompraProvCreateViewModel compra2 = new CompraProvCreateViewModel
            {
                Nombre = expectedUsuario2.Nombre,
                PrimerApellido = expectedUsuario2.PrimerApellido,
                SegundoApellido = expectedUsuario2.SegundoApellido,
                CompraItems = compraItemsViewModel2,
                DireccionEnvio = expectedCompra2.DireccionEnvio,
                TelefonoContacto = expectedCompra2.TelefonoContacto,
                MetodoPago = "PayPal",
                PayPal = expectedPago2
            };

            var allTests = new List<object[]>
            {                  //Input values                                              // expected values
                new object[] { compra1, compraItemsViewModel1, expectedPago1, null, expectedCompra1, expectedCantidad1},
                new object[] { compra2, compraItemsViewModel2, null, expectedPago2, expectedCompra2, expectedCantidad2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForComprasProveedorCreatePost_SinErrores))]
        public async Task Create_Post_SinErrores(CompraProvCreateViewModel compra, IList<CompraProvItemViewModel> compraItemsViewModel, Tarjeta_Credito payment1, PayPal payment2, CompraProveedor expectedCompra, int expectedCantidad)
        {
            using (context)
            {

                var controller = new CompraProveedorsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.CreatePost(compra, compraItemsViewModel, payment1, payment2);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualCompra = context.CompraProveedor.Include(p => p.CompraItems).FirstOrDefault(p => p.IdCompraProveedor == expectedCompra.IdCompraProveedor);
                Assert.Equal(expectedCompra, actualCompra);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedCantidad, context.ProductoProveedor.First(p => p.IdProductoProv == expectedCompra.CompraItems.First().ProductoProveedor.IdProductoProv).CantidadDisponible);


            }

        }
    }
}