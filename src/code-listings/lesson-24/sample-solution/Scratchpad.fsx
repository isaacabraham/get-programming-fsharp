#load "Domain.fs"
#load "Operations.fs"

open Capstone4.Operations
open Capstone4.Domain
open System

type CreditAccount = CreditAccount of Account

type RatedAccount =
    | Credit of CreditAccount
    | Overdrawn of Account

let rateAccount account =
    if account.Balance < 0M then Overdrawn account
    else Credit(CreditAccount account)

let withdraw amount (CreditAccount account) =
    { account with Balance = account.Balance - amount }
    |> rateAccount

let deposit amount account =
    let account =
        match account with
        | Credit (CreditAccount account) -> account
        | Overdrawn account -> account
    { account with Balance = account.Balance + amount }
    |> rateAccount

let myAccount = { Balance = 0M; Owner = { Name = "Isaac" }; AccountId = Guid.NewGuid() } |> rateAccount

myAccount
|> deposit 50M
|> deposit 100M
//|> withdraw 500M <- does not compile :)

let withdrawSafe amount ratedAccount =
    match ratedAccount with
    | Credit account -> account |> withdraw amount
    | Overdrawn _ ->
        printfn "Your account is overdrawn - withdrawal rejected!"
        ratedAccount

myAccount
|> deposit 50M
|> deposit 100M
|> withdrawSafe 500M
|> withdrawSafe 500M
