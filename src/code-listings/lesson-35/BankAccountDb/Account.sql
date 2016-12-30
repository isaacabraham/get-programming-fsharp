CREATE TABLE [dbo].[Account]
(
	[Owner] VARCHAR(256) NOT NULL PRIMARY KEY,
	[AccountId] UNIQUEIDENTIFIER NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_Account_AccountId] ON [dbo].[Account] ([AccountId])
