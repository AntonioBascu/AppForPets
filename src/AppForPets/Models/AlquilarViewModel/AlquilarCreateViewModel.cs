using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppForPets.Models.AlquilarViewModel
{
    public class AlquilarCreateViewModel
    {

        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer apellido")]
        public virtual string PrimerApellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string SegundoApellido
        {
            get;
            set;
        }

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string ClienteId
        {
            get;
            set;
        }

        public double PrecioTotal
        {
            get;
            set;
        }

        public DateTime FechaInicio
        {
            get;
            set;
        }

        public DateTime FechaFin
        {
            get;
            set;
        }

        public virtual IList<AlquilarProductoViewModel> AlquilarProductos
        {
            get;
            set;
        }

  

        public AlquilarCreateViewModel()
        {

            AlquilarProductos = new List<AlquilarProductoViewModel>();
        }
        //public CreditCard CreditCard { get; set; }
        public PayPal PayPal { get; set; }

        public override bool Equals(object obj)
        {
            AlquilarCreateViewModel alquilar = obj as AlquilarCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == alquilar.Nombre)
                && (this.PrimerApellido == alquilar.PrimerApellido)
                && (this.SegundoApellido == alquilar.SegundoApellido)
                && (this.ClienteId == alquilar.ClienteId)
                && (this.PrecioTotal == alquilar.PrecioTotal)
                
                //the timepsan is less than a minute between them
                && (this.FechaInicio.Subtract(alquilar.FechaInicio) < new TimeSpan(0, 1, 0))
                
                );

            result = result && (this.AlquilarProductos.Count == alquilar.AlquilarProductos.Count);

            for (i = 0; i < this.AlquilarProductos.Count; i++)
                result = result && (this.AlquilarProductos[i].Equals(alquilar.AlquilarProductos[i]));

            return result;
        }
    }

    public class AlquilarProductoViewModel
    {
        public virtual int ProductoID
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public virtual String NombreProducto
        {
            get;
            set;
        }

        [Display(Name = "Price For Purchase")]
        public virtual int PrecioAlquiler
        {
            get;
            set;
        }

        public virtual String TipoAnimal
        {
            get;
            set;
        }

        [Required]
        public virtual int CantidadAlquiler
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            AlquilarProductoViewModel alquilarProducto = obj as AlquilarProductoViewModel;
            bool result = false;
            if ((ProductoID == alquilarProducto.ProductoID)
                && (this.PrecioAlquiler == alquilarProducto.PrecioAlquiler)
                    && (this.CantidadAlquiler == alquilarProducto.CantidadAlquiler)
                    && (this.NombreProducto == alquilarProducto.NombreProducto))
                result = true;
            return result;
        }
    }
}


