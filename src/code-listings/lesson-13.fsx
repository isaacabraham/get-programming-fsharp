// Listing 13.5
type Customer = { Age : int }
let where filter customers =
    seq {
        for customer in customers do
            if filter customer then
                yield customer }

let customers = [ { Age = 21 }; { Age = 35 }; { Age = 36 } ]

let whereCustomersAreOver35 customers =
    seq {
        for customer in customers do
            if customer.Age > 35 then
                yield customer }


let isOver35 customer = customer.Age > 35

customers |> where isOver35
customers |> where (fun customer -> customer.Age > 35)


// Listing 13.6
let printCustomerAge writer customer =
    if customer.Age < 13 then writer "Child!"
    elif customer.Age < 20 then writer "Teenager!"
    else writer "Adult!"

// Listing 13.7
open System
printCustomerAge Console.WriteLine { Age = 21 }

let printToConsole = printCustomerAge Console.WriteLine
printToConsole { Age = 21 }
printToConsole { Age = 12 }
printToConsole { Age = 18 }

// Listing 13.8
open System.IO
let writeToFile text = File.WriteAllText(@"C:\temp\output.txt", text)

let printToFile = printCustomerAge writeToFile
printToFile { Age = 21 }

let contentsFromDisk = File.ReadAllText @"C:\temp\output.txt"

