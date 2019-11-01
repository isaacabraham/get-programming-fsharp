/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM dbo.Operation)
BEGIN
	INSERT INTO dbo.Operation VALUES (1, 'Withdraw')
	INSERT INTO dbo.Operation VALUES (2, 'Deposit')
END

IF NOT EXISTS (SELECT * FROM dbo.Account)
BEGIN
    INSERT INTO [dbo].[Account] ([Owner], [AccountId]) VALUES (N'isaac', N'c425e943-b3ca-40b5-8977-f3ddbcda80fc')
END

IF NOT EXISTS (SELECT * FROM dbo.AccountTransaction)
BEGIN
    SET IDENTITY_INSERT [dbo].[AccountTransaction] ON
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (1, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:51:47', 2, CAST(10 AS Decimal(18, 0)))
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (2, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:51:48', 2, CAST(10 AS Decimal(18, 0)))
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (3, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:51:48', 2, CAST(10 AS Decimal(18, 0)))
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (4, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:51:48', 2, CAST(10 AS Decimal(18, 0)))
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (5, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:51:51', 2, CAST(50 AS Decimal(18, 0)))
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (6, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:51:54', 1, CAST(100 AS Decimal(18, 0)))
    INSERT INTO [dbo].[AccountTransaction] ([AccountTransactionId], [AccountId], [Timestamp], [OperationId], [Amount]) VALUES (7, N'c425e943-b3ca-40b5-8977-f3ddbcda80fc', N'2017-05-11 22:52:00', 2, CAST(50 AS Decimal(18, 0)))
    SET IDENTITY_INSERT [dbo].[AccountTransaction] OFF
END