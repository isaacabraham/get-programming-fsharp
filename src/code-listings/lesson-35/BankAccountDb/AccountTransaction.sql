CREATE TABLE [dbo].[AccountTransaction]
(
	[AccountTransactionId] INT NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY(1,1),
	[AccountId] UNIQUEIDENTIFIER NOT NULL,
	[Timestamp] DATETIME NOT NULL,
	[OperationId] int NOT NULL,
	[Amount] decimal NOT NULL, 
    CONSTRAINT [FK_AccountTransaction_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account]([AccountId]), 
    CONSTRAINT [FK_AccountTransaction_Operation] FOREIGN KEY ([OperationId]) REFERENCES [dbo].[Operation]([OperationId])
)

GO

CREATE CLUSTERED INDEX [IX_Transaction_AccountId] ON [dbo].[AccountTransaction] ([AccountId])
