using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.EsteticaViewModels
{
    public class SelectServiciosForEsteticaViewModels
    {
        public IEnumerable<Servicio> Servicios { get; set; }
        
        //Utilizado para filtrar por Tipo de Servicio
        public SelectList Tipo_Servicios;

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
      
        //Utilizado para filtrar por tiempo de duración
        [Display(Name = "Tiempo Duracion")]
        public int Tiempo_Duracion { get; set; }
    }
}
