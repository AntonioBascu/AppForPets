using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models
{
    public class CompraProveedor
    {
        [Key]
        public int IdCompraProveedor
        {
            get;
            set;
        }


        [ForeignKey("IdUsuario")]
        public virtual ApplicationUser Usuario
        {
            get;
            set;
        }

        public double PrecioTotal
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha de la compra")]
        public DateTime FechaCompra
        {
            get;
            set;
        }

        public virtual IList<CompraProvItem> CompraItems
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección de envío")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, indica una dirección para la entrega")]

        public String DireccionEnvio
        {
            get;
            set;
        }

        [MaxLength(9, ErrorMessage = "Ese numero no existe"), MinLength(9)]
        public virtual string TelefonoContacto
        {
            get;
            set;
        }

        [Display(Name = "Método de pago")]
        [Required()]
        public Metodo_Pago MetodoPago
        {
            get;
            set;
        }

        public CompraProveedor()
        {

            CompraItems = new List<CompraProvItem>();
        }

        public override bool Equals(object obj)
        {
            CompraProveedor purchase = obj as CompraProveedor;
            int i;
            bool result = false;


            result = ((this.Usuario.UserName == purchase.Usuario.UserName)
                && (this.DireccionEnvio == purchase.DireccionEnvio)
                && (this.MetodoPago.Equals(purchase.MetodoPago)))
                && (this.FechaCompra.Subtract(purchase.FechaCompra) < new TimeSpan(0, 1, 0)); ;
            
            result = result && (this.CompraItems.Count == purchase.CompraItems.Count);


            for (i = 0; i < this.CompraItems.Count; i++)
                result = result && (this.CompraItems[i].Equals(purchase.CompraItems[i]));

            return result;
        }
    }
}
