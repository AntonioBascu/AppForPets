SET IDENTITY_INSERT [dbo].[Producto] ON
UPDATE [dbo].[Producto] SET CantidadAlquilar=0 FROM [dbo].[Producto]
SET IDENTITY_INSERT [dbo].[Producto] OFF
