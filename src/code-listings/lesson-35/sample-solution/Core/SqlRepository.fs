module internal Capstone6.SqlRepository

open Capstone6.Domain
open FSharp.Data
open System.Data.SqlClient

[<AutoOpen>]
module private DB =
    let [<Literal>] Conn = @"Data Source=(localdb)\MSSQLLocalDB;Database=BankAccountDb;Integrated Security=True;Connect Timeout=60"
    type AccountsDb = SqlProgrammabilityProvider<Conn>
    type GetAccountId = SqlCommandProvider<"SELECT TOP 1 AccountId FROM dbo.Account WHERE Owner = @owner", Conn, SingleRow = true>
    type FindTransactions = SqlCommandProvider<"SELECT Timestamp, OperationId, Amount FROM dbo.AccountTransaction WHERE AccountId = @accountId", Conn>
    type FindTransactionsByOwner = SqlCommandProvider<"SELECT a.AccountId, at.Timestamp, at.OperationId, at.Amount FROM dbo.Account a LEFT JOIN dbo.AccountTransaction at on a.AccountId = at.AccountId WHERE Owner = @owner", Conn>
    type DbOperations = SqlEnumProvider<"SELECT Description, OperationId FROM dbo.Operation", Conn>

type private DbTables = DB.AccountsDb.dbo.Tables

let toBankOperation operationId =
    match operationId with
    | DbOperations.Deposit -> Deposit
    | DbOperations.Withdraw -> Withdraw
    | _ -> failwith "Unknown DB Operation case!"

/// Single query version of Get Account and Transactions
let getAccountAndTransactions (connection:string) owner =
    let results = DB.FindTransactionsByOwner.Create(connection).Execute(owner) |> Seq.toList
    match results with
    | [] -> None
    | [ row ] when row.Amount.IsNone -> Some(row.AccountId, Seq.empty)
    | (firstRow :: _) as results->
        let accountId = firstRow.AccountId
        let transactions =
            results
            |> List.choose(fun r ->
                match r.Amount, r.Timestamp with
                | Some amount, Some timestamp -> Some { Transaction.Amount = amount; Timestamp = timestamp; Operation = Withdraw }
                | _ -> None)
            |> List.toSeq
        Some (accountId, transactions)

/// Multiple query version of Get Account and Transactions
let getAccountAndTransactionsV2 (connection:string) owner =
    let accountId = DB.GetAccountId.Create(connection).Execute(owner)
    
    accountId
    |> Option.map(fun accountId ->
        let transactions =
            DB.FindTransactions.Create(connection).Execute(accountId)
            |> Seq.map(fun row ->
                { Amount = row.Amount
                  Timestamp = row.Timestamp
                  Operation = row.OperationId |> toBankOperation })
        accountId, transactions)

/// An active pattern that tests whether an exception is a SQL PK constraint.
let (|PrimaryKeyConstraint|_|) (ex:exn) =
    match ex with
    | :? SqlException as ex when ex.Message.Contains "Violation of PRIMARY KEY constraint" -> Some PrimaryKeyConstraint
    | _ -> None   

let writeTransaction (connection:string) accountId owner transaction =
    use accounts = new DbTables.Account()
    accounts.AddRow(owner, accountId)
    use connection = new SqlConnection(connection)
    connection.Open()
    
    try accounts.Update(connection) |> ignore
    with
    | PrimaryKeyConstraint -> ()
    | _ -> reraise()
    
    use transactions = new DbTables.AccountTransaction()
    let bankOperationId =
        match transaction.Operation with 
        | Deposit -> DB.DbOperations.Deposit
        | Withdraw -> DB.DbOperations.Withdraw

    transactions.AddRow(accountId, transaction.Timestamp, bankOperationId, transaction.Amount)
    transactions.Update(connection) |> ignore