using System;
using System.Collections.Generic;
using System.Text;
using AppForPets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AppForPets.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<Animal> Animal { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
        public DbSet<L_Compra> L_Compra { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Estetica> Estetica { get; set; }
        public DbSet<Linea_Servicio> Linea_Servicio { get; set; }

        public DbSet<AppForPets.Models.Tarjeta_Credito> Tarjeta_Creditos { get; set; }

        public DbSet<AppForPets.Models.PayPal> PayPal { get; set; }


        public DbSet<TipoAnimal> TipoAnimal { get; set; }


        public DbSet<Tipo_Servicio> Tipo_Servicio { get; set; }

        public DbSet<Producto> Producto { get; set; }

        public DbSet<Proveedor> Proveedor { get; set; }

        public DbSet<ProductoProveedor> ProductoProveedor { get; set; }
        public DbSet<AlquilarProductos> AlquilarProductos { get; set; }

        public DbSet<Alquilar> Alquilar { get; set; }

        public DbSet<CompraProvItem> CompraProvItem { get; set; }

        public DbSet<CompraProveedor> CompraProveedor { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}

