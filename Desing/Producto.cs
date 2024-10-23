using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Desing
{
    public class Producto
    {
        [Key]
        public virtual int ProductoID
        {
            get;
            set;
        }

        public virtual string NombreProducto
        {
            get;
            set;
        }

        public virtual IList<AlquilarProductos> AlquilarProductos
        {
            get; 
            set;
        }

        public virtual IList<ProductoProveedor> ProductosProveedor
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 100, ErrorMessage = "Minimo es 1 y 100 el máximo")]
        [Display(Name = "Precio de alquiler")]
        public virtual int PrecioAlquiler
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "El precio minimo es 1.")]
        [Display(Name = "Precio por unidad")]
        public virtual int PrecioProveedor
        {
            get;
            set;
        }

        public virtual TipoAnimal TipoAnimal
        {
            get;
            set;
        }

        public virtual int CantidadAlquilar
        {
            get;
            set;
        }
    }
}
