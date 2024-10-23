using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.AlquilarViewModel
{
    public class SelectProductosForAlquilerViewModel
    {
        public IEnumerable<Producto> Productos{ get; set; }
        //Utilizado para filtrar por tipo de animal
        public SelectList TipoAnimal;
        [Display(Name = "TipoAnimal")]
        public string productoAnimalSelected { get; set; }
        //Utilizado para filtrar por título de producto
        [Display(Name = "Nombre")]
        public string productoNombre { get; set; }
    }
}
