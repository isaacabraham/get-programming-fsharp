#I @"..\..\..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

// Listing 38.1
open FSharp.Data
type AllAnimalsResponse = JsonProvider<"http://localhost:8080/api/animals">
let names =
    AllAnimalsResponse.GetSamples()
    |> Seq.map(fun a -> a.Name)
    |> Seq.toArray

// Listing 38.2
type GetAnimalResponse = JsonProvider<"http://localhost:8080/api/animals/Felix">
let getAnimal = sprintf "http://localhost:8080/api/animals/%s" >> GetAnimalResponse.Load
getAnimal "Felix"

// Listing 38.3
type Result<'TSuccess> = Success of 'TSuccess | Failure of exn
let ofFunc code =
    try code() |> Success
    with | ex -> Failure ex
let getAnimalSafe animal =
    (fun () -> sprintf "http://localhost:8080/api/animals/%s" animal |> GetAnimalResponse.Load)
    |> ofFunc
let frodo = getAnimalSafe "frodo"

#r @"Http.fs\lib\net40\HttpClient.dll"
open HttpClient

// Listing 38.4

createRequest Get "http://host/api/animals"
|> withCookie { name = "Foo"; value = "Bar" }
|> withHeader (ContentType "test/json")
|> withKeepAlive true
|> getResponse

// Listing 38.5
#r @"Http.fs\lib\net40\HttpClient.dll"
open HttpClient
let request = createRequest Get "http://localhost:8080/api/animals"
let response = request |> getResponse

// Listing 38.6
#r @"Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"
open Newtonsoft.Json
let buildRoute = sprintf "http://localhost:8080/api/%s"
let httpGetResponse = buildRoute >> createRequest Get >> getResponse

// Listing 38.7
type Animal = { Name : string; Species : string }
let tryGetAnimal animal =
    let response = sprintf "animals/%s" animal |> httpGetResponse
    response.EntityBody
    |> Option.map(fun body -> JsonConvert.DeserializeObject<Animal>(body))

tryGetAnimal "Felix"

// Listing 38.9
#r "YamlDotNet/lib/net35/YamlDotNet.dll"
#r "SwaggerProvider/lib/net45/SwaggerProvider.dll"
#r "SwaggerProvider/lib/net45/SwaggerProvider.Runtime.dll"

open SwaggerProvider
type SwaggerAnimals = SwaggerProvider<"http://localhost:8080/swagger/docs/v1">

let animalsApi = SwaggerAnimals()

let animal = animalsApi.AnimalsGetByName("Felix")
animal







let tryGetAnimalSmart animal =
    let resp = sprintf "animals/%s" animal |> httpGetResponse
    match resp.StatusCode, resp.EntityBody with
    | 200, Some body -> Some(JsonConvert.DeserializeObject<Animal>(body))
    | 400, Some error ->
        printfn "ERROR: %s" error
        None
    | 500, Some error ->
        printfn "CRITICAL ERROR: %s" error
        None
    | _ -> None

tryGetAnimalSmart "error"
