using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models
{
    public class PayPal: Metodo_Pago
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 2)]

        public string Prefijo { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 6)]

        public string Telefono { get; set; }


        public override bool Equals(object obj)
        {
            PayPal p = (PayPal)obj;
            return (p.Email == Email 
                && p.Prefijo == Prefijo 
                && p.Telefono == Telefono);
        }


    }
}
