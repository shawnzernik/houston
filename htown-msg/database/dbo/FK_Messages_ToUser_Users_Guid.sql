ALTER TABLE [dbo].[Messages]
	ADD CONSTRAINT [FK_Messages_ToUser_Users_Guid]
	FOREIGN KEY ([ToUser])
	REFERENCES [Users] ([Guid])
