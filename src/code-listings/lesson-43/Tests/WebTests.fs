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
    let api = createInMemApi()
    let request = new HttpRequestMessage()
    let config = new HttpConfiguration()
    new BankAccountController(api, Request = request, Configuration = config)

let executeRequest (request:IHttpActionResult Task) =
    async {
        let! request = request |> Async.AwaitTask
        return! request.ExecuteAsync(CancellationToken.None) |> Async.AwaitTask }
    |> Async.RunSynchronously
