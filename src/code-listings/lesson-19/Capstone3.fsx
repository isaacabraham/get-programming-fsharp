#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open Capstone3.Operations
open Capstone3.Domain
open Capstone3.Auditing
open System

// Test out create account from transaction history
let transactions =
    [ { Transaction.Accepted = false; Timestamp = DateTime.MinValue; Operation = "withdraw"; Amount = 10M }
      { Transaction.Accepted = true; Timestamp = DateTime.MinValue.AddSeconds 10.; Operation = "withdraw"; Amount = 10M }
      { Transaction.Accepted = true; Timestamp = DateTime.MinValue.AddSeconds 30.; Operation = "deposit"; Amount = 50M }
      { Transaction.Accepted = true; Timestamp = DateTime.MinValue.AddSeconds 50.; Operation = "withdraw"; Amount = 10M } ]

transactions = (transactions |> List.map (Transactions.serialize >> Transactions.deserialize))

let accountId = Guid.Empty
let owner = "Isaac"
let account = loadAccount(owner, accountId, transactions)