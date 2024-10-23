using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Desing
{
    public class ProductoProveedor
    {
        [Key]
        public virtual int IdProductoProv
        {
            get;
            set;
        }

        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public virtual String Nombre
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.2, float.MaxValue, ErrorMessage = "El precio mínimo es 0.2 ")]
        [Display(Name = "Precio por unidad")]
        public virtual float Precio
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de caducidad")]
        public virtual DateTime FechaCaducidad
        {
            get;
            set;
        }

        [Required]
        public virtual Producto Producto
        {
            get;
            set;
        }

        [Required]
        public virtual Proveedor Proveedor
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Cantidad disponible")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1.")]
        public virtual int CantidadDisponible 
        {
            get;
            set;
        }

        public virtual IList<CompraProvItem> CompraItems
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            var myObject = obj as ProductoProveedor;

            if (null != myObject)
            {
                return this.Producto == myObject.Producto
                   && this.Nombre == myObject.Nombre
                   && this.Precio == myObject.Precio
                   && this.CantidadDisponible == myObject.CantidadDisponible
                   && this.FechaCaducidad == myObject.FechaCaducidad;
            }
            else
            {
                return false;
            }
        }
    }
}
