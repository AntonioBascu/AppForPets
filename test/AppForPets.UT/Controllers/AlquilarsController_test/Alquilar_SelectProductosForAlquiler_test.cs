using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppForPets.Models.AlquilarViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppForPets.UT.Controllers;

namespace AppForPets.UT.Controller.AlquilarsController_test
{
    public class Alquilar_SelectProductos_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext alquilarContext;

        public Alquilar_SelectProductos_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.

            Utilities.InitializeDbProductosForTests(context);

            context.Users.Add(new Cliente { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido= "Jackson", SegundoApellido= "García" });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            alquilarContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            alquilarContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesForSelectProductosForAlquiler_get()
        {
            var allTests = new List<object[]>
            {
                new object[] {Utilities.GetProductos(0,3), Utilities.GetTipoAnimals(0,3), null, null },
                new object[] {Utilities.GetProductos(2,1), Utilities.GetTipoAnimals(0,3), "mega", null},
                new object[] {Utilities.GetProductos(0,3), Utilities.GetTipoAnimals(0,3), null, "Perro"},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectProductosForAlquiler_get))]
        public async Task SelectProductosForAlquiler_Get(List<Producto> expectedProductos, List <TipoAnimal> expectedTipoAnimals, string filterNombreProducto, string filterTipoAnimal )
        {
            using (context)
            {

                // Arrange
                var controller = new AlquilarsController(context);
                controller.ControllerContext.HttpContext = alquilarContext;

                var expectedTipoAnimalsSelectList = new SelectList(expectedTipoAnimals.Select(g => g.NombreAnimal).ToList());

                // Act
                var result = controller.SelectProductosForAlquiler(filterNombreProducto, filterTipoAnimal);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectProductosForAlquilerViewModel model = viewResult.Model as SelectProductosForAlquilerViewModel;

                // Comprueba ambas colecciones (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedProductos, model.Productos, Comparer.Get<Producto>((p1, p2) => p1.Equals(p2)));
                Assert.Equal(expectedTipoAnimalsSelectList, model.TipoAnimal, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                
            }
        }

        //[Fact]
        //public async Task SelectMovie_Get_WithoutAnyFilter()
        //{
        //    using (context)
        //    {

        //        // Arrange
        //        var controller = new PurchasesController(context);
        //        controller.ControllerContext.HttpContext = purchaseContext;
        //        Genre genre = new Genre { Name = "Sci-fi" };
        //        var genres = new List<Genre> { genre };
        //        var expectedGenres = new SelectList(genres.Select(g => g.Name).ToList());

        //        var expectedMovies = Utilities.Movies;

        //        // Act
        //        var result = controller.SelectMoviesForPurchase(null,null);

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
        //        SelectMoviesForPurchaseViewModel model = viewResult.Model as SelectMoviesForPurchaseViewModel;

        //        // Check that both collections (expected and result returned) have the same elements with the same name
        //        Assert.Equal(expectedMovies, model.Movies, Comparer.Get<Movie>((p1, p2) => p1.Equals(p2)));
        //        Assert.Equal(expectedGenres, model.Genres,Comparer.Get<SelectListItem>((s1,s2) => s1.Value == s2.Value));                

        //    }
        //}





        [Fact]
        public void SelectProductosForAlquiler_Post_ProductosNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new AlquilarsController(context);
                controller.ControllerContext.HttpContext = alquilarContext;
                var expectedTipoAnimals = new SelectList(Utilities.GetTipoAnimals(0, 3).Select(g => g.NombreAnimal).ToList());
                var expectedProductos = Utilities.GetProductos(0, 3);

                SelectedProductosForAlquilerViewModel selected = new SelectedProductosForAlquilerViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectProductosForAlquiler(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectProductosForAlquilerViewModel model = viewResult.Model as SelectProductosForAlquilerViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedProductos, model.Productos, Comparer.Get<Producto>((p1, p2) => p1.Equals(p2)));
                Assert.Equal(expectedTipoAnimals, model.TipoAnimal, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }

        [Fact]
        public void SelectProductosForAlquiler_Post_ProductosSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new AlquilarsController(context);
                controller.ControllerContext.HttpContext = alquilarContext;

                String[] ids = new string[1] { "1" };
                SelectedProductosForAlquilerViewModel productos = new SelectedProductosForAlquilerViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectProductosForAlquiler(productos);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentProductos = viewResult.RouteValues.Values.First();
                Assert.Equal(productos.IdsToAdd, currentProductos);

            }
        }




    }


}
