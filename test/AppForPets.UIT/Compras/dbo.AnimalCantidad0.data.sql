SET IDENTITY_INSERT [dbo].[Animal] ON
UPDATE [dbo].[Animal] SET Cantidad=0 FROM [dbo].[Animal]
SET IDENTITY_INSERT [dbo].[Animal] OFF
