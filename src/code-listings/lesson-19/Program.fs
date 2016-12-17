module Capstone3.Program

open System
open Capstone3.Domain
open Capstone3.Operations

[<EntryPoint>]
let main _ =
    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
    let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

    let openingAccount = { Owner = { Name = name }; Balance = 0M; AccountId = Guid.Empty } 

    // Fill in the main loop here...
    let closingAccount =
        openingAccount

    Console.Clear()
    printfn "Closing Balance:\r\n %A" closingAccount.Balance
    Console.ReadKey() |> ignore

    0