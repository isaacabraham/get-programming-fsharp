/// Provides access to the banking API.
module Capstone8.Api

open Capstone8.Domain
open Capstone8.Operations
open System

type Result<'T> = Success of 'T | Failure of string

/// Represents the gateway to perform bank operations.
type IBankApi =
    /// Loads an account from disk. If no account exists, an empty one is automatically created.
    abstract member LoadAccount : customer:Customer -> Async<RatedAccount>
    /// Deposits funds into an account.
    abstract member Deposit : amount:Decimal -> customer:Customer -> Async<RatedAccount>
    /// Withdraws funds from an account that is in credit.
    abstract member Withdraw : amount:Decimal -> customer:Customer -> Async<Result<RatedAccount>>
    /// Loads the transaction history for an owner.
    abstract member LoadTransactionHistory : customer:Customer -> Async<Transaction seq>

let buildApi loadAccountHistory saveTransaction =
    { new IBankApi with
        member __.LoadAccount(customer) = async {
            let! accountHistory = customer.Name |> loadAccountHistory
            return
                accountHistory
                |> Option.map (Operations.buildAccount customer.Name)
                |> defaultArg <|
                    InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                             Balance = 0M
                                             Owner = customer }) }
          member this.Deposit amount customer = async {
            let! (ratedAccount:RatedAccount) = this.LoadAccount customer
            let accountId = ratedAccount.GetField (fun a -> a.AccountId)
            let owner = ratedAccount.GetField(fun a -> a.Owner)
            return auditAs Deposit saveTransaction deposit amount ratedAccount accountId owner }
          member __.LoadTransactionHistory(customer) = async {
            let! accountHistory = customer.Name |> loadAccountHistory
            return
                accountHistory
                |> Option.map snd
                |> defaultArg <| Seq.empty }
          member this.Withdraw amount customer = async {
            let! account = this.LoadAccount customer
            match account with
            | InCredit (CreditAccount account as creditAccount) ->
                return
                    auditAs Withdraw saveTransaction withdraw amount creditAccount account.AccountId account.Owner
                    |> Success
            | _ -> return Failure "Account is overdrawn!" } }

/// Creates a SQL-enabled API.
let CreateSqlApi connectionString =
    buildApi
        (SqlRepository.getAccountAndTransactionsV2 connectionString)
        (SqlRepository.writeTransaction connectionString)

/// Gets a handle to the file-based API
let FileApi =
    buildApi
        (FileRepository.tryFindTransactionsOnDisk >> async.Return)
        FileRepository.writeTransaction