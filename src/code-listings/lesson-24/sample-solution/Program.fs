module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit
let tryLoadAccountFromDisk = FileRepository.tryFindTransactionsOnDisk >> Option.map Operations.loadAccount

type Command = | AccountCmd of BankOperation | Exit

[<AutoOpen>]
module CommandParsing =
    let tryParse cmd =
        match cmd with
        | 'd' -> Some (AccountCmd Deposit)
        | 'w' -> Some (AccountCmd Withdraw)
        | 'x' -> Some Exit
        | _ -> None

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }
    
    let getAmount command =
        let seqOfAmounts =
            Seq.initInfinite(fun _ ->
                Console.WriteLine()
                Console.Write "Enter Amount: "
                Console.ReadLine() |> Decimal.TryParse)
        
        let validAmount =
            seqOfAmounts
            |> Seq.choose(fun amount ->
                match amount with
                | true, amount -> Some amount
                | false, _ -> None)
            |> Seq.head

        command, validAmount

[<EntryPoint>]
let main _ =
    let openingAccount =
        Console.Write "Please enter your name: "
        let owner = Console.ReadLine()
        
        owner 
        |> tryLoadAccountFromDisk        
        |> defaultArg <| { AccountId = Guid.NewGuid()
                           Balance = 0M
                           Owner = { Name = owner } }
    
    printfn "Current balance is £%M" openingAccount.Balance

    let processCommand account (command, amount) =
        printfn ""
        let account =
            match command with
            | Deposit -> account |> depositWithAudit amount
            | Withdraw -> account |> withdrawWithAudit amount
        printfn "Current balance is £%M" account.Balance
        account

    let closingAccount =
        commands
        |> Seq.choose tryParse
        |> Seq.takeWhile ((<>) Exit)
        |> Seq.choose(fun cmd ->
            match cmd with
            | Exit -> None
            | AccountCmd cmd -> Some cmd)
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0