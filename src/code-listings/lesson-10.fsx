// Listing 10.4

type Address =
    { Street : string
      Town : string
      City : string }

// Listing 10.5
type Customer =
    { Forename : string
      Surname : string
      Age : int
      Address : Address
      EmailAddress : string }

let customer =
    { Forename = "Joe"
      Surname = "Bloggs"
      Age = 30
      Address =
        { Street = "The Street"
          Town = "The Town"
          City = "The City" }
      EmailAddress = "joe@bloggs.com" }

// Listing 10.6
let address : Address =
    { Street = "The Street"
      Town = "The Town"
      City = "The City" }

let addressExplicit =
    { Address.Street = "The Street"
      Town = "The Town"
      City = "The City" }

// Listing 10.7
let updatedCustomer = { customer with Age = 31; EmailAddress = "joe@bloggs.co.uk" }

// Listing 10.8
let isSameAddress = (address = addressExplicit)

// Now you try
let updateAge customer =
    let newAge =
        let r = System.Random()
        r.Next(18, 46)
    printfn "Changed customer's age from %d to %d" customer.Age newAge
    { customer with Age = newAge }

let randomAgeCustomer = updateAge customer

// Listing 10.9
do
    let myHome = { Street = "The Street"; Town = "The Town"; City = "The City" }
    let myHome = { myHome with City = "The Other City" }
    let myHome = { myHome with City = "The Third City" }
    ()
