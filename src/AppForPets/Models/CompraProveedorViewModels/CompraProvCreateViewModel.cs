using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.CompraProveedorViewModels
{
    public class CompraProvCreateViewModel
    {
        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer apellido")]
        public virtual string PrimerApellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo apellido")]
        public virtual string SegundoApellido
        {
            get;
            set;
        }

        public string IdUsuario
        {
            get;
            set;
        }

        public double PrecioTotal
        {
            get;
            set;
        }

        public DateTime FechaCompra
        {
            get;
            set;
        }

        public virtual IList<CompraProvItemViewModel> CompraItems
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion envío")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Inserta tu direccion para el envío")]

        public String DireccionEnvio
        {
            get;
            set;
        }

        [Display(Name = "Teléfono de contacto")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Inserta un teléfono de contacto")]
        [MaxLength(9, ErrorMessage = "Ese numero no existe"), MinLength(9)]
        public virtual string TelefonoContacto
        {
            get;
            set;
        }

        [Display(Name = "Método de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Selecciona un método de pago para la compra")]
        public String MetodoPago
        {
            get;
            set;
        }

        public CompraProvCreateViewModel()
        {

            CompraItems = new List<CompraProvItemViewModel>();
        }

        public Tarjeta_Credito TarjetaCredito { get; set; }
        public PayPal PayPal { get; set; }

        public override bool Equals(object obj)
        {
            CompraProvCreateViewModel purchase = obj as CompraProvCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == purchase.Nombre)
                && (this.PrimerApellido == purchase.PrimerApellido)
                && (this.SegundoApellido == purchase.SegundoApellido)
                && (this.IdUsuario == purchase.IdUsuario)
                && (this.PrecioTotal == purchase.PrecioTotal)
                && (this.DireccionEnvio == purchase.DireccionEnvio)
                && (this.TelefonoContacto == purchase.TelefonoContacto)
                //the timepsan is less than a minute between them
                && (this.FechaCompra.Subtract(purchase.FechaCompra) < new TimeSpan(0, 1, 0))
                && (
                   ((this.MetodoPago == null) && (purchase.MetodoPago == null))
                   || this.MetodoPago.Equals(purchase.MetodoPago)
                )
                );


            result = result && (this.CompraItems.Count == purchase.CompraItems.Count);


            for (i = 0; i < this.CompraItems.Count; i++)
                result = result && (this.CompraItems[i].Equals(purchase.CompraItems[i]));

            return result;
        }
    }

    public class CompraProvItemViewModel
    {
        public virtual int ProductoProvId
        {
            get;
            set;
        }


        [StringLength(50, ErrorMessage = "El nombre del producto no puede ser mayor de 50 caracteres.")]
        public virtual String NombreProductoProv
        {
            get;
            set;
        }

        [Display(Name = "Precio de la compra")]
        public virtual float PrecioCompra
        {
            get;
            set;
        }

        public virtual String Proveedor
        {
            get;
            set;
        }

        public virtual String TipoAnimal
        {
            get;
            set;
        }

        [Required]
        public virtual int Cantidad
        {
            get;
            set;
        }


        public override bool Equals(object obj)
        {
            CompraProvItemViewModel purchaseItem = obj as CompraProvItemViewModel;
            bool result = false;
            if ((ProductoProvId == purchaseItem.ProductoProvId)
                && (this.PrecioCompra == purchaseItem.PrecioCompra)
                    && (this.Cantidad == purchaseItem.Cantidad)
                    && (this.Proveedor == purchaseItem.Proveedor)
                    && (this.NombreProductoProv == purchaseItem.NombreProductoProv))
                result = true;
            return result;
        }
    }
}
