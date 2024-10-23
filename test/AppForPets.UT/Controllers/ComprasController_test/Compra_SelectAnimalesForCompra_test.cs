using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.CompraViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForPets.UT.Controllers.ComprasController_test
{
    public class Compra_SelectAnimalesForCompra_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;

        public Compra_SelectAnimalesForCompra_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.

            Utilities.InitializeDbAnimalesForTests(context);

            context.Users.Add(new Cliente { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            compraContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectAnimales_get()
        {
            var allTests = new List<object[]>
            {
                new object[] {Utilities.GetAnimales(0,1), null, 10},
                new object[] {Utilities.GetAnimales(0,1), "golden", 0},
                new object[] {Utilities.GetAnimales(0,3), null, 0 },
            };

            return allTests;
        }
        [Theory]
        [MemberData(nameof(TestCasesForSelectAnimales_get))]
        public async Task SelectAnimal_Get(List<Animal> expectedAnimales, string TipoAnimal, double precio)
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.SelectAnimalesForCompra(TipoAnimal, precio);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectAnimalesForCompraViewModels model = viewResult.Model as SelectAnimalesForCompraViewModels;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedAnimales, model.Animales, Comparer.Get<Animal>((p1, p2) => p1.Equals(p2)));

            }
        }
        [Fact]
        public void SelectAnimal_Post_AnimalesNotSelected()
        {
            using (context)
            {
                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = compraContext;
                
                //variable con todos los animales
                var expectedAnimales = Utilities.GetAnimales(0, 3);

                SelectedAnimalesForCompraViewModel selected = new SelectedAnimalesForCompraViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectAnimalesForCompra(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectAnimalesForCompraViewModels model = viewResult.Model as SelectAnimalesForCompraViewModels;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedAnimales, model.Animales, Comparer.Get<Animal>((p1, p2) => p1.Equals(p2)));


            }
        }
        [Fact]
        public void SelectAnimal_Post_AnimalesSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = compraContext;

                String[] ids = new string[1] { "1" };
                SelectedAnimalesForCompraViewModel animales = new SelectedAnimalesForCompraViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectAnimalesForCompra(animales);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentAnimales = viewResult.RouteValues.Values.First();
                Assert.Equal(animales.IdsToAdd, currentAnimales);

            }
        }
    }
    }

