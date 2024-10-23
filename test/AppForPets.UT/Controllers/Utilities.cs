using AppForPets.Data;
using AppForPets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppForPets.UT.Controllers
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Crear un nuevo proveedor de servicios, y una nueva //instancia de la base de datos temporal:
            var serviceProvider = new ServiceCollection()
                      .AddEntityFrameworkInMemoryDatabase()
                      .BuildServiceProvider();
            // Crear una nueva instancia de opciones que use la BD temporal ofrecida por el proveedor de servicios:
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("AppForPets")
                           .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }

        // Inicialización Tipo_Servicio
        public static void InitializeDbTipoServiciosForTests(ApplicationDbContext db)
        {
            db.Tipo_Servicio.AddRange(GetTipoServicios(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbTipoServiciosForTests(ApplicationDbContext db)
        {
            db.Tipo_Servicio.RemoveRange(db.Tipo_Servicio);
            db.SaveChanges();
        }


        // Inicialización Servicios
        public static void InitializeDbServiciosForTests(ApplicationDbContext db)
        {
            db.Servicio.AddRange(GetServicios(0, 3));
            //genre id=1 it is already added because it is related to the movies
            //db.Tipo_Servicio.AddRange(GetTipoServicios(1, 2));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });
            db.SaveChanges();
        }

        public static void ReInitializeDbServiciosForTests(ApplicationDbContext db)
        {
            db.Servicio.RemoveRange(db.Servicio);
            db.Tipo_Servicio.RemoveRange(db.Tipo_Servicio);
            db.SaveChanges();
        }


        // Inicialización del usuario

        public static void InitializeDbUsersForTests(ApplicationDbContext db)
        {

            db.Users.AddRange(GetUsers(0, 2));
            db.SaveChanges();
        }

        public static void ReInitializeDbUsersForTests(ApplicationDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }


        // Inicialización Estética
        public static void InitializeDbPEsteticasForTests(ApplicationDbContext db)
        {
            var esteticas = GetEsteticas(0, 1);
            foreach (Estetica estetica in esteticas)
            {
                db.Estetica.Add(estetica as Estetica);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbEsteticasForTests(ApplicationDbContext db)
        {
            db.Linea_Servicio.RemoveRange(db.Linea_Servicio);
            db.Estetica.RemoveRange(db.Estetica);
            db.SaveChanges();
        }

        // ILIST Estetica
        public static IList<Estetica> GetEsteticas(int index, int numOfEsteticas)
        {

            Cliente cliente = GetUsers(0, 1).First() as Cliente;
            var allEsteticas = new List<Estetica>();
            Estetica estetica;
            Servicio servicio;
            Linea_Servicio lineaServicio;
            //int tiempoduracion = 1;

            for (int i = 1; i < 3; i++)
            {
                servicio = GetServicios(i - 1, 1).First();
                //servicio.Tiempo_Duracion = servicio.Tiempo_Duracion - tiempoduracion;

                estetica = new Estetica
                {
                    EsteticaID = i,
                    Cliente = cliente,
                    PrecioTotal = servicio.Precio_Servicio,
                    FechaCompra = System.DateTime.Now,
                    Nombre_Servicio = servicio.Nombre_Servicio,
                    Metodo_Pago = GetMetodoPago(i - 1, 1).First(),
                    ClienteId = cliente.Id,
                    Linea_Servicios = new List<Linea_Servicio>()
                };

                lineaServicio = new Linea_Servicio
                {
                    LineaServicioID = i,
                    Turno_Servicio = servicio.Tiempo_Duracion,
                    Servicio = servicio,
                    ServicioId = servicio.ServicioID,
                    Estetica = estetica,
                    EsteticaId = estetica.EsteticaID

                };
                estetica.Linea_Servicios.Add(lineaServicio);
                estetica.PrecioTotal = lineaServicio.Servicio.Precio_Servicio * servicio.Tiempo_Duracion;
                allEsteticas.Add(estetica);

            }

            return allEsteticas.GetRange(index, numOfEsteticas);
        }

        // ILIST MÉTODO DE PAGO
        public static IList<Metodo_Pago> GetMetodoPago(int index, int numOfMetodoPago)
        {
            Cliente cliente = GetUsers(0, 1).First() as Cliente;
            var allPaymentMethods = new List<Metodo_Pago>
                {
                new Tarjeta_Credito {ID = 1, NumeroTarjeta = "1111111111111111", CCV = "111", FechaCaducidad = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, Email = cliente.Email, Prefijo = "+34", Telefono = cliente.PhoneNumber },

            };
            //return from the list as much instances as specified in numOfGenres
            return allPaymentMethods.GetRange(index, numOfMetodoPago);
        }

        // ILIST TIPO DE SERVICIO
        public static IList<Tipo_Servicio> GetTipoServicios(int index, int numOfTipoServicio)
        {
            var allTipoServicios = new List<Tipo_Servicio>
                {
                    new Tipo_Servicio { ID=1, Nombre = "Peluqueria" } ,
                    new Tipo_Servicio { ID=2, Nombre = "Tratamientos" },
                    new Tipo_Servicio { ID=3, Nombre = "Lavados" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allTipoServicios.GetRange(index, numOfTipoServicio);
        }


        // ILIST SERVICIOS
        public static IList<Servicio> GetServicios(int index, int numOfServicios)
        {
            //Tipo_Servicio tipoServicio = GetTipoServicios(0, 1).First();

            var allServicios = new List<Servicio>
        {

             new Servicio { ServicioID = 1, Nombre_Servicio = "Peluqueria Canina", Precio_Servicio = 10,
                 Tiempo_Duracion = 1, Cantidad_servicio = 4, Tipo_Servicio= GetTipoServicios(0,1).First(), },

            new Servicio { ServicioID = 2, Nombre_Servicio = "Tratamiento Antiparasitos", Precio_Servicio = 25,
                 Tiempo_Duracion = 1, Cantidad_servicio = 5, Tipo_Servicio= GetTipoServicios(1,1).First(), },

            new Servicio { ServicioID = 3, Nombre_Servicio = "Lavado Gatuno", Precio_Servicio = 15,
                 Tiempo_Duracion = 1, Cantidad_servicio = 2, Tipo_Servicio= GetTipoServicios(2,1).First(), },
        };

            return allServicios.GetRange(index, numOfServicios);
        }

        // Ilist Usuario
        public static IList<ApplicationUser> GetUsers(int index, int numOfUsers)
        {
            var allUsers = new List<ApplicationUser>
                {
                   new Cliente { Id = "1", UserName = "peter@uclm.com",
                       PhoneNumber = "967959595",  Email = "peter@uclm.com",
                       Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" },

                   new ApplicationUser { Id = "2", UserName = "elena@uclm.com",
                       PhoneNumber = "967959595",  Email = "elena@uclm.com",
                       Nombre = "Elena", PrimerApellido = "Navarro", SegundoApellido = "Martínez" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allUsers.GetRange(index, numOfUsers);
        }
        public static void InitializeDbTiposForTests(ApplicationDbContext db)
        {
            db.Tipo.AddRange(GetTipos(0, 2));
            db.SaveChanges();

        }
        public static void ReInitializeDbTiposForTests(ApplicationDbContext db)
        {
            db.Tipo.RemoveRange(db.Tipo);
            db.SaveChanges();
        }
        public static void InitializeDbAnimalesForTests(ApplicationDbContext db)
        {

            db.Animal.AddRange(GetAnimales(0, 3));
            //genre id=1 it is already added because it is related to the movies
            // db.Tipo.AddRange(GetTipos(1, 2));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });
            db.SaveChanges();
        }
        public static void ReInitializeDbAnimalesForTests(ApplicationDbContext db)
        {
            db.Animal.RemoveRange(db.Animal);
            db.Tipo.RemoveRange(db.Tipo);
            db.SaveChanges();
        }
        public static void InitializeDbComprasForTests(ApplicationDbContext db)
        {
            var compras = GetCompras(0, 1);
            foreach (Compra compra in compras)
            {
                db.Compra.Add(compra as Compra);
            }
            db.SaveChanges();

        }
        public static void ReInitializeDbComprasForTests(ApplicationDbContext db)
        {
            db.L_Compra.RemoveRange(db.L_Compra);
            db.Compra.RemoveRange(db.Compra);
            db.Animal.RemoveRange(db.Animal);
            db.Tipo.RemoveRange(db.Tipo);
            db.SaveChanges();
        }
        public static IList<Compra> GetCompras(int index, int numOfCompras)
        {

            Cliente cliente = GetUsers(0, 1).First() as Cliente;
            var allCompras = new List<Compra>();
            Compra compra;
            Animal animal;
            L_Compra l_compra;
            int cantidad = 2;

            for (int i = 1; i < 3; i++)
            {
                animal = GetAnimales(i - 1, 1).First();
                animal.Cantidad = animal.Cantidad - cantidad;
                compra = new Compra
                {
                    CompraID = i,
                    Cliente = cliente,
                    ClienteId = cliente.Id,
                    DirecionEnvio = "Avd. España s/n",
                    MetodoPago = GetMetodoPago(i - 1, 1).First(),
                    FechaCompra = System.DateTime.Now,
                    PrecioTotal = animal.Precio,
                    L_Compras = new List<L_Compra>()
                };
                l_compra = new L_Compra
                {
                    ID = i,
                    Cantidad = cantidad,
                    Animal = animal,
                    AnimalID = animal.AnimalID,
                    Compra = compra,
                    CompraID = compra.CompraID

                };
                compra.L_Compras.Add(l_compra);
                compra.PrecioTotal = l_compra.Cantidad * l_compra.Animal.Precio;
                allCompras.Add(compra);

            }

            return allCompras.GetRange(index, numOfCompras);
        }
        public static IList<Animal> GetAnimales(int index, int numOfAnimales)
        {
            //Tipo tipo = GetTipos(0, 1).First();
            var allAnimal = new List<Animal>
            {
                new Animal { AnimalID = 1, Precio= 10, Cantidad = 4, Edad = 1, Tipo= GetTipos(0,1).First()},

                new Animal { AnimalID = 2, Precio= 20, Cantidad = 5, Edad = 1, Tipo=  GetTipos(1,1).First()},

                new Animal { AnimalID = 3, Precio= 30, Cantidad = 2, Edad = 1, Tipo=  GetTipos(2,1).First()}

            };
            return allAnimal.GetRange(index, numOfAnimales);
        }
        public static IList<Tipo> GetTipos(int index, int numOfTipos)
        {
            var allTipos = new List<Tipo>
                {
                    new Tipo { TipoID=1, Raza = "golden" } ,
                    new Tipo { TipoID=2, Raza = "border collie" },
                    new Tipo { TipoID=3, Raza = "pastor aleman" },
                };
            //return from the list as much instances as specified in numOfGenres
            return allTipos.GetRange(index, numOfTipos);
        }

        public static void InitializeDbProveedoresForTests(ApplicationDbContext db)
        {
            db.Proveedor.AddRange(GetProveedores(0, 2));
            db.SaveChanges();

        }

        public static void ReInitializeDbProveedoresForTests(ApplicationDbContext db)
        {
            db.Proveedor.RemoveRange(db.Proveedor);
            db.SaveChanges();
        }

        public static void InitializeDbTipoAnimalesForTests(ApplicationDbContext db)
        {
            db.TipoAnimal.AddRange(GetTipoAnimales(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbTipoAnimalesForTests(ApplicationDbContext db)
        {
            db.TipoAnimal.RemoveRange(db.TipoAnimal);
            db.SaveChanges();
        }

        public static void InitializeDbProductosPForTests(ApplicationDbContext db)
        {
            db.Producto.AddRange(GetProductosP(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbProductosPForTests(ApplicationDbContext db)
        {
            db.Producto.RemoveRange(db.Producto);
            db.SaveChanges();
        }

        public static void InitializeDbProductosProveedorForTests(ApplicationDbContext db)
        {
            db.ProductoProveedor.AddRange(GetProductosProveedor(0, 3));

            db.SaveChanges();

            db.Users.Add(new ApplicationUser
            {
                Id = "2",
                UserName = "elena@uclm.com",
                PhoneNumber = "967959595",
                Email = "elena@uclm.com",
                Nombre = "Elena",
                PrimerApellido = "Navarro",
                SegundoApellido = "Martínez"
            });
            db.SaveChanges();
        }

        public static void ReInitializeDbProductosProveedorForTests(ApplicationDbContext db)
        {
            db.TipoAnimal.RemoveRange(db.TipoAnimal);
            db.Producto.RemoveRange(db.Producto);
            db.Proveedor.RemoveRange(db.Proveedor);
            db.ProductoProveedor.RemoveRange(db.ProductoProveedor);
            db.SaveChanges();
        }

        public static void InitializeDbComprasProveedorForTests(ApplicationDbContext db)
        {
            var compras = GetComprasProveedor(0, 1);
            foreach (CompraProveedor compra in compras)
            {
                db.CompraProveedor.Add(compra as CompraProveedor);
            }
            db.SaveChanges();
        }

        public static void ReInitializeDbComprasProveedorForTests(ApplicationDbContext db)
        {
            db.CompraProvItem.RemoveRange(db.CompraProvItem);
            db.CompraProveedor.RemoveRange(db.CompraProveedor);
            db.SaveChanges();
        }

        public static IList<Producto> GetProductosP(int index, int numOfMovies)
        {
            //TipoAnimal tipoAnimal = GetTipoAnimales(0, 1).First();
            var allProductos = new List<Producto>
            {
                new Producto
                {
                    ProductoID = 1,
                    NombreProducto = "Correa de cuello",
                    PrecioAlquiler = 3,
                    Precio = 10,
                    CantidadAlquilar = 12,
                    TipoAnimal = GetTipoAnimales(0, 1).First()
                },
                new Producto
                {
                    ProductoID = 2,
                    NombreProducto = "Caseta",
                    PrecioAlquiler = 6,
                    Precio = 25,
                    CantidadAlquilar = 8,
                    TipoAnimal = GetTipoAnimales(1, 1).First()
                },
                new Producto
                {
                    ProductoID = 3,
                    NombreProducto = "Pelota Goma",
                    PrecioAlquiler = 2,
                    Precio = 8,
                    CantidadAlquilar = 20,
                    TipoAnimal = GetTipoAnimales(2, 1).First()
                }
            };

            return allProductos.GetRange(index, numOfMovies);
        }

        public static IList<Proveedor> GetProveedores(int index, int numOfMovies)
        {
   
            var allProveedores = new List<Proveedor>
            {
                new Proveedor
                {
                    IdProveedor = 1,
                    Nombre = "Perrefersa",
                    Direccion = "Cuenca, C/Menor 57",
                    CorreoE = "perrefersa@perrefersa.es",
                    Telefono = "612 341 234"
                },
                new Proveedor
                {
                    IdProveedor = 2,
                    Nombre = "ZooPlus",
                    Direccion = "Murcia, C/Baños 77",
                    CorreoE = "ZooPlus@ZooPlus.es",
                    Telefono = "644 191 933"
                },
                new Proveedor
                {
                    IdProveedor = 3,
                    Nombre = "CatWorld",
                    Direccion = "Albacete, C/David Roncero 1",
                    CorreoE = "CatWorld@CatWorld.es",
                    Telefono = "685 341 468"
                }
            };

            return allProveedores.GetRange(index, numOfMovies);
        }

        public static IList<ProductoProveedor> GetProductosProveedor(int index, int numOfMovies)
        {
            
            var allProductosProv = new List<ProductoProveedor>
            {
                new ProductoProveedor
                {
                    IdProductoProv = 1,
                    Nombre = "Correa de cuello",
                    Precio = 6,
                    CantidadDisponible = 300,
                    Producto = GetProductosP(0, 1).First(),
                    Proveedor = GetProveedores(0, 1).First(),
                },
                new ProductoProveedor
                {
                    IdProductoProv = 2,
                    Nombre = "Caseta",
                    Precio = 20,
                    CantidadDisponible = 100,
                    Producto = GetProductosP(1, 1).First(),
                    Proveedor = GetProveedores(1, 1).First(),
                },
                new ProductoProveedor
                {
                    IdProductoProv = 3,
                    Nombre = "Pelota Goma",
                    Precio = 3,
                    CantidadDisponible = 300,
                    Producto = GetProductosP(2, 1).First(),
                    Proveedor = GetProveedores(2, 1).First(),
                }/*,
                new ProductoProveedor
                {
                    IdProductoProv = 4,
                    Nombre = "Correa de cuello",
                    Precio = 7,
                    CantidadDisponible = 200,
                    Producto = GetProductosP(0, 1).First(),
                    Proveedor = GetProveedores(1, 1).First(),
                },
                new ProductoProveedor
                {
                    IdProductoProv = 5,
                    Nombre = "Pelota Goma",
                    Precio = 4,
                    CantidadDisponible = 500,
                    Producto = GetProductosP(2, 1).First(),
                    Proveedor = GetProveedores(1, 1).First(),
                }*/
            };

            return allProductosProv.GetRange(index, numOfMovies);
        }

        public static IList<TipoAnimal> GetTipoAnimales(int index, int numOfTipoAnimales)
        {
            var allTipoAnimales = new List<TipoAnimal>
                {
                    new TipoAnimal { TipoAnimalID=1, NombreAnimal= "Perro" } ,
                    new TipoAnimal { TipoAnimalID=2, NombreAnimal= "Gato" } ,
                    new TipoAnimal { TipoAnimalID=3, NombreAnimal= "Pajaro" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allTipoAnimales.GetRange(index, numOfTipoAnimales);
        }

        public static IList<CompraProveedor> GetComprasProveedor(int index, int numOfPurchases)
        {

            ApplicationUser usuario = GetUsers(1, 1).First() as ApplicationUser;
            var allCompras = new List<CompraProveedor>();
            CompraProveedor compra;
            ProductoProveedor producto;
            CompraProvItem compraItem;
            int cantidad = 30;

            for (int i = 1; i < 3; i++)
            {
                producto = GetProductosProveedor(i-1, 1).First();
                producto.CantidadDisponible = producto.CantidadDisponible - cantidad;
                compra = new CompraProveedor { IdCompraProveedor = i, Usuario = usuario, DireccionEnvio = "Avd. España s/n", MetodoPago = GetMetodoPago(i-1, 1).First(), FechaCompra = System.DateTime.Now, PrecioTotal = producto.Precio, CompraItems = new List<CompraProvItem>(), TelefonoContacto = "967959595" };
                compraItem = new CompraProvItem { ProductoProveedor = producto, Compra = compra, Cantidad = cantidad };
                compra.CompraItems.Add(compraItem);
                compra.PrecioTotal = compraItem.Cantidad * compraItem.ProductoProveedor.Precio;
                allCompras.Add(compra);

            }

            return allCompras.GetRange(index, numOfPurchases);
        }

        public static void InitializeDbTipoAnimalsForTests(ApplicationDbContext db)
        {
            db.TipoAnimal.AddRange(GetTipoAnimals(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbTipoAnimalsForTests(ApplicationDbContext db)
        {
            db.TipoAnimal.RemoveRange(db.TipoAnimal);
            db.SaveChanges();
        }

        public static void InitializeDbProductosForTests(ApplicationDbContext db)
        {

            db.Producto.AddRange(GetProductos(0, 3));
            //Tipo animal id=1 it is already added because it is related to the movies
            db.TipoAnimal.AddRange(GetTipoAnimals(1, 2));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });
            db.SaveChanges();
        }

        public static void ReInitializeDbProductosForTests(ApplicationDbContext db)
        {
            db.Producto.RemoveRange(db.Producto);
            db.TipoAnimal.RemoveRange(db.TipoAnimal);
            db.SaveChanges();
        }

        public static void InitializeDbClientesForTests(ApplicationDbContext db)
        {

            db.Users.Add(GetUsers(0, 1).First());
            db.SaveChanges();
        }

        public static void ReInitializeDbClientesForTests(ApplicationDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }

        public static void InitializeDbAlquilarsForTests(ApplicationDbContext db)
        {
            var alquilars = GetAlquilars(0, 1);
            foreach (Alquilar alquilar in alquilars)
            {
                db.Alquilar.Add(alquilar as Alquilar);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbAlquilarsForTests(ApplicationDbContext db)
        {
            db.AlquilarProductos.RemoveRange(db.AlquilarProductos);
            db.Alquilar.RemoveRange(db.Alquilar);
            db.SaveChanges();
        }

        public static IList<Alquilar> GetAlquilars(int index, int numOfAlquilars)
        {

            Cliente cliente = GetUsers(0, 1).First() as Cliente;
            var allAlquilars = new List<Alquilar>();
            Alquilar alquilar;
            Producto producto;
            AlquilarProductos alquilarProductos;
            int cantidad = 2;

            for (int i = 1; i < 4; i++)
            {
                producto = GetProductos(i - 1, 1).First();
                producto.CantidadAlquilar = producto.CantidadAlquilar - cantidad;
                alquilar = new Alquilar
                {
                    AlquilarID = i,
                    Cliente = cliente,
                    ClienteId = "1",
                    FechaInicio = System.DateTime.Now,
                    FechaFin = System.DateTime.Now,
                    PrecioTotal = producto.PrecioAlquiler,
                    AlquilarProductos = new List<AlquilarProductos>()
                };
                alquilarProductos = new AlquilarProductos
                {
                    Id = i,
                    Producto = producto,
                    Alquilar = alquilar,
                    Cantidad = cantidad
                };
                alquilar.AlquilarProductos.Add(alquilarProductos);
                alquilar.PrecioTotal = alquilarProductos.Cantidad * alquilarProductos.Producto.PrecioAlquiler;
                allAlquilars.Add(alquilar);

            }

            return allAlquilars.GetRange(index, numOfAlquilars);
        }


        public static IList<Producto> GetProductos(int index, int numOfProductos)
        {
            TipoAnimal tipoAnimal = GetTipoAnimals(0, 1).First();
            var allProductos = new List<Producto>
            {
                new Producto { ProductoID = 1, NombreProducto = "Cama delux", PrecioAlquiler = 15,Precio= 10,TipoAnimal= tipoAnimal, CantidadAlquilar= 5, },
                new Producto { ProductoID = 2, NombreProducto = "Cama superdelux", PrecioAlquiler = 25,Precio= 15,TipoAnimal= tipoAnimal, CantidadAlquilar= 3, },
                new Producto { ProductoID = 3, NombreProducto = "Cama megadelux", PrecioAlquiler = 35,Precio= 20,TipoAnimal= tipoAnimal, CantidadAlquilar= 4, },
                new Producto { ProductoID = 4, NombreProducto = "Cama ultradelux", PrecioAlquiler = 50,Precio= 30,TipoAnimal= tipoAnimal, CantidadAlquilar= 2, }

            };

            return allProductos.GetRange(index, numOfProductos);
        }

        public static IList<TipoAnimal> GetTipoAnimals(int index, int numOfTipoAnimals)
        {
            var allTipoAnimals = new List<TipoAnimal>
                {
                    new TipoAnimal { TipoAnimalID=1, NombreAnimal= "Perro" } ,
                    new TipoAnimal { TipoAnimalID=2, NombreAnimal= "Pájaro" } ,
                    new TipoAnimal { TipoAnimalID=3, NombreAnimal= "Pez" } ,
                    new TipoAnimal { TipoAnimalID=4, NombreAnimal= "Gato" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allTipoAnimals.GetRange(index, numOfTipoAnimals);
        }

    }
}
