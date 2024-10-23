using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;

namespace AppForPets.Models
{
    public class Animal
    {
        [Key]
        public virtual int AnimalID 
        {
            get;
            set;
        }
        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 100, ErrorMessage = "Minimum is 1 and 100, respectively")]
        [Display(Name = "Precio animal")]
        public virtual double Precio
        {
            get;
            set;
        }
        [Required]
        [Display(Name = "Cantidad a comprar")]
        [Range(1, int.MaxValue, ErrorMessage = "Minima cantidad a comprar es 1")]
        public virtual int Cantidad 
        {
            get;
            set;
        }
        [Required]
        public virtual int Edad
        {
            get;
            set;
        }
        [Required]
        public virtual Tipo Tipo 
        {
            get;
            set;
        }
        public virtual IList<L_Compra> L_Compras 
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            var myObject = obj as Animal;
            if (null != myObject)
            {
                return this.Precio == myObject.Precio
                    && this.Cantidad == myObject.Cantidad
                    && this.Edad == myObject.Edad
                    && this.Precio == myObject.Precio
                    && this.Tipo.Raza == myObject.Tipo.Raza;
            }
            else
            {
                return false;
            }  
        }
    }
}
