namespace Controllers

open System
open System.Web.Http
open System.Net.Http
open System.Net
open System.Web.Http.Description

type Animal =
    { /// The name of the animal e.g. Felix, Fido.
      Name : string
      /// The species of the animal e.g. Cat, Dog.
      Species : string }

type Result<'T> = Success of 'T | Failure of error:string

module AnimalsRepository =
    let all = [ { Name = "Fido"; Species = "Dog" }; { Name = "Felix"; Species = "Cat" } ]
    let getAll() = all
    let getAnimal name = async {
        return 
            match name with
            | "error" -> failwith "Argh!"
            | name when name |> Seq.exists(Char.IsLetter >> not) -> Some(Failure "Invalid request. Provide a valid name.")
            | _ -> all |> List.tryFind(fun r -> r.Name = name) |> Option.map Success }

[<AutoOpen>]
module Helpers =
    open System.Web

    /// Gets an Async<Option<Result<T>>> and maps it to an Task<HttpResponseMessage>.
    let asResponseAsync (request:HttpRequestMessage) result =
        async {
            let! result = result
            return
                match result with
                | Some (Failure errorMessage) -> request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage)
                | Some (Success result) -> request.CreateResponse(HttpStatusCode.OK, result)
                | None -> request.CreateResponse(HttpStatusCode.NotFound)
        } |> Async.StartAsTask
    
    /// A simple wrapper function to lift a non-async response into an async response
    let asResponse createResponse result =
        let asyncResult = async.Return result
        asResponseAsync createResponse asyncResult

[<RoutePrefix("api")>]
type AnimalsController() =
    inherit ApiController()

    [<Route("animals")>]
    [<ResponseType(typeof<Animal list>)>]
    /// Gets all animals.
    member __.GetAll() = AnimalsRepository.getAll()

    [<Route("animals/{name}")>]
    [<ResponseType(typeof<Animal>)>]
    /// Gets an animal by name.
    member this.GetByName(name) =
        AnimalsRepository.getAnimal name
        |> asResponseAsync this.Request
