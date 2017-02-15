#I @"..\..\..\packages\"
#r @"FsCheck\lib\net45\FsCheck.dll"
#r @"xunit.extensibility.core\lib\dotnet\xunit.core.dll"
#r @"xunit.abstractions\lib\net35\xunit.abstractions.dll"
#r @"FsCheck.Xunit\lib\net45\FsCheck.Xunit.dll"

open System
open FsCheck
open FsCheck.Xunit

// Listing 41.1
let sumsNumbers numbers =
    numbers |> List.fold (+) 0

[<Property(Verbose = true)>]
let ``Correctly adds numbers`` numbers =
    let actual = sumsNumbers numbers
    actual = List.sum numbers

// Listing 41.2
let flipCase (text:string) =
    text.ToCharArray()
    |> Array.map(fun c -> if Char.IsUpper c then Char.ToLower c else Char.ToUpper c)
    |> String

[<Property>]
let ``Always has same number of letters`` (input:string) =
    let output = input |> flipCase
    input.Length = output.Length
    

// Listing 41.3
[<Property>]
let ``Always has same number of letters with Guard Clause`` (input:string) =
    input <> null ==> lazy
        let output = input |> flipCase
        input.Length = output.Length

// Listing 41.4
type LettersOnlyGen() =
    static member Letters() =
        Arb.Default.Char() |> Arb.filter Char.IsLetter

[<Property(Arbitrary = [| typeof<LettersOnlyGen> |])>]
let ``Always has same number of letters with Arb Gen`` (NonEmptyString input) =
    let output = input |> flipCase
    input.Length = output.Length




let noLetterIsTheSameCase (NonEmptyString input) =
    let output = input |> flipCase
    (input.ToCharArray(), output.ToCharArray())
    ||> Array.forall2 (<>)
let allLettersAreTheSame (NonEmptyString input) =
    let output = input |> flipCase
    (input.ToCharArray(), output.ToCharArray())
    ||> Array.forall2 (fun a b -> Char.ToLower a = Char.ToLower b)

// Bonus! You can also run tests directly in scripts like this: -
Check.Quick ``Correctly adds numbers`` // simple check

let config = { Config.Default with Arbitrary = [ typeof<LettersOnlyGen> ] }
Check.One(config, noLetterIsTheSameCase) // check with config