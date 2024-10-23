using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models
{
    public class Proveedor
    {
        [Key]
        public virtual int IdProveedor
        {
            get;
            set;
        }

        [Required]
        public virtual string Nombre
        {
            get;
            set;
        }


        [MaxLength(30, ErrorMessage = "Dirección demasiado larga"), MinLength(5)]
        public virtual string CorreoE
        {
            get;
            set;
        }


        [MaxLength(9, ErrorMessage = "Ese numero no existe"), MinLength(9)]
        public virtual string Telefono
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public virtual string Direccion
        {
            get;
            set;
        }


        public virtual IList<ProductoProveedor> ProveedorProductos
        {
            get;
            set;
        }
    }
}


