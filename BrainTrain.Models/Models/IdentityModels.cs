using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using BrainTrain.Core.Models;

namespace BrainTrain.Models.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    //public class BrainTrainContext : IdentityDbContext<ApplicationUser>
    //{
    //    public BrainTrainContext()
    //        : base("braintrainContext", throwIfV1Schema: false)
    //    {
    //    }
        
    //    public static BrainTrainContext Create()
    //    {
    //        return new BrainTrainContext();
    //    }
    //}
}