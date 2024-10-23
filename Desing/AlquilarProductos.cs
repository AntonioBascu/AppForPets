using Desing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Desing
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

        public virtual int Cantidad
        {
            get;
            set;
        }

        public virtual Alquilar Alquilar
        {
            get;
            set;
        }
    }
}
