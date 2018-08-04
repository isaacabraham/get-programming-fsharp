// Sample application entry point
let main argv =
    printfn "%A" argv
    0 // return an integer exit code

// Creating an array of items
let items = [| "item"; "item"; "item"; "item" |]

[<EntryPoint>]
let enhancedMain argv =     
    let items = argv.Length     
    printfn "Passed in %d items: %A" items argv     
    0 // return an integer exit code 

