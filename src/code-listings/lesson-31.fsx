// Listing 31.1
#I @"..\..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

type TvListing = JsonProvider<"http://www.bbc.co.uk/programmes/genres/comedy/schedules/upcoming.json">
let tvListing = TvListing.GetSample()
let title = tvListing.Broadcasts.[0].Programme.DisplayTitles.Title

// Now you try
#r @"Google.DataTable.Net.Wrapper\lib\Google.DataTable.Net.Wrapper.dll"
#r @"XPlot.GoogleCharts\lib\net45\XPlot.GoogleCharts.dll"
open XPlot.GoogleCharts

type Films = HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">
let deNiro = Films.GetSample()

deNiro.Tables.Film.Rows
|> Array.countBy(fun row -> string row.Year)
|> Chart.SteppedArea
|> Chart.Show



// Now you try #2
type Package = HtmlProvider< @"..\..\data\sample-package.html">

let ef = Package.Load("https://www.nuget.org/packages/entityframework")
let nunit = Package.Load("https://www.nuget.org/packages/nunit")
let newtonsoft = Package.Load("https://www.nuget.org/packages/newtonsoft.json")

// Listing 31.2
[ ef; nunit; newtonsoft ]
|> Seq.collect(fun package -> package.Tables.``Version History``.Rows)
|> Seq.sortByDescending(fun versionHistory -> versionHistory.Downloads)
|> Seq.take 10
|> Seq.map(fun vh -> vh.Version, vh.Downloads)
|> Chart.Column
|> Chart.Show
