module Capstone3.Auditing

open Capstone3.Operations
open Capstone3.Domain
open System
open System.IO

/// Logs to the console
let console _ accountId transaction =
    printfn "Account %O: %s of %M (approved: %b)" accountId transaction.Operation transaction.Amount transaction.Accepted
let accountsPath = @"C:\temp\learnfsharp\lesson19"
let buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId
let findAccountFolder owner =    
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

module Serialization =
    /// Serializes a transaction
    let serializeTransaction transaction =
        sprintf "%O***%s***%M***%b" transaction.Timestamp transaction.Operation transaction.Amount transaction.Accepted
    let deserializeTransaction (fileContents:string) =
        let parts = fileContents.Split([|"***"|], StringSplitOptions.None)
        { Timestamp = DateTime.Parse parts.[0]
          Operation = parts.[1]
          Amount = Decimal.Parse parts.[2]
          Accepted = Boolean.Parse parts.[3] }
/// Logs to the file system
let fileSystem accountId owner transaction =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (transaction.Timestamp.ToFileTimeUtc())
    let line = sprintf "%O***%s***%M***%b" transaction.Timestamp transaction.Operation transaction.Amount transaction.Accepted
    File.WriteAllText(filePath, line)

// Logs to both console and file system
let composedLogger = 
    let loggers =
        [ fileSystem
          console ]
    fun accountId owner transaction ->
        loggers
        |> List.iter(fun logger -> logger accountId owner transaction)
let findTransactionsOnDisk owner =
    let folder = findAccountFolder owner
    if String.IsNullOrEmpty folder then owner, Guid.Empty, Seq.empty
    else
        let owner, accountId =
            let parts = folder.Split '_'
            parts.[0], Guid.Parse parts.[1]
        owner, accountId, buildPath(owner, accountId)
                          |> Directory.EnumerateFiles
                          |> Seq.map (File.ReadAllText >> Serialization.deserializeTransaction)
let loadAccount (owner, accountId, transactions) =
    let openingAccount = { AccountId = accountId; Balance = 0M; Owner = { Name = owner } }

    transactions
    |> Seq.sortBy(fun txn -> txn.Timestamp)
    |> Seq.fold(fun account txn ->
        if txn.Operation = "withdraw" then account |> withdraw txn.Amount
        else account |> deposit txn.Amount) openingAccount