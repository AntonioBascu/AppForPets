SET IDENTITY_INSERT [dbo].[ProductoProveedor] ON
UPDATE [dbo].[ProductoProveedor] SET CantidadDisponible=0 FROM [dbo].[ProductoProveedor]
SET IDENTITY_INSERT [dbo].[ProductoProveedor] OFF
