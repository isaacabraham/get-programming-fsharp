// Listing 4.1
let age = 35
let website = System.Uri "http://fsharp.org"
let add (first, second) = first + second

// Listing 4.2
let foo() =
    let x = 10
    printfn "%d" (x + 20)
    let x = "test"
    let x = 50.0
    x + 200.0

// Listing 4.4
open System
let doStuffWithTwoNumbers(first, second) =
    let added = first + second
    Console.WriteLine("{0} + {1} = {2}", first, second, added)
    let doubled = added * 2
    doubled

// Listing 4.5
let theYear = DateTime.Now.Year
let theAge = theYear - 1979
let theEstimatedAge = sprintf "You are about %d years old!" theAge

// Listing 4.6
let estimatedAge =
    let age =
        let year = DateTime.Now.Year
        year - 1979
    sprintf "You are about %d years old!" age

// Listing 4.7
let estimateAges(familyName, age1, age2, age3) =
    let calculateAge age =
        let year = System.DateTime.Now.Year
        year - age
    
    let estimatedAge1 = calculateAge age1
    let estimatedAge2 = calculateAge age2
    let estimatedAge3 = calculateAge age3
    
    let averageAge = (estimatedAge1 + estimatedAge2 + estimatedAge3) / 3
    sprintf "Average age for family %s is %d" familyName averageAge

// Listing 4.8
open System
open System.Net
open System.Windows.Forms

let webClient = new WebClient()
let fsharpOrg = webClient.DownloadString(Uri "http://fsharp.org")
let browser = new WebBrowser(ScriptErrorsSuppressed = true, Dock = DockStyle.Fill, DocumentText = fsharpOrg)
let form = new Form(Text = "Hello from F#!")
form.Controls.Add browser
form.Show()

// Solution to Now You Try
let showBrowser uri =
    let browser =
        let fsharpOrg =
            let webClient = new WebClient()
            webClient.DownloadString(Uri uri)
        new WebBrowser(ScriptErrorsSuppressed = true, Dock = DockStyle.Fill, DocumentText = fsharpOrg)
    let form = new Form(Text = "Hello from F#!")
    form.Controls.Add browser
    form.Show()

// Listings 4.9 / 4.10
let r = System.Random()
let nextValue = r.Next(1, 6)
let answer = nextValue + 10

let generateRandomNumber max =
    let r = System.Random()
    let nextValue = r.Next(1, max)
    nextValue + 10
