SET IDENTITY_INSERT [dbo].[Tipo] ON
INSERT INTO [dbo].[Tipo] ([TipoID], [Raza], [cantidadTipo]) VALUES (1, N'golden', 4)
INSERT INTO [dbo].[Tipo] ([TipoID], [Raza], [cantidadTipo]) VALUES (2, N'borde collie', 5)
INSERT INTO [dbo].[Tipo] ([TipoID], [Raza], [cantidadTipo]) VALUES (3, N'pastor aleman', 2)
SET IDENTITY_INSERT [dbo].[Tipo] OFF

SET IDENTITY_INSERT [dbo].[Animal] ON
INSERT INTO [dbo].[Animal] ([AnimalID], [Precio], [Cantidad], [Edad], [TipoID]) VALUES (1, 10, 4, 1, 1)
INSERT INTO [dbo].[Animal] ([AnimalID], [Precio], [Cantidad], [Edad], [TipoID]) VALUES (2, 20, 5, 1, 2)
INSERT INTO [dbo].[Animal] ([AnimalID], [Precio], [Cantidad], [Edad], [TipoID]) VALUES (3, 30, 2, 1, 3)
SET IDENTITY_INSERT [dbo].[Animal] OFF
