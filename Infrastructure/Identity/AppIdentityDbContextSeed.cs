using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Aashish Sawant",
                    Email = "aashish@sawant.com",
                    UserName = "aashish@sawant.com",
                    Address = new Address
                    {
                        FirstName = "Aashish",
                        LastName = "Sawant",
                        Street = "17th Street",
                        City = "NewYork",
                        State = "NewYork",
                        Zipcode = "560012"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}