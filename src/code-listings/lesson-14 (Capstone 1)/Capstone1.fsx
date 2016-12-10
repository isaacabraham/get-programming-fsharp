#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open Capstone1.Operations
open Capstone1.Domain
open Capstone1.Auditing
open System

let withdraw = withdraw |> auditAs "withdraw" console
let deposit = deposit |> auditAs "deposit" console

let customer = { Name = "Isaac" }
let account = { AccountId = Guid.NewGuid(); Owner = customer; Balance = 90M }

account
|> withdraw 50M
|> deposit 50M 
|> deposit 100M 
|> withdraw 50M 
|> withdraw 350M