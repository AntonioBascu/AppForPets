using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Desing
{
    public class Tarjeta_Credito: Metodo_Pago
    {


        [RegularExpression(@"^[0-9]{16}$")]
        [Display(Name ="Tarjeta de Credito")]
        public virtual string NumeroTarjeta { get; set; }


        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CCV { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime FechaCaducidad { get; set; }
    }
}
