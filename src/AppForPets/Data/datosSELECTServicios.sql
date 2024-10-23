
SET IDENTITY_INSERT [dbo].[Tipo_Servicio] ON
INSERT INTO [dbo].[Tipo_Servicio] ([ID], [Nombre]) VALUES (1, N'Peluqueria')
INSERT INTO [dbo].[Tipo_Servicio] ([ID], [Nombre]) VALUES (2, N'Tratamientos')
INSERT INTO [dbo].[Tipo_Servicio] ([ID], [Nombre]) VALUES (3, N'Lavados')
SET IDENTITY_INSERT [dbo].[Tipo_Servicio] OFF

SET IDENTITY_INSERT [dbo].[Servicio] ON
INSERT INTO [dbo].[Servicio] ([ServicioID], [Nombre_Servicio], [Precio_Servicio], [Tiempo_Duracion], [Cantidad_servicio], [Tipo_ServicioID]) VALUES (1, N'Peluqueria Canina', 10, 1, 4, 1)
INSERT INTO [dbo].[Servicio] ([ServicioID], [Nombre_Servicio], [Precio_Servicio], [Tiempo_Duracion], [Cantidad_servicio], [Tipo_ServicioID]) VALUES (2, N'Tratamiento Antiparasitos', 25, 2, 5, 2)
INSERT INTO [dbo].[Servicio] ([ServicioID], [Nombre_Servicio], [Precio_Servicio], [Tiempo_Duracion], [Cantidad_servicio], [Tipo_ServicioID]) VALUES (10, N'Lavado Gatuno', 15, 1, 2, 3)
SET IDENTITY_INSERT [dbo].[Servicio] OFF

SET IDENTITY_INSERT [dbo].[Tipo_Servicio] ON
INSERT INTO [dbo].[Tipo_Servicio] ([ID], [Nombre]) VALUES (1, N'Peluqueria')
INSERT INTO [dbo].[Tipo_Servicio] ([ID], [Nombre]) VALUES (2, N'Tratamientos')
INSERT INTO [dbo].[Tipo_Servicio] ([ID], [Nombre]) VALUES (3, N'Lavados')
SET IDENTITY_INSERT [dbo].[Tipo_Servicio] OFF
