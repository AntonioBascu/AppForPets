using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.CompraProveedorViewModels
{
    public class SelectProductosProveedorForCompraViewModel
    {
            public IEnumerable<ProductoProveedor> Productos { get; set; }

            public SelectList TipoAnimales;

            [Display(Name = "TipoAnimal")]
            public string tipoAnimalSelect { get; set; }

            [Display(Name = "Nombre")]
            public string NombreProducto { get; set; }

            [Display(Name = "Proveedor")]
            public string proveedor { get; set; }
    }
}
