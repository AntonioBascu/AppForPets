SET IDENTITY_INSERT [dbo].[TipoAnimal] ON
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (1, N'Perro')
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (2, N'Gato')
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (3, N'Pajaro')
SET IDENTITY_INSERT [dbo].[TipoAnimal] OFF

SET IDENTITY_INSERT [dbo].[Producto] ON
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [Precio], [TipoAnimalID], [CantidadAlquilar], [PrecioAlquiler]) VALUES (1, N'Correa de cuello', 5, 1, 20, 1)
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [Precio], [TipoAnimalID], [CantidadAlquilar], [PrecioAlquiler]) VALUES (2, N'Arenero', 20, 2, 10, 4)
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [Precio], [TipoAnimalID], [CantidadAlquilar], [PrecioAlquiler]) VALUES (3, N'Jaula metal', 30, 3, 8, 6)
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [Precio], [TipoAnimalID], [CantidadAlquilar], [PrecioAlquiler]) VALUES (4, N'Pienso P', 25, 1, 0, 0)
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [Precio], [TipoAnimalID], [CantidadAlquilar], [PrecioAlquiler]) VALUES (5, N'Pienso G', 15, 2, 0, 0)
SET IDENTITY_INSERT [dbo].[Producto] OFF

SET IDENTITY_INSERT [dbo].[Proveedor] ON
INSERT INTO [dbo].[Proveedor] ([IdProveedor], [Nombre], [Direccion], [CorreoE], [Telefono]) VALUES (1, N'Perrefersa', N'Cuenca, C/Menor 57', N'perrefersa@perrefersa.es', N'612341234')
INSERT INTO [dbo].[Proveedor] ([IdProveedor], [Nombre], [Direccion], [CorreoE], [Telefono]) VALUES (2, N'CatWorld', N'Albacete, C/David Roncero 1', N'CatWorld@CatWorld.es', N'685341468')
INSERT INTO [dbo].[Proveedor] ([IdProveedor], [Nombre], [Direccion], [CorreoE], [Telefono]) VALUES (3, N'ZooPlus', N'Murcia, C/Baños 77', N'ZooPlus@ZooPlus.es', N'644191933')
SET IDENTITY_INSERT [dbo].[Proveedor] OFF

SET IDENTITY_INSERT [dbo].[ProductoProveedor] ON
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (1, N'Correa de cuello', 3, 100, 1, 1)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (2, N'Correa de cuello', 4, 70, 1, 2)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (3, N'Correa de cuello', 2, 150, 1, 3)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (4, N'Arenero', 15, 50, 2, 2)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (5, N'Arenero', 13, 30, 2, 3)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (6, N'Jaula metal', 20, 30, 3, 3)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (7, N'Pienso P', 18, 50, 4, 1)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (8, N'Pienso G', 9, 50, 5, 2)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (9, N'Pienso P', 17, 40, 4, 3)
INSERT INTO [dbo].[ProductoProveedor] ([IdProductoProv], [Nombre], [Precio], [CantidadDisponible], [ProductoID], [ProveedorIdProveedor]) VALUES (10, N'Pienso G', 10, 40, 5, 3)
SET IDENTITY_INSERT [dbo].[ProductoProveedor] OFF