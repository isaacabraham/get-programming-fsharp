module ``Property Tests``

open Capstone8.Api
open FsCheck
open FsCheck.Xunit
open Capstone8.Domain
open ``API Tests``

let isSuccess result = match result with Success _ -> true | Failure _ -> false

[<Property(Verbose = true)>]
let ``Going under 0 makes the account overdrawn``(PositiveInt startingBalance) =
    let startingBalance = decimal startingBalance
    async {
        let _, api = createInMemApi()
        do! customer |> api.Deposit startingBalance |> Async.Ignore
        let! account = customer |> api.Withdraw (startingBalance + 1M)
        match account with
        | Success (Overdrawn _) -> return true
        | Success (InCredit _) -> return false
        | Failure _ -> return false }
    |> Async.RunSynchronously

[<Property(Verbose = true)>]
let ``Withdraws fail if overdrawn``(PositiveInt withdrawAmount)=
    let withdrawAmount = decimal withdrawAmount
    async {
        let _, api = createInMemApi()
        do! customer |> api.Withdraw withdrawAmount |> Async.Ignore
        let! accountResult = customer |> api.Withdraw 1M
        let! account = api.LoadAccount customer
        match accountResult with
        | Failure _ when account.Balance = -withdrawAmount -> return true
        | _ -> return false }
    |> Async.RunSynchronously
