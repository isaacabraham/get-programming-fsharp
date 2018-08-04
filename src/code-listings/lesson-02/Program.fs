// You can run this application using e.g.
// dotnet run -- banana apple orange

[<EntryPoint>]
let enhancedMain argv =     
    let items = argv.Length     
    printfn "Passed in %d items: %A" items argv     
    0 // return an integer exit code 

