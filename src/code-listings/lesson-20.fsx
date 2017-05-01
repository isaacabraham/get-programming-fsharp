System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
    
// Listing 20.1
for number in 1 .. 10 do
    printfn "%d Hello!" number

for number in 10 .. -1 .. 1 do
    printfn "%d Hello!" number

let customerIds = [ 45 .. 99 ]
for customerId in customerIds do
    printfn "%d bought something!" customerId

for even in 2 .. 2 .. 10 do
    printfn "%d is an even number!" even

// Listing 20.2
open System.IO
let reader = new StreamReader(File.OpenRead @"File.txt")
while (not reader.EndOfStream) do
    printfn "%s" (reader.ReadLine())

// Listing 20.3
open System

let arrayOfChars = [| for c in 'a' .. 'z' -> Char.ToUpper c |]
let listOfSquares = [ for i in 1 .. 10 -> i * i ]
let seqOfStrings = seq { for i in 2 .. 4 .. 20 -> sprintf "Number %d" i }
seqOfStrings
    
// Listing 20.4
let getLimit (score, years) =
    if score = "medium" && years = 1 then 500
    elif score = "good" && (years = 0 || years = 1) then 750
    elif score = "good" && years = 2 then 1000
    elif score = "good" then 2000
    else 250

let customer = "good", 1
getLimit customer

// Listing 20.5-6
let getLimitPm customer =
    match customer with
    | "medium", 1 -> 500
    | "good", 0 | "good", 1 -> 750
    | "good", 2 -> 1000
    | "good", _ -> 2000
    | _ -> 250

getLimitPm customer    

// Listing 20.7
let getCreditLimit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years ->
        match years with
        | 0 | 1 -> 750
        | 2 -> 1000
        | _ -> 2000
    | _ -> 250

// Now you try #3
type Customer = { Balance : int; Name : string }

let handleCustomer customers =
    match customers with
    | [] -> failwith "No customers supplied!"
    | [ customer ] -> printfn "Single customer, name is %s" customer.Name
    | [ first; second ] -> printfn "Two customers, balance = %d" (first.Balance + second.Balance)
    | customers -> printfn "Customers supplied: %d" customers.Length

handleCustomer [] // throws exception
handleCustomer [ { Balance = 10; Name = "Joe" } ] // prints name

// Listing 20.9
let getStatus customer =
    match customer with
    | { Balance = 0 } -> "Customer has empty balance!"
    | { Name = "Isaac" } -> "This is a great customer!"
    | { Name = name; Balance = 50 } -> sprintf "%s has a large balance!" name
    | { Name = name } -> sprintf "%s is a normal customer" name

{ Balance = 50; Name = "Joe" } |> getStatus

// Listing 20.10
let customers = [ { Balance = 10; Name = "Joe" } ]
match customers with
| [ { Name = "Tanya" }; { Balance = 25 }; _ ] -> "It's a match!"
| _ -> "No match!"

// Listing 20.11
let customerTwo = { Balance = 150; Name = "Isaac" }
if customerTwo.Name = "Isaac" then printfn "Hello!"

match customerTwo.Name with
| "Isaac" -> printfn "Hello!"
| _ -> ()
