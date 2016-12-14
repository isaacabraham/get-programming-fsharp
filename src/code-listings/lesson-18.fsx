// Listing 18.1
type Sum = int seq -> int
type Average = float seq -> float 
type Count<'T> = 'T seq -> int

// Listing 18.2
let sum inputs =
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + input
    accumulator

// Now you try #1
let length inputs =
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + 1
    accumulator
let lettersInTheAlphabet = [ 'a' .. 'z' ] |> length

// Listing 18.3
do
    let sum inputs =
        Seq.fold
            (fun state input -> state + input)
            0
            inputs
    ()

// Listing 18.4
do
    let sum inputs =
        Seq.fold
            (fun state input ->
                let newState = state + input
                printfn "Current state is %d, input is %d, new state value is %d" state input newState
                newState)
            0
            inputs

    sum [ 1 .. 5 ]
    ()

// Now you try #2
let lengthFold inputs =
    Seq.fold
        (fun state input -> state + 1)
        0
        inputs

let foldAlphabet = [ 'a' .. 'z' ] |> lengthFold

let maxFold inputs =
    Seq.fold
        (fun state input -> if input > state then input else state)
        0
        inputs
let shouldBeTwenty = [ 1;2;5;3;20;13;18 ] |> maxFold

// Listing 18.5
let inputs = [ 1 .. 5 ]
Seq.fold (fun state input -> state + input) 0 inputs
inputs |> Seq.fold (fun state input -> state + input) 0
(0, inputs) ||> Seq.fold (fun state input -> state + input)

// Listing 18.6
open System.IO
let mutable totalChars = 0
let sr = new StreamReader(File.OpenRead "book.txt")

while (not sr.EndOfStream) do
    let line = sr.ReadLine()
    totalChars <- totalChars + line.ToCharArray().Length

// Listing 18.7
let lines : string seq =
    seq {
        use sr = new StreamReader(File.OpenRead @"book.txt")
        while (not sr.EndOfStream) do
            yield sr.ReadLine() }

(0, lines) ||> Seq.fold(fun total line -> total + line.Length)

// Listing 18.8
open System
type Rule = string -> bool * string 

let rules : Rule list =
    [ fun text -> (text.Split ' ').Length = 3, "Must be three words"
      fun text -> text.Length <= 30, "Max length is 30 characters"
      fun text -> text.ToCharArray()
                  |> Array.filter Char.IsLetter
                  |> Array.forall Char.IsUpper, "All letters must be caps" ]

// Listing 18.9
let validateManual (rules: Rule list) word =
    let passed, error = rules.[0] word
    if not passed then false, error
    else
        let passed, error = rules.[1] word
        if not passed then false, error
        else
            let passed, error = rules.[2] word
            if not passed then false, error
            else true, ""

// Listing 18.10
let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
        fun word ->
            let passed, error = firstRule word
            if passed then
                let passed, error = secondRule word
                if passed then true, "" else false, error
            else false, error)

let validate = buildValidator rules
let word = "HELLO FrOM F#"

validate word
 
// Now you try #3
module Rules =
    let threeWordRule (text:string) =
        printfn "Running three word rule"
        (text.Split ' ').Length = 3, "Must be three words"
    let maxLengthRule (text:string) =
        printfn "Running max length rule"
        text.Length <= 30, "Max length is 30 characters"
    let allCapsRule (text:string) =
        printfn "Running all caps rule"
        text
        |> Seq.filter Char.IsLetter
        |> Seq.forall Char.IsUpper, "All letters must be caps"
    let noNumbersRule (text:string) =
        printfn "Running no numbers rule"
        text
        |> Seq.forall (Char.IsNumber >> not), "Numbers are not permitted"

    let allRules = [ threeWordRule; allCapsRule; maxLengthRule; noNumbersRule ]

let debugValidate = buildValidator Rules.allRules
let pass = debugValidate "HELLO FROM F#"
let fail = debugValidate "HELLO FR0M F#"