using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Desing
{
    public class Tipo_Servicio
    {

        [Key]
        public virtual int ID {
            get;
            set;
        }

        [Required]
        [StringLength(50, ErrorMessage = "El tipo de nombre no puede tener más de 50 characters.")]
        [Display(Name = "Tipo del Servicio")]
        public virtual String Nombre {
            get;
            set;
        }

        public virtual IList<Servicio> Servicio {
            get;
            set;
        }


    }
}
