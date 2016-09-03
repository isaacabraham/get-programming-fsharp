// Listing 10.1
let tupledAdd(a,b) = a + b
let answer = tupledAdd (5,10)

let curriedAdd a b = a + b
let curriedAnswer = curriedAdd 5 10

// Listing 10.2
open System
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYearFull month day = buildDt DateTime.UtcNow.Year month day
let buildDtThisMonthFull day = buildDtThisYearFull DateTime.UtcNow.Month day

// Listing 10.3
let buildDtThisYear = buildDt DateTime.UtcNow.Year
let buildDtThisMonth = buildDtThisYear DateTime.UtcNow.Month

// Listing 10.4
open System.IO
let writeToFile date filename text =
    let path = sprintf "%O-%s.txt" date filename
    File.WriteAllText(path, text)

// Listing 10.5
let writeToToday = writeToFile DateTime.UtcNow.Date
let writeToTomorrow = writeToFile (DateTime.UtcNow.Date.AddDays 1.)
let writeToTodayHelloWorld = writeToToday "hello-world"

writeToToday "first-file" "The quick brown fox jumped over the lazy dog"
writeToTomorrow "second-file" "The quick brown fox jumped over the lazy dog"
writeToTodayHelloWorld "The quick brown fox jumped over the lazy dog"

// Listing 10.6
let checkCreation (creationDate:DateTime) =
    if (DateTime.UtcNow - creationDate).TotalDays > 7. then printfn "Old"
    else printfn "New"

let time =
    let directory = Directory.GetCurrentDirectory()
    Directory.GetCreationTime directory
checkCreation time

// Listing 10.7
checkCreation(
    Directory.GetCreationTime(
        Directory.GetCurrentDirectory())) 

// Listing 10.8
Directory.GetCurrentDirectory()
|> Directory.GetCreationTime
|> checkCreation

// Listing 10.9
let answer = 10 |> add 5 |> timesBy 2 |> add 20 |> add 7 |> timesBy 3 #A

// loadCustomer 17 |> buildReport |> convertTo Formats.PDF |> postToQueue #B

(*
let customersWithOverdueOrders =
    getSqlConnection "DevelopmentDb"
    |> createDbConnection
    |> findCustomersWithOrders Status.Outstanding (TimeSpan.FromDays 7.0)
*)

// Listing 10.11
let drive distance petrol =
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

let startPetrol = 100.0

startPetrol
|> drive "far"
|> drive "medium"
|> drive "short"

// Listing 10.12
let checkCurrentDirectoryAge =
    Directory.GetCurrentDirectory
    >> Directory.GetCreationTime
    >> checkCreation
let description = checkCurrentDirectoryAge() 
