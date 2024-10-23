using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Desing
{
    public class CompraProvItem
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        [ForeignKey("IdProductoProveedor")]
        public virtual ProductoProveedor ProductoProveedor
        {
            get;
            set;
        }

        public virtual int Cantidad
        {
            get;
            set;
        }

        [ForeignKey("IdCompra")]
        public virtual CompraProveedor Compra
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            CompraProvItem purchaseItem = obj as CompraProvItem;
            if ((this.Cantidad == purchaseItem.Cantidad) && (this.Compra == purchaseItem.Compra) && (this.ProductoProveedor.Equals(purchaseItem.ProductoProveedor)))
                return true;
            return false;
        }




    }
}

