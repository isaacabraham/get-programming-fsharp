#r @"..\..\packages\FSharp.Data.SqlClient\lib\net40\FSharp.Data.SqlClient.dll"

open FSharp.Data

// Listing 32.1
let [<Literal>] Conn = @"Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
type GetCustomers = SqlCommandProvider<"SELECT * FROM SalesLT.Customer", Conn>
let customers = GetCustomers.Create(Conn).Execute() |> Seq.toArray
let customer = customers.[0]

// Now you try #1
printfn "%s %s works for %s" customer.FirstName customer.LastName (customer.CompanyName |> defaultArg <| "no-one")

// Now you try #2
type AdventureWorks = SqlProgrammabilityProvider<Conn>
let productCategory = new AdventureWorks.SalesLT.Tables.ProductCategory()
productCategory.AddRow("Mittens", Some 3)
productCategory.AddRow("Long Shorts", Some 3)
productCategory.AddRow("Wooly Hats", Some 4)
productCategory.Update()

// Listing 32.2
type Categories = SqlEnumProvider<"SELECT Name, ProductCategoryId FROM SalesLT.ProductCategory", Conn>
let woolyHats = Categories.``Wooly Hats``
printfn "Wooly Hats has ID %d" woolyHats