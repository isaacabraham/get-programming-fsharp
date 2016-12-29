/// Provides access to the banking API.
module Capstone5.Api

open Capstone5.Domain
open Capstone5.Operations
open System

/// Deposits funds into an account.
let Deposit amount (ratedAccount:RatedAccount) =
    let accountId = ratedAccount.GetField (fun a -> a.AccountId)
    let owner = ratedAccount.GetField(fun a -> a.Owner)
    auditAs "deposit" Auditing.composedLogger deposit amount ratedAccount accountId owner

/// Withdraws funds from an account that is in credit.
let Withdraw amount creditAccount =
    let (CreditAccount account) = creditAccount
    auditAs "withdraw" Auditing.composedLogger withdraw amount creditAccount account.AccountId account.Owner

/// Loads the transaction history for an owner.
let LoadTransactionHistory(owner) =
    owner
    |> FileRepository.tryFindTransactionsOnDisk
    |> Option.map(fun (_,_,txns) -> txns)
    |> defaultArg <| Seq.empty

/// Loads an account from disk. If no account exists, an empty one is automatically created.
let LoadAccount(owner) =
    owner
    |> FileRepository.tryFindTransactionsOnDisk
    |> Option.map Operations.loadAccount
    |> defaultArg <|
        InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                 Balance = 0M
                                 Owner = { Name = owner } })