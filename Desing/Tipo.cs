using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Desing
{
    public class Tipo
    {
        [Key]
        public virtual int TipoID {
            get;
            set;
        }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección animal")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "es necesario introducir un raza")]
        public virtual string Raza
        {
          get;
          set;
        }
        //Cantidad para controlar el stock
        [Display(Name = "Cantidad Disponible")]
        [Range(1, int.MaxValue, ErrorMessage = "Minima cantidad a comprar es 1")]
        public virtual int cantidadTipo 
        {
            get;
            set;
        }
    }
  
    }
