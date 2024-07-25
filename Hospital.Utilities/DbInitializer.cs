using Azure.Identity;
using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception) 
            {
                throw;
            }
            if (!_roleManager.RoleExistsAsync(UserRoles.WebSite_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(UserRoles.WebSite_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.WebSite_Doctor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.WebSite_Patient)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    UserName = "Rajat",
                    Email = "rajat@gmail.com"
                }, "12345").GetAwaiter().GetResult();

                var AppUser = _context.User.FirstOrDefault(x => x.Email == "rajat@gmail.com");
                if (AppUser != null)
                {
                    _userManager.AddToRoleAsync(AppUser, UserRoles.WebSite_Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}
