//using Microsoft.AspNetCore.Mvc;
using Desing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Desing
{
    public class Alquilar
    {
        [Key]
        public virtual int AlquilarID
        {
            get;
            set;
        }


        public virtual Cliente Cliente
        {
            get;
            set;
        }


        [DataType(DataType.Currency)]
        public virtual int PrecioTotal
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Pickup Date")]
        public virtual DateTime FechaInicio
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Return Date")]
        public virtual DateTime FechaFin
        {
            get;
            set;
        }

        public virtual IList<AlquilarProductos> AlquilarProductos
        {
            get;
            set;
        }

        public Alquilar()
        {
            AlquilarProductos = new List<AlquilarProductos>();
        }

    }
}
