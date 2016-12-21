module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit amount (CreditAccount account as creditAccount) =
    auditAs "withdraw" Auditing.composedLogger withdraw amount creditAccount account.AccountId account.Owner
let depositWithAudit amount unratedAccount =
    auditAs "deposit" Auditing.composedLogger deposit amount unratedAccount unratedAccount.UnratedAccountId unratedAccount.UnratedOwner
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
        |> defaultArg <|
            InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                     Balance = 0M
                                     Owner = { Name = owner } })
    
    printfn "Opening balance is £%M" openingAccount.UnratedBalance

    let processCommand account (command, amount) =
        printfn ""
        let account =
            match command with
            | Deposit -> account |> depositWithAudit amount
            | Withdraw ->
                match account with
                | InCredit account -> account |> withdrawWithAudit amount
                | Overdrawn _ ->
                    printfn "You cannot withdraw funds as your account is overdrawn!"
                    account
        printfn "Current balance is £%M" account.UnratedBalance
        match account with
        | InCredit _ -> ()
        | Overdrawn _ -> printfn "Your account is overdrawn!!"
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