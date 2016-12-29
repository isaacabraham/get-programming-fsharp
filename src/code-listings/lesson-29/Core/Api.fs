/// Provides access to the banking API.
module Capstone5.Api

open Capstone5.Domain
open Capstone5.Operations
open System

/// Loads an account from disk. If no account exists, an empty one is automatically created.
let LoadAccount (customer:Customer) : RatedAccount =
    InCredit(CreditAccount { AccountId = Guid.NewGuid()
                             Balance = 0M
                             Owner = customer })

/// Deposits funds into an account.
let Deposit (amount:decimal) (customer:Customer) : RatedAccount =
    InCredit(CreditAccount { AccountId = Guid.NewGuid()
                             Balance = 0M
                             Owner = customer })

/// Withdraws funds from an account that is in credit.
let Withdraw (amount:decimal) (customer:Customer) : RatedAccount =
    InCredit(CreditAccount { AccountId = Guid.NewGuid()
                             Balance = 0M
                             Owner = customer })
                                 
/// Loads the transaction history for an owner. If no transactions exist, returns an empty sequence.
let LoadTransactionHistory(customer:Customer) : Transaction seq =
    Seq.empty

