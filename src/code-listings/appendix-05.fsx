// OO features
open System

type Person(age, firstname, surname) =    
    let fullName = sprintf "%s %s" firstname surname

    member __.PrintFullName() =
        printfn "%s is %d years old" fullName age
    
    member this.Age = age
    member that.Name = fullName
    member val FavouriteColour = System.Drawing.Color.Green with get,set


type IQuack = 
    abstract member Quack : unit -> unit

// A class that implements interfaces and overrides methods
type Duck() =
    interface IQuack with
        member this.Quack() = printfn "QUACK!"

module Quackers =
    let superQuack =
        { new IQuack with
            member this.Quack() = printfn "What type of animal am I?" }
        

[<AbstractClass>]
type Employee(name:string) =
    member __.Name = name
    abstract member Work : unit -> string
    member this.DoWork() =
        printfn "%s is working hard: %s!" name (this.Work())

type ProjectManager(name:string) =
    inherit Employee(name)
    override this.Work() = "Creating a project plan"

// Exception Handling
module Exceptions =
    let riskyCode() =
        raise(ApplicationException())
        ()
    let runSafely() =
        try riskyCode()
        with
        | :? ApplicationException as ex -> printfn "Got an application exception! %O" ex
        | :? System.MissingFieldException as ex -> printfn "Got a missing field exception! %O" ex
        | ex -> printfn "Got some other type of exception! %O" ex


// Resource Management
module ResourceManagement =
    let createDisposable() =
        printfn "Created!"
        { new IDisposable with member __.Dispose() = printfn "Disposed!" }
    
    let foo() =
        use x = createDisposable()
        printfn "inside!"
    
    let bar() =
        using (createDisposable()) (fun x ->
            printfn "inside!")

// Casting
module Casting =
    let anException = Exception()
    let upcastToObject = anException :> obj
    let upcastToAppException = anException :> ApplicationException
    let downcastToAppException = anException :?> ApplicationException
    let downcastToString = anException :?> string

// Active Patterns
module ActivePatterns =
    let (|Long|Medium|Short|) (value:string) =
        if value.Length < 5 then Short
        elif value.Length < 10 then Medium
        else Long

    match "Hello" with
    | Short -> "This is a short string!"
    | Medium -> "This is a medium string!"
    | Long -> "This is a long string!"

// Lazy Computations
module Lazy =
    let lazyText =
        lazy
            let x = 5 + 5
            printfn "%O: Hello! Answer is %d" System.DateTime.UtcNow x
            x
    
    let text = lazyText.Value
    let text2 = lazyText.Value

// Recursion
module Recursion =
    let rec factorial number total =
        if number = 1 then total
        else
            printfn "Number %d" number
            factorial (number - 1) (total * number)
    
    factorial 5 1