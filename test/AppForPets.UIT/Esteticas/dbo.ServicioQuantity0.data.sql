SET IDENTITY_INSERT [dbo].[Servicio] ON
UPDATE [dbo].[Servicio] SET Tiempo_Duracion=0 FROM [dbo].[Servicio]
SET IDENTITY_INSERT [dbo].[Servicio] OFF
