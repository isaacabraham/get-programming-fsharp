﻿/// Provides access to the banking API.
module Capstone6.Api

open Capstone6.Domain
open Capstone6.Operations
open System

/// Loads an account from disk. If no account exists, an empty one is automatically created.
let LoadAccount customer =
    customer.Name
    |> FileRepository.tryFindTransactionsOnDisk
    |> Option.map (Operations.buildAccount customer.Name)
    |> defaultArg <|
        InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                 Balance = 0M
                                 Owner = customer })
/// Deposits funds into an account.
let Deposit amount customer =
    let ratedAccount = LoadAccount customer
    let accountId = ratedAccount.GetField (fun a -> a.AccountId)
    let owner = ratedAccount.GetField(fun a -> a.Owner)
    auditAs Deposit FileRepository.writeTransaction deposit amount ratedAccount accountId owner

/// Withdraws funds from an account that is in credit.
let Withdraw amount customer =
    let account = LoadAccount customer
    match account with
    | InCredit (CreditAccount account as creditAccount) -> auditAs Withdraw FileRepository.writeTransaction withdraw amount creditAccount account.AccountId account.Owner
    | account -> account

/// Loads the transaction history for an owner.
let LoadTransactionHistory customer =
    customer.Name
    |> FileRepository.tryFindTransactionsOnDisk
    |> Option.map(fun (_,txns) -> txns)
    |> defaultArg <| Seq.empty