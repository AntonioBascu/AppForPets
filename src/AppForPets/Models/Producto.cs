using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models
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

        public virtual int PrecioAlquiler
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "El precio minimo es 1.")]
        [Display(Name = "Precio por unidad")]
        public virtual int Precio
        {
            get;
            set;
        }

        public virtual TipoAnimal TipoAnimal
        {
            get;
            set;
        }

        public virtual IList<ProductoProveedor> ProductosProveedor
        {
            get;
            set;
        }

        public virtual int CantidadAlquilar
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            var myObject = obj as Producto;

            if (null != myObject)
            {
                return this.ProductoID == myObject.ProductoID
                   && this.NombreProducto == myObject.NombreProducto
                   && this.PrecioAlquiler == myObject.PrecioAlquiler
                   && this.CantidadAlquilar == myObject.CantidadAlquilar
                   && this.TipoAnimal.NombreAnimal == myObject.TipoAnimal.NombreAnimal;
            }
            else
            {
                return false;
            }
        }
    }
}
