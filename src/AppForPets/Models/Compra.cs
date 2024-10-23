using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppForPets.Models
{
    public class Compra
    {
        [Key]
        public int CompraID
        {
            get;
            set;
        }
        [DataType(DataType.Currency)]
        [Range(1, 100, ErrorMessage = "Mínimo is 10 y 200, respectivamente")]
        [Display(Name = "Precio del Servicio")]
        public double PrecioTotal 
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha de Compra")]
        public DateTime FechaCompra 
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
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor,ponga su dirección de envio")]
        public String DirecionEnvio 
        {
            get;
            set;
        }
        public virtual IList<L_Compra> L_Compras {
            get;
            set;
        }
        public virtual Cliente Cliente 
        {
            get;
            set;
        }
        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string ClienteId {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            Compra compra = obj as Compra;
            int i;
            bool result = false;

            result = ((this.Cliente.UserName == compra.Cliente.UserName)
                && (this.DirecionEnvio == compra.DirecionEnvio)
                && (this.MetodoPago.Equals(compra.MetodoPago))
                && (this.FechaCompra.Subtract(compra.FechaCompra) < new TimeSpan (0, 1, 0)));

            result = result && (this.L_Compras.Count == compra.L_Compras.Count);

            for (i = 0; i < this.L_Compras.Count; i++)
                result = result && (this.L_Compras[i].Equals(compra.L_Compras[i]));

            return result;
        }
    }
}
