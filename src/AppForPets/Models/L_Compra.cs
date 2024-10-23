using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppForPets.Models
{
    public class L_Compra
    {
        [Key]
        public int ID 
        {
            get;
            set;
        }
        [Required]
        [Display(Name = "Cantidad Comprada")]
        [Range(1, int.MaxValue, ErrorMessage = "Minima cantidad a comprar es 1")]
        public virtual int Cantidad 
        {
            get;
            set;
        }
        public virtual int AnimalID 
        {
            get;
            set;
        }
        public virtual int CompraID 
        {
            get;
            set;
        }
        [Required]
        public virtual Animal Animal 
        {
            get;
            set;
        }
        [Required]
        public virtual Compra Compra
        {
            get;
            set;
        }
        public override bool Equals(Object obj)
        {
            L_Compra l_compra = obj as L_Compra;

            if ((this.Cantidad == l_compra.Cantidad) && (this.CompraID == l_compra.CompraID) && (this.AnimalID == l_compra.AnimalID)
               && (this.Animal.Equals(l_compra.Animal)))
                return true;
            return false;
        }
    }
}
