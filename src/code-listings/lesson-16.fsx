// Listing 16.1
let numbers = [ 1 .. 10 ]
let timesTwo n = n * 2

let outputImperative = ResizeArray()
for number in numbers do
    outputImperative.Add (number |> timesTwo)

let outputFunctional = numbers |> List.map timesTwo

// Listing 16.2
type Order = { OrderId : int }
type Customer = { CustomerId : int; Orders : Order list; Town : string }
let customers : Customer list = []
let orders : Order list = customers |> List.collect(fun c -> c.Orders)

// Listing 16.3
open System

[ DateTime(2010,5,1); DateTime(2010,6,1); DateTime(2010,6,12); DateTime(2010,7,3) ]
|> List.pairwise
|> List.map(fun (a, b) -> b - a)
|> List.map(fun time -> time.TotalDays)

// Listing 16.4
let londonCustomers, otherCustomers =
    customers
    |> List.partition(fun c -> c.Town = "London")

// Listing 16.5
do
    let numbers = [ 1.0 .. 10.0 ]
    let total = numbers |> List.sum
    let average = numbers |> List.average
    let max = numbers |> List.max
    let min = numbers |> List.min
    ()

// Listing 16.6
let numberOne =
    [ 1 .. 5 ]
    |> List.toArray
    |> Seq.ofArray
    |> Seq.head
