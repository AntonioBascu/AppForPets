
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models
{
    public class AlquilarProductos
    {

        [Key()]
        public virtual int Id
        {
            get;
            set;
        }

        public virtual Producto Producto
        {
            get;
            set;
        }


        public virtual Alquilar Alquilar
        {
            get;
            set;
        }

        [Range(1, Double.MaxValue, ErrorMessage = "You must provide a valid quantity")]
        public virtual int Cantidad
        {
            get;
            set;
        }
    }
}
