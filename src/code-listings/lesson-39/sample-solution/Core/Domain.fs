namespace Capstone7.Domain

open System
open System.ComponentModel.DataAnnotations

type BankOperation = Deposit | Withdraw

/// A customer of the bank.
type Customer = { Name : string }
/// An account held at the bank.
type Account = { AccountId : Guid; Owner : Customer; [<Required>] Balance : decimal }
/// A single transaction that has occurred.
type Transaction = { [<Required>] Timestamp : DateTime; Operation : BankOperation; [<Required>] Amount : decimal }

/// Represents a bank account that is known to be in credit.
type CreditAccount = CreditAccount of Account
/// A bank account which can either be in credit or overdrawn.
type RatedAccount =
    /// Represents an account that is known to be in credit.
    | InCredit of Account:CreditAccount
    /// Represents an account that is known to be overdrawn.
    | Overdrawn of Account:Account
    member internal this.GetField getter =
        match this with
        | InCredit (CreditAccount account) -> getter account
        | Overdrawn account -> getter account
    /// Gets the current balance of the account.
    member this.Balance = this.GetField(fun a -> a.Balance)