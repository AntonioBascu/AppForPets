using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Desing
{
    public class Servicio
    {

        [Key]
        public virtual int ServicioID {
            get;
            set;
        }


        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        [Display(Name = "Nombre del Servicio")]
        public virtual String Nombre_Servicio {
            get;
            set;
        }


        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 100, ErrorMessage = "El mínimo es 10 y 100, respectivamente")]
        [Display(Name = "Precio del Servicio")]
        public virtual int Precio_Servicio {
            get;
            set;
        }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tiempo de duracion del servicio")]
        public virtual DateTime Tiempo_Duracion {
            get;
            set;
        }

        [Required]
        [Display(Name = "Cantidad del servicio disponible")]
        public virtual int Cantidad_servicio {
            get;
            set;
        }


        public virtual IList<Linea_Servicio> Linea_Servicios {
            get;
            set;
        }


        [Required]
        public virtual Tipo_Servicio Tipo_Servicio {
            get;
            set;
        }


        public override bool Equals(Object obj)
        {

            var servicio = obj as Servicio;

            if (null != servicio)
            {
                return this.ServicioID == servicio.ServicioID
                   && this.Nombre_Servicio == servicio.Nombre_Servicio
                   && this.Precio_Servicio == servicio.Precio_Servicio
                   && this.Tiempo_Duracion == servicio.Tiempo_Duracion
                   && this.Tipo_Servicio.Nombre == servicio.Tipo_Servicio.Nombre
                   && this.Cantidad_servicio == servicio.Cantidad_servicio;
            }
            else
            {
                return false;
            }
        }


    }
}
