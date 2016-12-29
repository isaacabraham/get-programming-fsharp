module Capstone5.FileRepository

open Capstone5.Domain
open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path
let private tryFindAccountFolder owner =    
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner) |> Seq.toList
    match folders with
    | [] -> None
    | folder :: _ -> Some(DirectoryInfo(folder).Name)

let private buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

let loadTransactions (folder:string) =
    let owner, accountId =
        let parts = folder.Split '_'
        parts.[0], Guid.Parse parts.[1]
    owner, accountId, buildPath(owner, accountId)
                      |> Directory.EnumerateFiles
                      |> Seq.map (File.ReadAllText >> Transactions.deserialize)

/// Finds all transactions from disk for specific owner.
let tryFindTransactionsOnDisk = tryFindAccountFolder >> Option.map loadTransactions

/// Logs to the file system
let writeTransaction accountId owner transaction =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (transaction.Timestamp.ToFileTimeUtc())
    let line = Transactions.serialize transaction
    File.WriteAllText(filePath, line)