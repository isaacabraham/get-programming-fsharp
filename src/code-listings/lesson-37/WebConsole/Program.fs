open Microsoft.Owin.Hosting
open Newtonsoft.Json.Serialization
open Owin
open System
open System.Web.Http

[<Sealed>]
type Startup() =
    static member RegisterWebApi(config: HttpConfiguration) =
        // Configure routing
        config.MapHttpAttributeRoutes()

        // Configure serialization
        config.Formatters.Remove(config.Formatters.XmlFormatter) |> ignore
        config.Formatters.JsonFormatter.SerializerSettings.ContractResolver <- DefaultContractResolver()

    member __.Configuration(builder: IAppBuilder) =
        let config = new HttpConfiguration()
        Startup.RegisterWebApi(config)
        builder.UseWebApi(config) |> ignore

[<EntryPoint>]
let main _ = 
    use app = WebApp.Start<Startup>(url = "http://localhost:9000/")
    printfn "Listening on localhost:9000!"
    Console.ReadLine() |> ignore
    
    0 // return an integer exit code

