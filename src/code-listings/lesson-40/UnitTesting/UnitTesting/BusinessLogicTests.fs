module ``Business Logic Tests``

open BusinessLogic

open Xunit

let department =
    { Name = "Super Team"
      Team = [ for i in 1 .. 15 -> { Name = sprintf "Person %d" i; Age = 19 } ] }

// Listing 40.2
[<Fact>]
let ``Large, young teams are correctly identified``() =
    Assert.True(department |> isLargeAndYoungTeam)

// Listing 40.3
let isTrue (b:bool) = Assert.True b

[<Fact>]
let ``Custom DSLs are easy to write``() =
    department
    |> isLargeAndYoungTeam
    |> isTrue

// Listing 40.4
open FsUnit.Xunit
[<Fact>]
let ``FSUnit makes nice DSLs!``() =
    department
    |> isLargeAndYoungTeam
    |> should equal true

    department.Team.Length
    |> should be (greaterThan 10)

// Listing 40.5
open Swensen.Unquote
[<Fact>]
let ``Unquote has a simple custom operator for equality``() =
    department |> isLargeAndYoungTeam =! true

// Listing 40.6
[<Fact>]
let ``Unquote can parse quotations for excellent diagnostics``() =
    let emptyTeam = { Name = "Super Team"; Team = [] }
    test <@ emptyTeam.Name.StartsWith "D" @>