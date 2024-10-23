using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.CompraProveedorViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForPets.UT.Controllers.CompraProveedorController_test
{
    public class CompraProveedor_SelectProductosProveedorForCompra_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;

        public CompraProveedor_SelectProductosProveedorForCompra_test()
        {
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


        public static IEnumerable<object[]> TestCasesForSelectProductosProvForCompra_get()
        {
            string id = new string("1");
            SelectedProveedorForCompraViewModel seleccionado = new SelectedProveedorForCompraViewModel { IdProveedor = id };

            var allTests = new List<object[]>
            {
                new object[] { Utilities.GetProductosProveedor(0, 1), Utilities.GetTipoAnimales(0, 3), seleccionado, null, null, null},
                new object[] { Utilities.GetProductosProveedor(0, 1), Utilities.GetTipoAnimales(0, 3), null, "Correa", null, seleccionado.IdProveedor},
                new object[] { Utilities.GetProductosProveedor(0, 1), Utilities.GetTipoAnimales(0, 3), null, null, "Perro", seleccionado.IdProveedor},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectProductosProvForCompra_get))]
        public async Task SelectProductosProvForCompra_Get(List<ProductoProveedor> expectedProductos, List<TipoAnimal> expectedTiposAnimal, SelectedProveedorForCompraViewModel seleccionado, string filtroNombre, string filtroTipo, string proveedor)
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;
        
                var animalesEsperados = new SelectList(expectedTiposAnimal.Select(t => t.NombreAnimal).ToList());

                // Act
                var result = controller.SelectProductosProveedorForCompra(seleccionado, filtroNombre, filtroTipo, proveedor);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectProductosProveedorForCompraViewModel model = viewResult.Model as SelectProductosProveedorForCompraViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedProductos, model.Productos, Comparer.Get<ProductoProveedor>((p1, p2) => p1.Equals(p2)));
                Assert.Equal(animalesEsperados, model.TipoAnimales, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }

        [Fact]
        public async Task SelectProductosProveedor_Post_ProductosNoSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;
           
                var animalesEsperados = new SelectList(Utilities.GetTipoAnimales(0, 3).Select(t => t.NombreAnimal).ToList());                
                var expectedProductos = Utilities.GetProductosProveedor(0, 1);

                string id = new string("1");

                SelectedProveedorForCompraViewModel seleccionado = new SelectedProveedorForCompraViewModel { IdProveedor = id };
                SelectedProductosProveedorForCompraViewModel prodSeleccionados = new SelectedProductosProveedorForCompraViewModel { IdsProductos = null, Proveedor= seleccionado.IdProveedor };

                // Act
                var result = controller.SelectProductosProveedorForCompra(prodSeleccionados);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectProductosProveedorForCompraViewModel model = viewResult.Model as SelectProductosProveedorForCompraViewModel;

                Assert.Equal(expectedProductos, model.Productos, Comparer.Get<ProductoProveedor>((p1, p2) => p1.Equals(p2)));
                Assert.Equal(animalesEsperados, model.TipoAnimales, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
            }
        }

        [Fact]
        public async Task SelectProductosProveedor_Post_ProductosSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;

                string id = new string("1");

                SelectedProveedorForCompraViewModel seleccionado = new SelectedProveedorForCompraViewModel { IdProveedor = id };

                string[] ids = new string[1] { "1" };
                SelectedProductosProveedorForCompraViewModel prodSeleccionados = new SelectedProductosProveedorForCompraViewModel { IdsProductos = ids, Proveedor = seleccionado.IdProveedor };

                // Act
                var result = controller.SelectProductosProveedorForCompra(prodSeleccionados);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result); // Check the controller returns a view
                var productosSelec = viewResult.RouteValues.Values.First();

                Assert.Equal(prodSeleccionados.IdsProductos, productosSelec);
            }
        }
    }
}
