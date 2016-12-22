namespace Capstone4.Domain

open System

type BankOperation = Deposit | Withdraw
type Customer = { Name : string }
type Account = { AccountId : Guid; Owner : Customer; Balance : decimal }
type Transaction = { Timestamp : DateTime; Operation : string; Amount : decimal }

/// Represents a bank account that is known to be in credit.
type CreditAccount = CreditAccount of Account
/// A bank account which can either be in credit or overdrawn.
type UnratedAccount =
    | InCredit of CreditAccount
    | Overdrawn of Account
    member private this.Account = 
        match this with
        | InCredit (CreditAccount account)
        | Overdrawn account -> account
    member this.UnratedBalance = this.Account.Balance
    member this.UnratedAccountId = this.Account.AccountId
    member this.UnratedOwner = this.Account.Owner

module Transactions =
    /// Serializes a transaction
    let serialize transaction =
        sprintf "%O***%s***%M" transaction.Timestamp transaction.Operation transaction.Amount
    
    /// Deserializes a transaction
    let deserialize (fileContents:string) =
        let parts = fileContents.Split([|"***"|], StringSplitOptions.None)
        { Timestamp = DateTime.Parse parts.[0]
          Operation = parts.[1]
          Amount = Decimal.Parse parts.[2] }