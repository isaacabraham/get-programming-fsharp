#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open Capstone2.Operations
open Capstone2.Domain
open Capstone2.Auditing
open System

// Create console-auditing withdraw and deposit functions.
// Notice that the signatures of the new "shadowed" functions
// have the same signature as the original ones. This is equivalent
// to a Decorator in OO terms.
let withdraw = auditAs "withdraw" console withdraw
let deposit = auditAs "deposit" console deposit

let customer = { Name = "Isaac" }
let account = { AccountId = Guid.Empty; Owner = customer; Balance = 90M }

// Test out withdraw
let newAccount = account |> withdraw 10M
newAccount.Balance = 80M // should be true!

// Test out console auditer
console account "Testing console audit"
// should print "Account 00000000-0000-0000-0000-000000000000: Testing console audit"

account
|> withdraw 50M
|> deposit 50M 
|> deposit 100M 
|> withdraw 50M 
|> withdraw 350M