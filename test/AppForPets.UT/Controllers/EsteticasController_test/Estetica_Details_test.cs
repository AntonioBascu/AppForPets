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

namespace AppForPets.UT.Controllers.EsteticasController_test
{
    public class Estetica_Details_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext esteticaContext;

        public Estetica_Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbPEsteticasForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            esteticaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            esteticaContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesFor_Estetica_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }


        [Theory]
        [MemberData(nameof(TestCasesFor_Estetica_notfound_withoutId))]
        public async Task Details_Estetica_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new EsteticasController(context);
                controller.ControllerContext.HttpContext = esteticaContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }


        [Fact]
        public async Task Details_Estetica_found()
        {
            // Arrange
            using (context)
            {
                var expectedEstetica = Utilities.GetEsteticas(0, 1).First();
                var controller = new EsteticasController(context);
                controller.ControllerContext.HttpContext = esteticaContext;

                // Act
                var result = await controller.Details(expectedEstetica.EsteticaID);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Estetica;
                Assert.Equal(expectedEstetica, model);

            }
        }


    }
}
