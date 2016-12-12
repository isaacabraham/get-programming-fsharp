// Listing 17.1-2
open System.Collections.Generic

do
    let inventory = Dictionary<string, float>()
    //let inventory = Dictionary<_,_>()
    //let inventory = Dictionary()

    inventory.Add("Apples", 0.33)
    inventory.Add("Oranges", 0.23)
    inventory.Add("Bananas", 0.45)

    inventory.Remove "Oranges" |> ignore

    let bananas = inventory.["Bananas"]
    let oranges = inventory.["Oranges"]
    ()

// Listing 17.3
do
    let inventory : IDictionary<string, float> = 
        [ "Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45 ]
        |> dict

    let bananas = inventory.["Bananas"]

    inventory.Add("Pineapples", 0.85)
    inventory.Remove("Bananas") |> ignore

do
    // Listing 17.4
    let inventory = 
        [ "Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45 ]
        |> Map.ofList

    let apples = inventory.["Apples"]
    let pineapples = inventory.["Pineapples"]

    let newInventory =
        inventory
        |> Map.add "Pineapples" 0.87
        |> Map.remove "Apples"

    // Listing 17.5
    let cheapFruit, expensiveFruit =
        inventory
        |> Map.partition(fun fruit cost -> cost > 0.3)

    ()

// Now you try
open System
open System.IO

Directory.EnumerateDirectories(@"C:\")
|> Seq.map DirectoryInfo
|> Seq.map(fun di -> di.Name, di.CreationTimeUtc)
|> Map.ofSeq
|> Map.map(fun key value -> (DateTime.UtcNow - value).TotalDays)

// Listing 17.6
let myBasket = [ "Apples"; "Apples"; "Apples"; "Bananas"; "Pinapples" ]
let myBasketSet = myBasket |> Set.ofList

// Listing 17.7
let yourBasket = [ "Kiwi"; "Bananas"; "Grapes" ]

let bothBaskets = (myBasket @ yourBasket) |> List.distinct

let yourBasketSet = yourBasket |> Set.ofList
let bothBasketsSet = myBasketSet + yourBasketSet

// Listing 17.8
let allFruits = myBasketSet + yourBasketSet
let firstFruitsOnly = myBasketSet - yourBasketSet
let fruitsInBoth = myBasketSet |> Set.intersect yourBasketSet
let isSubset = myBasketSet |> Set.isSubset yourBasketSet

