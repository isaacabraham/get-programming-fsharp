// Listing 15.1-2
open System.Collections.Generic

do
    let inventory = Dictionary<string, float>()
    //let inventory = Dictionary<_,_>()
    //let inventory = Dictionary()

    inventory.Add("Apples", 0.33)
    inventory.Add("Oranges", 0.23)
    inventory.Add("Bananas", 0.45)

    inventory.Remove "Oranges"

    let bananas = inventory.["Bananas"]
    let oranges = inventory.["Oranges"]
    ()

// Listing 15.3
do
    let inventory : IDictionary<string, float> = 
        [ "Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45 ]
        |> dict

    let bananas = inventory.["Bananas"]

    inventory.Add("Pineapples", 0.85)
    inventory.Remove("Bananas")
    ()

do
    // Listing 15.4
    let inventory = 
        [ "Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45 ]
        |> Map.ofList

    let apples = inventory.["Apples"]
    let pineapples = inventory.["Pineapples"]

    let newInventory =
        inventory
        |> Map.add "Pineapples" 0.87
        |> Map.remove "Apples"

    // Listing 15.5
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

// Listing 15.6
let fruits = [ "Apples"; "Apples"; "Apples"; "Bananas"; "Pinapples" ]
let fruitsSet = fruits |> Set

// Listing 15.7
let otherFruits = [ "Kiwi"; "Bananas"; "Grapes" ]

let allFruitsList = (fruits @ otherFruits) |> List.distinct

let otherFruitsSet = Set otherFruits
let allFruitsSet = fruitsSet + otherFruitsSet

// Listing 15.8
let allFruits = fruitsSet + otherFruitsSet
let firstFruitsOnly = fruitsSet - otherFruitsSet
let fruitsInBoth = fruitsSet |> Set.intersect otherFruitsSet
let isSubset = fruitsSet |> Set.isSubset otherFruitsSet

