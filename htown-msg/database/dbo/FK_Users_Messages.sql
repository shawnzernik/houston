ALTER TABLE [dbo].[Messages]
	ADD CONSTRAINT [FK_Users_Messages]
	FOREIGN KEY ([To User])
	REFERENCES [Users] ([Guid])
