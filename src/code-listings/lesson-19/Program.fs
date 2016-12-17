module Capstone3.Program

open System
open Capstone3.Domain
open Capstone3.Operations

[<EntryPoint>]
let main _ =
    let account =
        let loadAccountFromDisk = FileRepository.findTransactionsOnDisk >> Operations.loadAccount
        let name =
            Console.Write "Please enter your name: "
            Console.ReadLine()
        loadAccountFromDisk name
    printfn "Current balance is £%M" account.Balance

    let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
    let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadLine() }

    let validCommands = Set [ "d"; "w"; "x" ]
    let isValidCommand = validCommands.Contains
    let isStopCommand = (=) "x"

    let processCommand account command =
        Console.WriteLine()
        Console.Write "Enter Amount: "
        let amount = Console.ReadLine() |> Decimal.Parse

        Console.Clear()

        let account =
            if command = "d" then account |> depositWithAudit amount
            elif command = "w" then account |> withdrawWithAudit amount
            else account

        printfn "Current balance is £%M" account.Balance
        account

    let account =
        commands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.fold processCommand account
    
    Console.Clear()
    printfn "Closing Balance:\r\n %A" account
    Console.ReadKey() |> ignore

    0