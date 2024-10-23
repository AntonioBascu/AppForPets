using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.CompraViewModels
{
    public class CompraCreateViewModel
    {
        public virtual string Nombre{
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string PrimerApellido {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string SegundoApellido {
            get;
            set;
        }

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string ClienteId {
            get;
            set;
        }

        public double PrecioTotal {
            get;
            set;
        }

        public DateTime fechaCompra {
            get;
            set;
        }

        public virtual IList<L_compraViewModel> l_compras {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduce tu dirección de envio")]

        public String DireccionEnvio {
            get;
            set;
        }

        [Display(Name = "Metodo de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Selecciona un método de pago")]
        public String MetodoPago {
            get;
            set;
        }

        public CompraCreateViewModel()
        {
            l_compras = new List<L_compraViewModel>();
            TarjetaCredito = new Tarjeta_Credito();
        }
        public Tarjeta_Credito TarjetaCredito { get; set; }
        public PayPal PayPal { get; set; }

        

        public override bool Equals(object obj)
        {
            CompraCreateViewModel compra = obj as CompraCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == compra.Nombre)
                && (this.PrimerApellido == compra.PrimerApellido)
                && (this.SegundoApellido == compra.SegundoApellido)
                && (this.ClienteId == compra.ClienteId)
                && (this.PrecioTotal == compra.PrecioTotal)
                && (this.DireccionEnvio == compra.DireccionEnvio)
                //the timepsan is less than a minute between them
                && (this.fechaCompra.Subtract(compra.fechaCompra) < new TimeSpan(0, 1, 0))
                && (
                   ((this.MetodoPago == null) && (compra.MetodoPago == null))
                   || this.MetodoPago.Equals(compra.MetodoPago)
                )
                );


            result = result && (this.l_compras.Count == compra.l_compras.Count);


            for (i = 0; i < this.l_compras.Count; i++)
                result = result && (this.l_compras[i].Equals(compra.l_compras[i]));

            return result;
        }
    }

    public class L_compraViewModel
    {
        public virtual int AnimalID {
            get;
            set;
        }

        [Display(Name = "Price de compra")]
        public virtual double PrecioCompra {
            get;
            set;
        }

        public virtual String Tipo {
            get;
            set;
        }
        public virtual String raza {
            get;
            set;
        }
        [Required]
        public virtual int cantidad {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            L_compraViewModel l_Compra = obj as L_compraViewModel;
            bool result = false;
            if ((AnimalID == l_Compra.AnimalID)
                && (this.PrecioCompra == l_Compra.PrecioCompra)
                    && (this.cantidad == l_Compra.cantidad))
                result = true;
            return result;
        }
    }
}
