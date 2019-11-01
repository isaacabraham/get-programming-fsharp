module internal Capstone6.SqlRepository

open Capstone6.Domain
open FSharp.Data
open System

[<AutoOpen>]
module private DB =
    let [<Literal>] Conn = "Name=AccountsDb"
    type AccountsDb = SqlProgrammabilityProvider<Conn>
    type GetAccountId = SqlCommandProvider<"SELECT TOP 1 AccountId FROM dbo.Account WHERE Owner = @owner", Conn, SingleRow = true>
    type FindTransactions = SqlCommandProvider<"SELECT Timestamp, OperationId, Amount FROM dbo.AccountTransaction WHERE AccountId = @accountId", Conn>
    type FindTransactionsByOwner = SqlCommandProvider<"SELECT a.AccountId, at.Timestamp, at.OperationId, at.Amount FROM dbo.Account a LEFT JOIN dbo.AccountTransaction at on a.AccountId = at.AccountId WHERE Owner = @owner", Conn>
    type DbOperations = SqlEnumProvider<"SELECT Description, OperationId FROM dbo.Operation", Conn>


let getAccountAndTransactions (owner:string) : (Guid * Transaction seq) option =
    None

let writeTransaction (accountId:Guid) (owner:string) (transaction:Transaction) =
    ()