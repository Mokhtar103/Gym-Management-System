using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementDAL.Data.DataSeed
{
    public class IdentityDataSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {

                if (!roleManager.Roles.Any())
                {

                    var roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = "SuperAdmin", NormalizedName = "TRAINER" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            };

                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }

                }

                if (!userManager.Users.Any())
                {
                    var superAdmin = new ApplicationUser
                    {
                        FirstName = "Ahmed",
                        LastName = "Mokhtar",
                        UserName = "Mokhtar103",
                        Email = "ahmedmohktar26@gmail.com",
                        PhoneNumber = "01022223333",

                    };

                    userManager.CreateAsync(superAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(superAdmin, "SuperAdmin").Wait();

                    var admin = new ApplicationUser
                    {
                        FirstName = "Mohamed",
                        LastName = "Ali",
                        UserName = "MohamedAli2002",
                        Email = "mohamed12@gmail.com",
                        PhoneNumber = "01044445555",
                    };

                    userManager.CreateAsync(admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();

                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex.Message}");
                return false;
            }
          

           

        }
    }
}
