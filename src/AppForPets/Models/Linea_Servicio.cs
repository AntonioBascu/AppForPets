using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Text;

namespace AppForPets.Models
{
    public class Linea_Servicio
    {

        [Key]
        public virtual int LineaServicioID {
            get;
            set;
        }


        //[Required]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        //[Display(Name = "Fecha Servicio")]
        //public virtual DateTime Fecha_Servicio {
        //    get;
        //    set;
        //}


        [Required]
        [Range(8, 19, ErrorMessage = "Hora inicio 8h y hora último servicio 19h")]
        [Display(Name = "Turno para el servicio, desde las 8 hasta las 19, de forma ininterrumpida ")]
        public virtual int Turno_Servicio {
            get;
            set;
        }

        public virtual Estetica Estetica {
            get;
            set;
        }

        public virtual int EsteticaId {
            get;
            set;
        }

        public virtual Servicio Servicio {
            get;
            set;
        }

        public virtual int ServicioId {
            get;
            set;
        }

        // Equals
        public override bool Equals(Object obj)
        {
            Linea_Servicio l_servicio = obj as Linea_Servicio;
            if ((this.LineaServicioID == l_servicio.LineaServicioID)
                //&& this.Fecha_Servicio.Subtract(l_servicio.Fecha_Servicio) < new TimeSpan(0, 1, 0)
                && (this.Turno_Servicio == l_servicio.Turno_Servicio))

                return true;
            return false;
        }

    }
}
