// Listing 36.1
printfn "Loading data!"
System.Threading.Thread.Sleep(5000)
printfn "Loaded Data!"
printfn "My name is Simon."

async {
    printfn "Loading data!"
    System.Threading.Thread.Sleep(5000)
    printfn "Loaded Data!" }
|> Async.Start
printfn "My name is Simon."

// Listing 36.2
let asyncHello : Async<string> = async { return "Hello" }
let length = asyncHello.Length
let text = asyncHello |> Async.RunSynchronously
let lengthTwo = text.Length

// Listing 36.3
open System.Threading
let printThread text = printfn "THREAD %d: %s" Thread.CurrentThread.ManagedThreadId text
let doWork() =
    printThread "Starting long running work!"
    Thread.Sleep 5000
    "HELLO"

let asyncLength =
    printThread "Creating async block"
    let asyncBlock =
        async {
            printThread "In block!"
            let text = doWork()
            return (text + " WORLD").Length }
    printThread "Created async block"
    asyncBlock

asyncLength |> Async.RunSynchronously

// Listing 36.4
let getTextAsync = async { return "HELLO" }
let printHelloWorld =
    async {
        let! text = getTextAsync
        return printf "%s WORLD" text }

printHelloWorld |> Async.Start

// Listing 36.5
let random = System.Random()
let pickANumberAsync =
    async { return random.Next(10) }

let createFiftyNumbers =
    let workflows = [ for _ in 1 .. 50 -> pickANumberAsync ]
    async {
        let! numbers = workflows |> Async.Parallel
        printfn "Total is %d" (numbers |> Array.sum) }
    
createFiftyNumbers |> Async.Start

// Listing 36.6
let urls = [| "http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com" |]
let downloadData url = async {
    use wc = new System.Net.WebClient()
    printfn "Downloading data on thread %d" System.Threading.Thread.CurrentThread.ManagedThreadId
    let! data = wc.AsyncDownloadData(System.Uri url)
    return data.Length }

let downloadedBytes =
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.RunSynchronously
    
printfn "You downloaded %d characters" (Array.sum downloadedBytes)

// Listing 36.7
let downloadData url = async {
    use wc = new System.Net.WebClient()
    printfn "Downloading data on thread %d" System.Threading.Thread.CurrentThread.ManagedThreadId
    let! data = wc.DownloadDataTaskAsync(System.Uri url) |> Async.AwaitTask
    return data.Length }

let downloadedBytes =
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.StartAsTask
    
printfn "You downloaded %d characters" (Array.sum downloadedBytes.Result)

// Bonus - custom computation expression!
type Maybe() =
    member __.Bind(opt, func) = opt |> Option.bind func
    member __.Return v = Some v

let maybe = Maybe()

let rateCustomer name =
    match name with
    | "isaac" -> Some 3
    | "mike" -> Some 2
    | _ -> None
let answer =
    maybe {
        let! first = rateCustomer "isaac"
        let! second = rateCustomer "mike"
        return first + second }