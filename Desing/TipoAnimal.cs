using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Desing
{
    public class TipoAnimal
    {
        [Key]
        public virtual int TipoAnimalID
        {
            get;
            set;
        }

        [Required]
        public virtual string NombreAnimal
        {
            get;
            set;
        }

        public virtual IList<Producto> Productos
        {
            get;
            set;
        }

    }
}
