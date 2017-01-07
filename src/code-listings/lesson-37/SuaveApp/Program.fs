open Newtonsoft.Json
open Suave
open Suave.Filters
open Suave.Operators
open System.Text
open Suave.Successful

/// Convert a result to a JSON-enabled WebPart
let asJson mapper =
    Suave.Json.mapJsonWith
        (fun b -> JsonConvert.DeserializeObject<'TIn>(Encoding.UTF8.GetString b))
        (JsonConvert.SerializeObject >> Encoding.UTF8.GetBytes)
        mapper

/// Converts a result to a WebPart
let asResponse res notFoundMessage =
    match res with
    | Some (Controllers.Success result) -> OK (result |> JsonConvert.SerializeObject)
    | Some (Controllers.Failure errorMessage) -> Suave.RequestErrors.BAD_REQUEST errorMessage
    | None -> Suave.RequestErrors.NOT_FOUND notFoundMessage

/// Wraps the getAnimal function 
let getAnimal name ctx = async {
    let! animal = Controllers.AnimalsRepository.getAnimal name
    return! asResponse animal "No such animal" ctx }

let app logger =
    choose [
        GET >=> 
            choose [
                path "/api/animals" >=> (Controllers.AnimalsRepository.getAll |> asJson)
                pathScan "/api/animals/%s" getAnimal ]
        Suave.RequestErrors.NOT_FOUND "This is the default HTTP 404 page." ]
        >=> log logger logFormat

[<EntryPoint>]
let main _ = 
    let logger = Logging.LiterateConsoleTarget(Array.empty, Logging.LogLevel.Debug)
    let config = Suave.Web.defaultConfig.withLogger logger
    Suave.Web.startWebServer config (app logger)
    0
