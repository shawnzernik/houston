﻿CREATE TABLE [dbo].[Messages]
(
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [To] UNIQUEIDENTIFIER NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Message] VARCHAR(MAX) NOT NULL
)
