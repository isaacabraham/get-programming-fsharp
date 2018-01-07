// Listing 11.1
let tupledAdd(a,b) = a + b
let answer = tupledAdd (5,10)

let curriedAdd a b = a + b
let curriedAnswer = curriedAdd 5 10

// Listing 11.2
let add first second = first + second
let addFive = add 5
let fifteen = addFive 10

// Listing 11.3
open System
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYearFull month day = buildDt DateTime.UtcNow.Year month day
let buildDtThisMonthFull day = buildDtThisYearFull DateTime.UtcNow.Month day

// Listing 11.4
let buildDtThisYear = buildDt DateTime.UtcNow.Year
let buildDtThisMonth = buildDtThisYear DateTime.UtcNow.Month

// Listing 11.5
open System.IO
let writeToFile (date:DateTime) filename text =
    let path = sprintf "%s-%s.txt" (date.ToString "yyMMdd") filename
    File.WriteAllText(path, text)

// Listing 11.6
let writeToToday = writeToFile DateTime.UtcNow.Date
let writeToTomorrow = writeToFile (DateTime.UtcNow.Date.AddDays 1.)
let writeToTodayHelloWorld = writeToToday "hello-world"

writeToToday "first-file" "The quick brown fox jumped over the lazy dog"
writeToTomorrow "second-file" "The quick brown fox jumped over the lazy dog"
writeToTodayHelloWorld "The quick brown fox jumped over the lazy dog"

// Listing 11.7
let checkCreation (creationDate:DateTime) =
    if (DateTime.UtcNow - creationDate).TotalDays > 7. then sprintf "Old"
    else sprintf "New"

let time =
    let directory = Directory.GetCurrentDirectory()
    Directory.GetCreationTime directory
checkCreation time

// Listing 11.8
checkCreation(
    Directory.GetCreationTime(
        Directory.GetCurrentDirectory())) 

// Listing 11.9
Directory.GetCurrentDirectory()
|> Directory.GetCreationTime
|> checkCreation

// Listing 11.10
//let answer = 10 |> add 5 |> timesBy 2 |> add 20 |> add 7 |> timesBy 3

// loadCustomer 17 |> buildReport |> convertTo Formats.PDF |> postToQueue

(*
let customersWithOverdueOrders =
    getSqlConnection "DevelopmentDb"
    |> createDbConnection
    |> findCustomersWithOrders Status.Outstanding (TimeSpan.FromDays 7.0)
*)

// Listing 11.12
let drive distance petrol =
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

let startPetrol = 100.0

startPetrol
|> drive "far"
|> drive "medium"
|> drive "short"

// Listing 11.13
let checkCurrentDirectoryAge =
    Directory.GetCurrentDirectory
    >> Directory.GetCreationTime
    >> checkCreation
let description = checkCurrentDirectoryAge() 
