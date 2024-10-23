using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models
{
    public class Metodo_Pago
    {
        [Key]
        public virtual int ID
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            Metodo_Pago p = (Metodo_Pago)obj;
            return (p.ID == ID);
        }
    }
}
