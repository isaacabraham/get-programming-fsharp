namespace Web

open Owin
open System.Web.Http
open Newtonsoft.Json.Serialization
open Swashbuckle.Application

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