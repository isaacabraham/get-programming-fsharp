// Listing 12.5
type Customer = { Age : int }
let where filter customers =
    seq {
        for customer in customers do
            if filter customer then
                yield customer }

let customers = [ { Age = 21 }; { Age = 35 }; { Age = 36 } ]
let isOver35 customer = customer.Age > 35

customers |> where isOver35
customers |> where (fun customer -> customer.Age > 35)



// Listing 12.6
let printCustomerAge writer customer =
    if customer.Age < 13 then writer "Child!"
    elif customer.Age < 20 then writer "Teenager!"
    else writer "Adult!"

// Listing 12.7
open System
printCustomerAge Console.WriteLine { Age = 21 }

let printToConsole = printCustomerAge Console.WriteLine
printToConsole { Age = 21 }
printToConsole { Age = 12 }
printToConsole { Age = 18 }

// Listing 12.8
open System.IO
let writeToFile text = File.WriteAllText(@"C:\temp\output.txt", text)

let printToFile = printCustomerAge writeToFile
printToFile { Age = 21 }

