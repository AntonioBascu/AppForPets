using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppForPets.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual string Nombre {
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string PrimerApellido {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string SegundoApellido {
            get;
            set;
        }
    }
}
