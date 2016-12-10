module CustomerRepository

open FSharp.Data

let [<Literal>] private CompileTimeConnection = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
type private GetCustomers = SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", CompileTimeConnection>

let printCustomers(runtimeConnection:string) =
    let customers = GetCustomers.Create(runtimeConnection)

    customers.Execute()
    |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)

