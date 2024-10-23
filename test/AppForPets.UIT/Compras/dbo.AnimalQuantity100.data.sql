SET IDENTITY_INSERT [dbo].[Animal] ON
UPDATE [dbo].[Animal] SET Cantidad=100 FROM [dbo].[Animal]
SET IDENTITY_INSERT [dbo].[Animal] OFF
