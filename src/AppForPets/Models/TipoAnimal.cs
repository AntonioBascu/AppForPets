using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AppForPets.Models
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
        public override bool Equals(Object obj)
        {

            var myObject = obj as TipoAnimal;

            if (null != myObject)
            {
                return this.TipoAnimalID == myObject.TipoAnimalID
                   && this.NombreAnimal == myObject.NombreAnimal
                   && this.Productos == myObject.Productos;
            }
            else
            {
                return false;
            }
        }
    }
}
