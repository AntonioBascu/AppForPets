using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.EsteticaViewModels;
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

namespace AppForPets.UT.Controllers.EsteticasController_test
{
    public class Estetica_SelectServiciosForEstetica_test
    {


        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext esteticaContext;

        public Estetica_SelectServiciosForEstetica_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.

            Utilities.InitializeDbServiciosForTests(context);

            context.Users.Add(new Cliente { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });


            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            esteticaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            esteticaContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesForSelectServicio_get()
        {
            var allTests = new List<object[]>
            {
                new object[] {Utilities.GetServicios(0,1), Utilities.GetTipoServicios(0,3), "Peluqueria", 0},
                new object[] {Utilities.GetServicios(0,3), Utilities.GetTipoServicios(0,3), null, 0 },
                new object[] {Utilities.GetServicios(0,3), Utilities.GetTipoServicios(0,3), null, 1},
            };

            return allTests;
        }


        [Theory]
        [MemberData(nameof(TestCasesForSelectServicio_get))]
        public async Task SelectServicio_Get(List<Servicio> expectedServicios, List<Tipo_Servicio> expectedTipoServicios, string filtroNombreTipo, int filtroTiempoDur)
        {
            using (context)
            {

                // Arrange
                var controller = new EsteticasController(context);
                controller.ControllerContext.HttpContext = esteticaContext;

                var expectedTipoServiciosSelectList = new SelectList(expectedTipoServicios.Select(g => g.Nombre).ToList());

                // Act
                var result = controller.SelectServicioForEstetica(filtroNombreTipo, filtroTiempoDur);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectServiciosForEsteticaViewModels model = viewResult.Model as SelectServiciosForEsteticaViewModels;


                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedServicios, model.Servicios, Comparer.Get<Servicio>((p1, p2) => p1.Equals(p2)));
                Assert.Equal(expectedTipoServiciosSelectList, model.Tipo_Servicios, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }


        [Fact]
        public void SelectServicio_Post_ServiciosNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new EsteticasController(context);
                controller.ControllerContext.HttpContext = esteticaContext;
                var expectedTipoServicios = new SelectList(Utilities.GetTipoServicios(0, 3).Select(g => g.Nombre).ToList());
                var expectedServicios = Utilities.GetServicios(0, 3);

                SelectedServiciosForEsteticaViewModels selected = new SelectedServiciosForEsteticaViewModels { IdsToAdd = null };

                // Act
                var result = controller.SelectServicioForEstetica(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectServiciosForEsteticaViewModels model = viewResult.Model as SelectServiciosForEsteticaViewModels;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedServicios, model.Servicios, Comparer.Get<Servicio>((p1, p2) => p1.Equals(p2)));
                Assert.Equal(expectedTipoServicios, model.Tipo_Servicios, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }


        [Fact]
        public void SelectServicio_Post_ServiciosSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new EsteticasController(context);
                controller.ControllerContext.HttpContext = esteticaContext;

                String[] ids = new string[1] { "1" };
                SelectedServiciosForEsteticaViewModels servicios = new SelectedServiciosForEsteticaViewModels { IdsToAdd = ids };

                // Act
                var result = controller.SelectServicioForEstetica(servicios);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentServicios = viewResult.RouteValues.Values.First();
                Assert.Equal(servicios.IdsToAdd, currentServicios);

            }
        }


    }
}
