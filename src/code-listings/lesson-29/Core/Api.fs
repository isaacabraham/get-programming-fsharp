/// Provides access to the banking API.
module Capstone5.Api

open Capstone5.Domain
open Capstone5.Operations
open System

/// Deposits funds into an account.
let Deposit (amount:decimal) (ratedAccount:RatedAccount) : RatedAccount =
    ratedAccount

/// Withdraws funds from an account that is in credit.
let Withdraw (amount:decimal) (creditAccount:CreditAccount) : RatedAccount =
    InCredit creditAccount
    
/// Loads the transaction history for an owner. If no transactions exist, returns an empty sequence.
let LoadTransactionHistory(owner:string) : Transaction seq =
    Seq.empty

/// Loads an account from disk. If no account exists, an empty one is automatically created.
let LoadAccount(owner:string) : RatedAccount =
    InCredit(CreditAccount { AccountId = Guid.NewGuid()
                             Balance = 0M
                             Owner = { Name = owner } })