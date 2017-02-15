module ``Web Tests``

open System
open Xunit
open Swensen.Unquote
open Capstone8.Controllers
open System.Threading
open System.Web.Http
open System.Net.Http
open System.Threading.Tasks

let createController() =
    let _, api = ``API Tests``.createInMemApi()
    new BankAccountController(api, Request = new HttpRequestMessage(), Configuration = new HttpConfiguration())

let executeRequest (request:IHttpActionResult Task) =
    async {
        let! request = request |> Async.AwaitTask
        return! request.ExecuteAsync(CancellationToken.None) |> Async.AwaitTask }
    |> Async.RunSynchronously

[<Fact>]
let ``Successful withdrawal returns OK``() =
    let controller = createController()
    let result = controller.PostWithdrawal("Fred", { Amount = 10M }) |> executeRequest
    test <@ result.StatusCode = Net.HttpStatusCode.OK @>


[<Fact>]
let ``Unsuccessful withdrawal returns 400``() =
    let controller = createController()
    let result = controller.PostWithdrawal("Fred", { Amount = 10M }) |> executeRequest
    let result = controller.PostWithdrawal("Fred", { Amount = 10M }) |> executeRequest
    test <@ result.StatusCode = Net.HttpStatusCode.BadRequest @>

[<Fact>]
let ``Returns correct balance``() =
    let controller = createController()
    controller.PostDeposit("Fred", { Amount = 10M }) |> executeRequest |> ignore
    controller.PostDeposit("Fred", { Amount = 25M }) |> executeRequest |> ignore
    let result = controller.GetAccount "Fred" |> Async.AwaitTask |> Async.RunSynchronously
    test <@ result.Balance = 35M @>