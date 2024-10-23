using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using AppForPets.Data;
using AppForPets.Models;

namespace AppForPets.Data
{
    public static class SeedData
    {
        //public static void Initialize(UserManager<ApplicationUser> userManager,
        //            RoleManager<IdentityRole> roleManager)
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var role = serviceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            
            
            List<string> rolesNames = new List<string> { "Administrador", "Cliente" };

            SeedRoles(roleManager, rolesNames);
            SeedUsers(userManager, rolesNames);

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {    

            foreach (string roleName in roles) { 
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.com").Result == null)
            {
                Cliente user = new Cliente();
                user.UserName = "elena@uclm.com";
                user.Email = "elena@uclm.com";
                user.Nombre = "Elena";
                user.PrimerApellido = "Navarro";
                user.SegundoApellido = "Martínez";

                IdentityResult result = userManager.CreateAsync(user, "Password1234%").Result;

                if (result.Succeeded)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user,roles[0]).Wait();
                    user.EmailConfirmed = true;
                }
            }

            if (userManager.FindByNameAsync("gregorio@uclm.com").Result == null)
            {
                Cliente user = new Cliente();
                user.UserName = "gregorio@uclm.com";
                user.Email = "gregorio@uclm.com";
                user.Nombre = "Gregorio";
                user.PrimerApellido = "Diaz";
                user.SegundoApellido = "Descalzo";

                IdentityResult result = userManager.CreateAsync(user, "APassword1234%").Result;

                if (result.Succeeded)
                {
                    //Employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                    user.EmailConfirmed = true;
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.com").Result == null)
            {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                Cliente user = new Cliente();
                user.UserName = "peter@uclm.com";
                user.Email = "peter@uclm.com";
                user.Nombre = "Peter";
                user.PrimerApellido = "Jackson";
                user.SegundoApellido = "Jackson";

                IdentityResult result = userManager.CreateAsync(user, "OtherPass12$").Result;

                if (result.Succeeded)
                {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                    user.EmailConfirmed = true;
                }
            }

        }

     

    }



}


