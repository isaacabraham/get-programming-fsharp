module Capstone3.Auditing

open Capstone3.Domain
open Newtonsoft.Json
open System
open System.IO

/// Logs to the console
let console accountId transaction =
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
          fun accountId _ txn -> console accountId txn ]
    fun accountId owner transaction ->
        loggers
        |> List.iter(fun logger -> logger accountId owner transaction)

let loadAccount owner =
    let folder = findAccountFolder owner
    if String.IsNullOrEmpty folder then
        { AccountId = Guid.NewGuid(); Balance = 0M; Owner = { Name = owner } }
    else
        let owner, accountId =
            let parts = folder.Split '_'
            parts.[0], Guid.Parse parts.[1]

        buildPath(owner, accountId)
        |> Directory.EnumerateFiles
        |> Seq.map File.ReadAllText
        |> Seq.map(fun fileContents -> JsonConvert.DeserializeObject<Transaction> fileContents)
        |> Seq.filter(fun txn -> txn.Accepted)
        |> Seq.sortBy(fun txn -> txn.Timestamp)
        |> Seq.fold(fun account txn ->
            { account with
                Balance =
                    if txn.Operation = "withdraw" then account.Balance - txn.Amount
                    else account.Balance + txn.Amount })
            { AccountId = accountId; Balance = 0M; Owner = { Name = owner } }