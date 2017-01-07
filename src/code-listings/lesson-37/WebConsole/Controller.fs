namespace Controllers

open System.Web.Http

type Animal = { Name : string; Species : string }

[<RoutePrefix("api")>]
type AnimalsController() =
    inherit ApiController()

    [<Route("animals")>]
    member __.Get() = [ { Name = "Fido"; Species = "Dog" }; { Name = "Felix"; Species = "Cat" } ]