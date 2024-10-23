using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Text;

namespace Desing
{
    public class Linea_Servicio
    {

        [Key]
        public virtual int LineaServicioID {
            get;
            set;
        }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha Servicio")]
        public virtual DateTime Fecha_Servicio {
            get;
            set;
        }


        [Required]
        [DataType(DataType.Currency)]
        [Range(8, 19, ErrorMessage = "Hora inicio 8h y hora último servicio 19h")]
        [Display(Name = "Turno para el servicio, desde las 8 hasta las 19, de forma ininterrumpida ")]
        public virtual int Turno_Servicio {
            get;
            set;
        }


        // Relación con clase Estética
        public virtual Estetica Estetica {
            get;
            set;
        }


        // Relación con clase Servicio
        public virtual Servicio Servicio {
            get;
            set;
        }


        // Equals
        public override bool Equals(Object obj)
        {
            Linea_Servicio l_servicio = obj as Linea_Servicio;
            if ((this.LineaServicioID == l_servicio.LineaServicioID)
                && (this.Fecha_Servicio == l_servicio.Fecha_Servicio)
                && (this.Turno_Servicio == l_servicio.Turno_Servicio))

                return true;
            return false;
        }

    }
}
