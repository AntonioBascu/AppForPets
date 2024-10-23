using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.EsteticaViewModels
{
    public class EsteticaCreateViewModel
    {
        // lo que queremos que mostremos para crear

        public EsteticaCreateViewModel()
        {
            Tarjeta_Credito = new Tarjeta_Credito();
            EsteticaItems = new List<EsteticaItemViewModel>();
        }
        
        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string Primer_Apellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string Segundo_Apellido
        {
            get;
            set;
        }

        // Se necesita para tener la relación de ApplicationUser con cualquier clase
        public string ClienteId
        {
            get;
            set;
        }

        [EmailAddress]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección de correo electrónico")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ponga su dirección de correo electrónico")]
        public String Direccion_correo
        {
            get;
            set;
        }


        [StringLength(9, MinimumLength = 9)]
        [Display(Name = "Teléfono de contacto")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ponga su teléfono de contacto")]
        public String telefono
        {
            get;
            set;
        }


        public int PrecioTotal
        {
            get;
            set;
        }

        public DateTime FechaCompra
        {
            get;
            set;
        }


        public virtual IList<EsteticaItemViewModel> EsteticaItems
        {
            get;
            set;
        }

   
        [Display(Name = "Método de Pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, selecciona un método de pago")]
        public String Metodo_Pago
        {
            get;
            set;
        }


        public Tarjeta_Credito Tarjeta_Credito { get; set; }
        public PayPal PayPal { get; set; }



        public override bool Equals(object obj)
        {
            EsteticaCreateViewModel estetica = obj as EsteticaCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == estetica.Nombre)
                && (this.Primer_Apellido  == estetica.Primer_Apellido)
                && (this.Segundo_Apellido == estetica.Segundo_Apellido)
                && (this.ClienteId == estetica.ClienteId)
                && (this.PrecioTotal == estetica.PrecioTotal)
                && (this.Direccion_correo == estetica.Direccion_correo)
                && (this.telefono == estetica.telefono)
                //the timepsan is less than a minute between them
                && (this.FechaCompra.Subtract(estetica.FechaCompra) < new TimeSpan(0, 1, 0))
                && (
                   ((this.Metodo_Pago == null) && (estetica.Metodo_Pago == null))
                   || this.Metodo_Pago.Equals(estetica.Metodo_Pago)
                )
                );


            result = result && (this.EsteticaItems.Count == estetica.EsteticaItems.Count);


            for (i = 0; i < this.EsteticaItems.Count; i++)
                result = result && (this.EsteticaItems[i].Equals(estetica.EsteticaItems[i]));

            return result;
        }

    }

    public class EsteticaItemViewModel
    {
        public virtual int ServicioId
        {
            get;
            set;
        }


        [StringLength(50, ErrorMessage = "El primer nombre no puede tener más de 50 caracteres.")]
        public virtual String Nombre_Servicio
        {
            get;
            set;
        }


        [Display(Name = "Precio de Servicio")]
        public virtual int PrecioCompra
        {
            get;
            set;
        }


        public virtual String Tipo_Servicio
        {
            get;
            set;
        }


        [Required]
        public virtual int Tiempo_Duracion
        {
            get;
            set;
        }



        public override bool Equals(object obj)
        {
            EsteticaItemViewModel esteticaItem = obj as EsteticaItemViewModel;
            bool result = false;
            if ((ServicioId == esteticaItem.ServicioId)
                && (this.PrecioCompra == esteticaItem.PrecioCompra)
                    && (this.Tiempo_Duracion == esteticaItem.Tiempo_Duracion)
                   && (this.Nombre_Servicio == esteticaItem.Nombre_Servicio))
                result = true;
            return result;
        }
    }
}
