using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppForPets.Controllers;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.UT.Controllers;

namespace AppForPets.UT.Controller.AlquilarsController_test
{
    public class Alquilar_Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext alquilarContext;

        public Alquilar_Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDbAlquilarsForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            alquilarContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            alquilarContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesFor_Alquilar_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Alquilar_notfound_withoutId))]
        public async Task Details_Alquilar_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new AlquilarsController(context);
                controller.ControllerContext.HttpContext = alquilarContext;

                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }
        
        [Fact]
        public async Task Details_Alquilar_found()
        {
            // Arrange
            using (context)
            {
                var expectedAlquilar = Utilities.GetAlquilars(0, 1).First();
                var controller = new AlquilarsController(context);
                controller.ControllerContext.HttpContext = alquilarContext;

                // Act
                var result = await controller.Details(expectedAlquilar.AlquilarID);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Alquilar;
                Assert.Equal(expectedAlquilar, model);

            }
        }

    }
}

