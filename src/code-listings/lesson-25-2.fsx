// Listing 25.1
#I @"..\..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data
type Football = CsvProvider< @"..\..\data\FootballResults.csv">
let data = Football.GetSample().Rows |> Seq.toArray

// Listing 25.2
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