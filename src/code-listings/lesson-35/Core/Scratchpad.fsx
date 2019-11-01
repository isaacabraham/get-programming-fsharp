#load "Domain.fs"
#load "Operations.fs"
#r @"..\packages\FSharp.Data.SqlClient.1.8.2\lib\net40\FSharp.Data.SqlClient.dll"
#load "SqlRepository.fs"

open Capstone6.Operations
open Capstone6.Domain
open System
open FSharp.Data

// Copied from SqlRepository.fs, this allows you to test out the queries and commands in isolation.
let [<Literal>] Conn = @"Data Source=(localdb)\MSSQLLocalDB;Database=BankAccountDb;Integrated Security=True;Connect Timeout=60"
type AccountsDb = SqlProgrammabilityProvider<Conn>
type GetAccountId = SqlCommandProvider<"SELECT TOP 1 AccountId FROM dbo.Account WHERE Owner = @owner", Conn, SingleRow = true>
type FindTransactions = SqlCommandProvider<"SELECT Timestamp, OperationId, Amount FROM dbo.AccountTransaction WHERE AccountId = @accountId", Conn>
type FindTransactionsByOwner = SqlCommandProvider<"SELECT a.AccountId, at.Timestamp, at.OperationId, at.Amount FROM dbo.Account a LEFT JOIN dbo.AccountTransaction at on a.AccountId = at.AccountId WHERE Owner = @owner", Conn>
type DbOperations = SqlEnumProvider<"SELECT Description, OperationId FROM dbo.Operation", Conn>

// Get an accountId and then associated transactions. Note that I'm using .Value to "unwrap" the
// optional accountId. This is unsafe and should NEVER be done in "real" code; use either pattern
// matching or Option.map. I'm using it here as (a) this is a demo script, and (b) the database
// is primed with an account that I know about.
let accountId = GetAccountId.Create(Conn).Execute("isaac")
let transactions = FindTransactions.Create(Conn).Execute(accountId.Value) |> Seq.toArray