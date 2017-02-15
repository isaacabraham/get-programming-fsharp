[<AutoOpen>]
module internal Helpers

open System
open Capstone8.Domain
open Capstone8.Api

let customer = { Name = "Joe" }

let createInMemApi() =
    let dataStore = ResizeArray()
    let save accountId owner transaction = ()
    let load (owner:string) = async.Return None
    buildApi load save
