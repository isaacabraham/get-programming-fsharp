#load "Domain.fs"
#load "Operations.fs"

open Capstone4.Operations
open Capstone4.Domain
open System

// Listing 24.1
// commands
// |> Seq.filter isValidCommand
// |> Seq.takeWhile (not << isStopCommand)
// |> Seq.map getAmount
// |> Seq.fold processCommand openingAccount


// Listing 24.2
// let processCommand account (command, amount) =
//     if command = 'd' then account |> deposit amount
//     else account |> withdraw amount

// Listing 24.3
// type BankOperation = Deposit | Withdraw
// type Command = BankCommand of BankOperation | Exit
// let tryGetBankOperation cmd =
//     match cmd with
//     | BankCommand op -> Some op
//     | Exit -> None

// Listing 24.4
// let tryGetAmount command =
//     Console.WriteLine()
//     Console.Write "Enter Amount: "
//     let amount = Console.ReadLine() |> Decimal.TryParse
//     match amount with
//     | true, amount -> Some(command, amount)
//     | false, _ -> None

// Listing 24.5
// let private findAccountFolder owner =    
// 	// code elided…
//     if Seq.isEmpty folders then ""
//     else
//         let folder = Seq.head folders
//         DirectoryInfo(folder).Name

// let findTransactionsOnDisk owner =
//     let folder = findAccountFolder owner
//     if String.IsNullOrEmpty folder then …
//        else loadTransactions folder

// Listing 24.6
// let loadAccountOptional value =
//     match value with
//     | Some value -> Some(Operations.loadAccount value)
//     | None -> None
// FileRepository.tryFindTransactionsOnDisk >> loadAccountOptional

// Listing 24.7
// let loadAccountOptional = Option.map Operations.loadAccount
// FileRepository.tryFindTransactionsOnDisk >> loadAccountOptional

// Listing 24.8
// let openingAccount =
//     Console.Write "Please enter your name: "
//     let owner = Console.ReadLine()
        
//     match (tryLoadAccountFromDisk owner) with
//     | Some account -> account
//     | None ->
//         { Balance = 0M
//           AccountId = Guid.NewGuid()
//           Owner = { Name = owner } }

// Listing 24.9
// type CreditAccount = CreditAccount of Account
// type RatedAccount =
//     | Credit of CreditAccount
//     | Overdrawn of Account

// Listing 24.10
// let rateAccount account =
//     if account.Balance < 0M then Overdrawn account
//     else Credit(CreditAccount account)

// let withdraw amount (CreditAccount account) =
//     { account with Balance = account.Balance - amount }
//     |> rateAccount

// let deposit amount account =
//     let account =
//         match account with
//         | Credit (CreditAccount account) -> account
//         | Overdrawn account -> account
//     { account with Balance = account.Balance + amount }
//     |> rateAccount

// Listing 24.11
// let withdrawSafe amount ratedAccount =
//     match ratedAccount with
//     | Credit account -> account |> withdraw amount
//     | Overdrawn _ ->
//         printfn "Your account is overdrawn - withdrawal rejected!"
//         ratedAccount // return input back out
