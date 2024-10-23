using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.CompraProveedorViewModels
{
    public class SelectProveedorForCompraViewModel
    {
        public IEnumerable<Proveedor> Proveedores { get; set; }

        //needed to store the genre selected by the user
        [Display(Name = "Dirección")]
        public string direccionSeleccionada { get; set; }

        [Display(Name = "Nombre")]
        public string nombreProveedor { get; set; }
    }
}
