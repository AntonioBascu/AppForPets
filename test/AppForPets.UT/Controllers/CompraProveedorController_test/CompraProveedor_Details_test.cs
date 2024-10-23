using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
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
    public class CompraProveedor_Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;

        public CompraProveedor_Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbComprasProveedorForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            compraContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesFor_Compra_notfound_SinId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null},
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Compra_notfound_SinId))]
        public async Task Details_Compra_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        public async Task Details_Compra_found()
        {
            // Arrange
            using (context)
            {
                var expectedCompra = Utilities.GetComprasProveedor(0, 1).First();
                var controller = new CompraProveedorsController(context);
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = await controller.Details(expectedCompra.IdCompraProveedor);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as CompraProveedor;
                Assert.Equal(expectedCompra, model);

            }
        }

    }
}

