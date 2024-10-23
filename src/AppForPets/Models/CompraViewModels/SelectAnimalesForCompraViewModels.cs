using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.CompraViewModels
{
    public class SelectAnimalesForCompraViewModels
    {
        //crear una enumeracion de animales 
        public IEnumerable<Animal> Animales { get; set; }
        //public SelectList Tipos;
        //Para filtrar 
        [Display(Name = "Raza")]
        public string TipoAnimal { get; set; }

        [Display(Name = "Precio")]
        public double precio { get; set; }
    }
}
