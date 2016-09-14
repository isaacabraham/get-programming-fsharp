module Capstone1.Auditing

open Capstone1.Domain
open System.IO

/// Logs to the console
let console account message = printfn "Account %O: %s" account.AccountId message

/// Logs to the file system
let fileSystem account message =
    Directory.CreateDirectory(sprintf @"C:\temp\learnfsharp\capstone1\%s" account.Owner.Name) |> ignore
    let filePath = sprintf @"C:\temp\learnfsharp\capstone1\%s\%O.txt" account.Owner.Name account.AccountId
    File.AppendAllLines(filePath, [ message ])