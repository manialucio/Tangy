using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tangy_Common;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext   )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Initialize()
        {

            try {

                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
                if (_roleManager.RoleExistsAsync(SD.Roles.Admin).GetAwaiter().GetResult())
                {
                    return;
                }

                _roleManager.CreateAsync(new IdentityRole(SD.Roles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Roles.Customer)).GetAwaiter().GetResult();

                string userName = "superUser";
                if (_userManager.FindByNameAsync(userName).GetAwaiter().GetResult() != null)
                {
                    return;
                }

                IdentityUser user = new()
                {
                    UserName = userName,
                    Email = "superUser@gmail.com",
                    EmailConfirmed = true
                };
                _userManager.CreateAsync(user, "Test01!").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, SD.Roles.Admin).GetAwaiter().GetResult();

            }
            catch (Exception ex) 
            {
            
            }

        }
    }
}
