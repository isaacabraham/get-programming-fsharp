// Listing 19.5
let aNumber : int = 10
let maybeANumber : int option = Some 10

let calculateAnnualPremiumUsd score =
    match score with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None ->
        printfn "No score supplied! Using temporary premium."
        300

calculateAnnualPremiumUsd (Some 250)
calculateAnnualPremiumUsd None

// Now you try #1
type Driver = { Name : string; SafetyScore : int option; YearPassed : int }

let drivers =
    [ { Name = "Fred Smith"; SafetyScore = Some 550; YearPassed = 1980 }
      { Name = "Jane Dunn"; SafetyScore = None; YearPassed = 1980 } ]

let calculatePremiumForCustomer customer =
    match customer.SafetyScore with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None ->
        printfn "No score supplied! Using temporary premium."
        300

// Listing 19.7
let customer = drivers.[0]
let describe safetyScore = if safetyScore > 200 then "Safe" else "High Risk"

let description =
    match customer.SafetyScore with
    | Some score -> Some(describe score)
    | None -> None

let descriptionTwo =
    customer.SafetyScore
    |> Option.map(fun score -> describe score)

let shorthand = customer.SafetyScore |> Option.map describe
let optionalDescribe = Option.map describe
