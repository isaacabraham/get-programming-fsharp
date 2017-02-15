namespace Capstone8.Controllers

open System
open System.Web.Http
open System.Net.Http
open System.Net
open System.Web.Http.Description
open Capstone8.Domain
open Capstone8.Api
open System.Web.Http.Dependencies
open System.ComponentModel.DataAnnotations

/// A request to perform a transaction.
type TransactionRequest =
    { [<Required>]
      Amount : decimal }

[<RoutePrefix("api")>]
type BankAccountController(bankApi:IBankApi) =
    inherit ApiController()

    // Some helper functions and methods.
    // let-bound functions are NOT the same as private members. The latter are C#-style private methods on a class.
    let asRawAccount account =
        match account with
        | InCredit (CreditAccount a) -> a
        | Overdrawn a -> a
    member private this.AsOK account = this.Ok(account) :> IHttpActionResult
    member private this.AsBadRequest (message:string) = this.BadRequest message :> IHttpActionResult

    [<Route("accounts/{name}")>]
    [<ResponseType(typeof<Account>)>]
    /// Gets details of an account.
    member __.GetAccount(name) = async {
        let! account = bankApi.LoadAccount { Name = name }
        return account |> asRawAccount } |> Async.StartAsTask

    [<Route("transactions/{name}")>]
    [<ResponseType(typeof<Transaction seq>)>]
    /// Gets the transaction history of an account.
    member __.GetHistory(name) = bankApi.LoadTransactionHistory { Name = name } |> Async.StartAsTask

    [<Route("transactions/deposit/{name}")>]
    [<ResponseType(typeof<RatedAccount>)>]
    /// Posts a deposit transaction to an account.
    member this.PostDeposit(name, request : TransactionRequest) =
        let customer = { Name = name }
        async {
            let! account = bankApi.Deposit request.Amount customer
            return this.AsOK(account |> asRawAccount) } |> Async.StartAsTask

    [<Route("transactions/withdraw/{name}")>]
    [<ResponseType(typeof<RatedAccount>)>]
    /// Posts a withdrawal transaction to an account.
    member this.PostWithdrawal(name, request : TransactionRequest) =
        let customer = { Name = name }
        async {
            let! account = bankApi.Withdraw request.Amount customer
            match account with
            | Success account -> return this.AsOK(account |> asRawAccount)
            | Failure message -> return this.AsBadRequest message } |> Async.StartAsTask

/// Handles dependency resolution for controllers.
type DependencyResolver() =
    let bankApi = CreateSqlApi Configuration.ConfigurationManager.ConnectionStrings.["AccountsDb"].ConnectionString
    let resolve (serviceType:System.Type) =
        match serviceType with
        | t when t = typeof<BankAccountController> -> new BankAccountController(bankApi) :> obj
        | _ -> null
        
    interface IDependencyResolver with
        member this.BeginScope() = this :> IDependencyScope            
        member __.Dispose() = ()
        member __.GetService(serviceType) = resolve serviceType
        member __.GetServices(serviceType) = resolve serviceType |> Seq.singleton
        
