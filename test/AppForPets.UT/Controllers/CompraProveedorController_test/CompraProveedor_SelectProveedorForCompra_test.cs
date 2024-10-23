using AppForPets.UT.Controllers;
using AppForPets.Data;
using System;
using System.Collections.Generic;
using System.Text;
using AppForPets.Models;
using System.Threading.Tasks;
using Xunit;
using AppForPets.Controllers;
using Microsoft.AspNetCore.Mvc;
using AppForPets.Models.CompraProveedorViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AppForPets.UT.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppForPets.UT.Controllers.CompraProveedorController_test
{
    public class CompraProveedor_SelectProveedorForCompra_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;

        public CompraProveedor_SelectProveedorForCompra_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbProveedoresForTests(context);

            context.Users.Add(new ApplicationUser
            {
                Id = "2",
                UserName = "elena@uclm.com",
                PhoneNumber = "967959595",
                Email = "elena@uclm.com",
                Nombre = "Elena",
                PrimerApellido = "Navarro",
                SegundoApellido = "Martínez"
            });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            compraContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesForSelectProveedorForCompra_get()
        {
            var allTests = new List<object[]>
            {
                new object[] {Utilities.GetProveedores(0, 2), null, null },
                new object[] {Utilities.GetProveedores(1, 1), "ZooPlus", null},
                new object[] {Utilities.GetProveedores(0, 1), null, "Cuenca"},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectProveedorForCompra_get))]
        public async Task SelectProveedorForCompra_Get(List<Proveedor> expectedProveedor, string filtroNombre, string filtroDireccion)
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.SelectProveedorForCompra(filtroNombre, filtroDireccion);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectProveedorForCompraViewModel model = viewResult.Model as SelectProveedorForCompraViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedProveedor, model.Proveedores, Comparer.Get<Proveedor>((p1, p2) => p1.Nombre == p2.Nombre && p1.Direccion == p2.Direccion && p1.CorreoE == p2.CorreoE && p1.Telefono == p2.Telefono));

            }
        }

        [Fact]
        public async Task SelectProveedor_Post_ProveedorNoSeleccionado()
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;

                var expectedProveedores = Utilities.GetProveedores(0, 2);   

                SelectedProveedorForCompraViewModel seleccionado = new SelectedProveedorForCompraViewModel { IdProveedor= null};

                // Act
                var result = controller.SelectProveedorForCompra(seleccionado);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectProveedorForCompraViewModel model = viewResult.Model as SelectProveedorForCompraViewModel;

                Assert.Equal(expectedProveedores, model.Proveedores, Comparer.Get<Proveedor>((p1, p2) => p1.Nombre == p2.Nombre && p1.Direccion == p2.Direccion && p1.CorreoE == p2.CorreoE && p1.Telefono == p2.Telefono));

            }
        }

        [Fact]
        public async Task SelectProveedor_Post_ProveedorSeleccionado()
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;

                string id = new string("1");

                SelectedProveedorForCompraViewModel seleccionado = new SelectedProveedorForCompraViewModel { IdProveedor = id };

                // Act
                var result = controller.SelectProveedorForCompra(seleccionado);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result); // el controlador devuelve una redirección de acción
                var proveedorSelec = viewResult.RouteValues.First();

                Assert.Equal(seleccionado.IdProveedor, proveedorSelec.Value);

            }
        }

    }
}
