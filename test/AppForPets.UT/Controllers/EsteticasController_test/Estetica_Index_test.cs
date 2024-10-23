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
    public class Estetica_Index_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext esteticaContext;


        public Estetica_Index_test()
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


        ////[Fact]
        ////public async Task Index_Get()
        ////{
        ////    using (context)
        ////    {
        ////        int i;


        ////        var expectedPurchases = new List<Estetica> { Utilities.GetEsteticas(0, 1).First() };

        ////        var controller = new EsteticasController(context);

        ////        controller.ControllerContext.HttpContext = esteticaContext;

        ////        // Act
        ////        var result = await controller.Index();

        ////        //Assert
        ////        var viewResult = Assert.IsType<ViewResult>(result);

        ////        List<Estetica> model = viewResult.Model as List<Estetica>;

        ////        Assert.Equal(expectedPurchases.Count, model.Count);

        ////        for (i = 0; i < model.Count(); i++)
        ////            Assert.Equal(expectedPurchases[i], model[i]);

        ////    }
        ////}
    }
}
