// Listing 32.3
#r @"..\..\packages\SQLProvider\lib\FSharp.Data.SqlProvider.dll"
open FSharp.Data.Sql

type AdventureWorks = SqlDataProvider<ConnectionString = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI", UseOptionTypes = true>

let context = AdventureWorks.GetDataContext()

type Customer = { Name : string }

let customers = 
    query {
        for customer in context.SalesLt.Customer do
        take 10
    } |> Seq.toArray
let customer = customers.[0]

// Listing 32.4
let names = 
    query {
        for customer in context.SalesLt.Customer do
        where (customer.CompanyName = Some "Sharp Bikes")
        select { Name = (customer.FirstName + " " + customer.LastName) }
        distinct
    } |> Seq.toArray

// Listing 32.5
let category = context.SalesLt.ProductCategory.Create()
category.ParentProductCategoryId <- Some 3
category.Name <- "Scarf"
context.SubmitUpdates()

// Listing 32.6
let mittens =
    context.SalesLt.ProductCategory
                   .Individuals
                   .``As Name``
                   .
