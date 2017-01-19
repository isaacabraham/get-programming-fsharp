/// Provides access to the banking API.
module Capstone7.Api

open Capstone7.Domain
open Capstone7.Operations
open System

type Result<'T> = Success of 'T | Failure of string

/// Represents the gateway to perform bank operations.
type IBankApi =
    /// Loads an account from disk. If no account exists, an empty one is automatically created.
    abstract member LoadAccount : customer:Customer -> RatedAccount
    /// Deposits funds into an account.
    abstract member Deposit : amount:Decimal -> customer:Customer -> RatedAccount
    /// Withdraws funds from an account that is in credit.
    abstract member Withdraw : amount:Decimal -> customer:Customer -> RatedAccount
    /// Loads the transaction history for an owner.
    abstract member LoadTransactionHistory : customer:Customer -> Transaction seq

let private buildApi loadAccountHistory saveTransaction =
    { new IBankApi with
        member __.LoadAccount(customer) = 
            let accountHistory = customer.Name |> loadAccountHistory
            accountHistory
            |> Option.map (Operations.buildAccount customer.Name)
            |> defaultArg <|
                InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                         Balance = 0M
                                         Owner = customer })
          member this.Deposit amount customer =
            let (ratedAccount:RatedAccount) = this.LoadAccount customer
            let accountId = ratedAccount.GetField (fun a -> a.AccountId)
            let owner = ratedAccount.GetField(fun a -> a.Owner)
            auditAs Deposit saveTransaction deposit amount ratedAccount accountId owner
          member __.LoadTransactionHistory(customer) =
            let accountHistory = customer.Name |> loadAccountHistory
            accountHistory
            |> Option.map snd
            |> defaultArg <| Seq.empty
          member this.Withdraw amount customer =
            let account = this.LoadAccount customer
            match account with
            | InCredit (CreditAccount account as creditAccount) -> auditAs Withdraw saveTransaction withdraw amount creditAccount account.AccountId account.Owner
            | _ -> account }

/// Creates a SQL-enabled API.
let CreateSqlApi connectionString =
    buildApi
        (SqlRepository.getAccountAndTransactions connectionString)
        (SqlRepository.writeTransaction connectionString)

/// Gets a handle to the file-based API
let FileApi =
    buildApi
        (FileRepository.tryFindTransactionsOnDisk)
        FileRepository.writeTransaction