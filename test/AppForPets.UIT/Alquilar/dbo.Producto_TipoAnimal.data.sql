SET IDENTITY_INSERT [dbo].[TipoAnimal] ON
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (1, N'Perro')
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (2, N'Gato')
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (3, N'Pez')
INSERT INTO [dbo].[TipoAnimal] ([TipoAnimalID], [NombreAnimal]) VALUES (4, N'Pájaro')
SET IDENTITY_INSERT [dbo].[TipoAnimal] OFF

SET IDENTITY_INSERT [dbo].[Producto] ON
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [PrecioAlquiler], [Precio], [TipoAnimalID], [CantidadAlquilar]) VALUES (3, N'Rascador', 20, 30, 1, 3)
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [PrecioAlquiler], [Precio], [TipoAnimalID], [CantidadAlquilar]) VALUES (4, N'Cama', 40, 50, 2, 3)
INSERT INTO [dbo].[Producto] ([ProductoID], [NombreProducto], [PrecioAlquiler], [Precio], [TipoAnimalID], [CantidadAlquilar]) VALUES (5, N'Pecera aquaman', 150, 200, 3, 1)
SET IDENTITY_INSERT [dbo].[Producto] OFF
