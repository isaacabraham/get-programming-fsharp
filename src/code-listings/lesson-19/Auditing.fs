module Capstone3.Auditing

open Capstone3.Operations
open Capstone3.Domain
open Newtonsoft.Json
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

/// Logs to the file system
let fileSystem accountId owner transaction =
    let path = buildPath(owner, accountId)
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.json" path (transaction.Timestamp.ToFileTimeUtc())
    File.WriteAllText(filePath, JsonConvert.SerializeObject transaction)

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
        let fileToTransaction filename =
            let fileContents = filename |> File.ReadAllText
            JsonConvert.DeserializeObject<Transaction> fileContents
        let owner, accountId =
            let parts = folder.Split '_'
            parts.[0], Guid.Parse parts.[1]
        owner, accountId, buildPath(owner, accountId)
                          |> Directory.EnumerateFiles
                          |> Seq.map fileToTransaction
let loadAccount (owner, accountId, transactions) =
    let openingAccount = { AccountId = accountId; Balance = 0M; Owner = { Name = owner } }

    transactions
    |> Seq.sortBy(fun txn -> txn.Timestamp)
    |> Seq.fold(fun account txn ->
        if txn.Operation = "withdraw" then account |> withdraw txn.Amount
        else account |> deposit txn.Amount) openingAccount