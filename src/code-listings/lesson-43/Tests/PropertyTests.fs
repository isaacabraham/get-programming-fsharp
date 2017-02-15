module ``Property Tests``

open Capstone8.Api
open Capstone8.Domain
open FsCheck
open FsCheck.Xunit

let isSuccess result = match result with Success _ -> true | Failure _ -> false

[<Property(Verbose = true)>]
let ``Going under 0 makes the account overdrawn``(PositiveInt startingBalance) =
    let startingBalance = decimal startingBalance
    true