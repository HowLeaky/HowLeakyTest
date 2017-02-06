using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HowLeakyModels.Accounts
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : Microsoft.AspNet.Identity.EntityFramework.IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        [Display(Name="Given Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Family Name")]
        public string LastName { get; set; }

        public string Telephone { get; set; }

        public string Organisation { get; set; }

        //public string Region { get; set; }
        //public string Location { get; set; }

        public string ConfirmationToken { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }
        
        [Display(Name = "Date Registered")]
        public DateTime? RegisterDate { get; set; }

    }

}