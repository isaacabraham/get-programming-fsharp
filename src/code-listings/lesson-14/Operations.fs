module Capstone2.Operations

open System
open Capstone2.Domain

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

/// Deposits an amount into an account
let deposit amount account =
    { account with Balance = account.Balance + amount }

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount account =
    audit account (sprintf "%O: Performing a %s operation for £%M..." DateTime.UtcNow operationName amount)
    let updatedAccount = operation amount account
    
    let accountIsUnchanged = (updatedAccount = account)

    if accountIsUnchanged then audit account (sprintf "%O: Transaction rejected!" DateTime.UtcNow) 
    else audit account (sprintf "%O: Transaction accepted! Balance is now £%M." DateTime.UtcNow updatedAccount.Balance)

    updatedAccount