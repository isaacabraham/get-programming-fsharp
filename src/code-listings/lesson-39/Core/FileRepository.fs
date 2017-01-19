module internal Capstone7.FileRepository

open Capstone7.Domain
open System.IO
open System
open Newtonsoft.Json

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

let private loadTransactions (folder:string) =
    let owner, accountId =
        let parts = folder.Split '_'
        parts.[0], Guid.Parse parts.[1]
    accountId, buildPath(owner, accountId)
               |> Directory.EnumerateFiles
               |> Seq.map (fun path -> JsonConvert.DeserializeObject<Transaction>(File.ReadAllText path))

/// Finds all transactions from disk for specific owner.
let tryFindTransactionsOnDisk = tryFindAccountFolder >> Option.map loadTransactions

/// Logs to the file system
let writeTransaction accountId owner transaction =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.json" path (transaction.Timestamp.ToFileTimeUtc())
    let line = transaction |> JsonConvert.SerializeObject
    File.WriteAllText(filePath, line)