module ``API Tests``

open Capstone8.Domain
open Xunit
open Swensen.Unquote

[<Fact>]
let ``Creates an account if none exists``() =
    async {
        let api = createInMemApi()
        let! account = api.LoadAccount customer
        test <@ account.Balance = 0M @> }
    |> Async.RunSynchronously