module ``API Tests``

open System
open Capstone8.Domain
open Capstone8.Api
open Xunit
open Swensen.Unquote

let createInMemApi() =
    let dataStore = ResizeArray()
    let save accountId owner transaction = dataStore.Add(accountId, owner, transaction)
    let load (owner:string) =
        dataStore
        |> Seq.filter(fun (_:Guid, txnOwner, _:Transaction) -> txnOwner = owner)
        |> Seq.map(fun (accountId, _, txn) -> accountId, txn)
        |> Seq.groupBy fst
        |> Seq.map(fun (accountId, rows) -> accountId, rows |> Seq.map snd)
        |> Seq.tryHead
        |> async.Return
    dataStore, buildApi load save

let customer = { Name = "Joe" }

[<Fact>]
let ``Creates an account if none exists``() =
    async {
        let _, api = createInMemApi()
        let! account = api.LoadAccount customer
        test <@ account.Balance = 0M @> }
    |> Async.RunSynchronously

[<Fact>]
let ``Multiple deposits are correctly stored and aggregated``() =
    async {
        let store, api = createInMemApi()
        do! customer |> api.Deposit 10M |> Async.Ignore
        let! account = customer |> api.Deposit 20M

        test <@ account.Balance = 30M @>
        test <@ store.Count = 2 @> }
    |> Async.RunSynchronously

[<Fact>]
let ``Cannot withdraw if overdrawn``() =
    async {
        let _, api = createInMemApi()
        do! customer |> api.Withdraw 10M |> Async.Ignore
        let! result = customer |> api.Withdraw 20M

        test <@ result = Result.Failure "Account is overdrawn!" @> }
    |> Async.RunSynchronously
