using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Desing
{
    public class Estetica
    {

        [Key]
        public virtual int EsteticaID {
            get;
            set;
        }


        public double PrecioTotal {
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

     
        public Metodo_Pago Metodo_Pago {
            get;
            set;
        }


        public virtual Cliente Cliente {
            get;
            set;
        }


        public virtual IList<Linea_Servicio> Linea_Servicios {
            get;
            set;
        }


      
        public override bool Equals(Object obj)
        {

            var estetica = obj as Estetica;

            if (null != estetica)
            {
                return this.EsteticaID == estetica.EsteticaID
                   && this.PrecioTotal == estetica.PrecioTotal
                   && this.FechaCompra == estetica.FechaCompra
                   && this.Nombre_Servicio == estetica.Nombre_Servicio
                   && this.Metodo_Pago == estetica.Metodo_Pago;
            }
            else
            {
                return false;
            }
        }


    }
}
