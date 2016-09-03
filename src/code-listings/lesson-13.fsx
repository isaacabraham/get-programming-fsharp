// Listing 13.1
type FootballResult = { HomeTeam : string; AwayTeam : string; HomeGoals : int; AwayGoals : int }
let create (ht, hg) (at, ag) = { HomeTeam = ht; AwayTeam = at; HomeGoals = hg; AwayGoals = ag }
let results =
    [ create ("Messiville", 1) ("Ronaldo City", 2)
      create ("Messiville", 1) ("Bale Town", 3)
      create ("Bale Town", 3) ("Ronaldo City", 1)
      create ("Bale Town", 2) ("Messiville", 1)
      create ("Ronaldo City", 4) ("Messiville", 2)
      create ("Ronaldo City", 1) ("Bale Town", 2) ]

// Listing 13.2
open System.Collections.Generic

type TeamSummary = { Name : string; mutable AwayWins : int }
let summary = ResizeArray()

for result in results do
    if result.AwayGoals > result.HomeGoals then
        let mutable found = false
        for entry in summary do
            if entry.Name = result.AwayTeam then
                found <- true
                entry.AwayWins <- entry.AwayWins + 1
        if not found then
            summary.Add { Name = result.AwayTeam; AwayWins = 1 }
            
let comparer =
    { new IComparer<TeamSummary> with
        member this.Compare(x,y) =
            if x.AwayWins > y.AwayWins then -1
            elif x.AwayWins < y.AwayWins then 1
            else 0 }

summary.Sort(comparer)

// Listing 13.4
let isAwayWin result = result.AwayGoals > result.HomeGoals

results
|> List.filter isAwayWin
|> List.countBy(fun x -> x.AwayTeam)
|> List.sortByDescending(fun (_, awayWins) -> awayWins)

// Listing 13.5
let numbers = [| 1; 2; 3; 4; 6 |]
let firstNumber = numbers.[0]
let firstThreeNumbers = numbers.[0 .. 2]
numbers.[0] <- 99

// Listing 13.6
do
    let numbers = [ 1; 2; 3; 4; 5; 6 ]
    let numbersQuick = [ 1 .. 6 ]
    let head :: tail = numbers
    let moreNumbers = 0 :: numbers
    let evenMoreNumbers = moreNumbers @ [ 7 .. 9 ]
    ()
