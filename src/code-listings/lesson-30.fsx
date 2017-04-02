//  Listing 30.1
#I @"..\..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"

open FSharp.Data
type Football = CsvProvider< @"..\..\data\FootballResults.csv">
let data = Football.GetSample().Rows |> Seq.toArray

//  Listing 30.2
#r @"Google.DataTable.Net.Wrapper\lib\Google.DataTable.Net.Wrapper.dll"
#r @"XPlot.GoogleCharts\lib\net45\XPlot.GoogleCharts.dll"
open XPlot.GoogleCharts

data
|> Seq.filter(fun row ->
    row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
|> Seq.countBy(fun row -> row.``Home Team``)
|> Seq.sortByDescending snd
|> Seq.take 10
|> Chart.Column
|> Chart.Show