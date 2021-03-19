using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<User>>(userData);
            if (users == null) return;

            var roles = new List<Role>
            {
                new Role{Name = "Member"},
                new Role{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "password");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new User
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "password");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
