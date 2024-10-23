using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppForPets.Models
{
    public class Estetica
    {

        [Key]
        public virtual int EsteticaID {
            get;
            set;
        }


        public int PrecioTotal {
            get;
            set;
        }

        public DateTime FechaCompra {
            get;
            set;
        }

        
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 characters.")]
        [Display(Name = "Nombre del Servicio")]
        public virtual String Nombre_Servicio {
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

        // ------------------------- RELACIONES ----------------------------- //

        public Metodo_Pago Metodo_Pago {
            get;
            set;
        }


        public virtual Cliente Cliente {
            get;
            set;
        }

        //Es necesario para relacionar ApplicationUser con otras clases
        public string ClienteId {
            get;
            set;
        }
        public virtual IList<Linea_Servicio> Linea_Servicios {
            get;
            set;
        }



        public override bool Equals(object obj)
        {
            Estetica compra = obj as Estetica;
            int i;
            bool result = false;



            result = ((this.Cliente.UserName == compra.Cliente.UserName)
            && (this.Direccion_correo == compra.Direccion_correo)
            && (this.Metodo_Pago.Equals(compra.Metodo_Pago))
            && (this.FechaCompra.Subtract(compra.FechaCompra) < new TimeSpan(0, 1, 0)));



            result = result && (this.Linea_Servicios.Count == compra.Linea_Servicios.Count);



            for (i = 0; i < this.Linea_Servicios.Count; i++)
                result = result && (this.Linea_Servicios[i].Equals(compra.Linea_Servicios[i]));



            return result;
        }


    }
}
