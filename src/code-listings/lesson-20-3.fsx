// Now you try #2
type ContactDetails =
| Address of string
| Telephone of string
| Email of string
type CustomerId = CustomerId of string
type Customer =
    { CustomerId : CustomerId
      PrimaryContactDetails : ContactDetails
      SecondaryContactDetails : ContactDetails option }

// Listing 20.5
let createCustomer customerId contactDetails secondaryDetails =
    { CustomerId = customerId
      PrimaryContactDetails = contactDetails
      SecondaryContactDetails = secondaryDetails }

let customer = createCustomer (CustomerId "C-123") (Email "nicki@myemail.com") None

// Listing 20.6
type GenuineCustomer = GenuineCustomer of Customer

let validateCustomer customer =
    match customer.PrimaryContactDetails with
    | Email e when e.EndsWith "SuperCorp.com" -> Some(GenuineCustomer customer)
    | Address _ | Telephone _ -> Some(GenuineCustomer customer)
    | Email _ -> None

let sendWelcomeEmail (GenuineCustomer customer) =
    printfn "Hello, %A, and welcome to our site!" customer.CustomerId

sendWelcomeEmail customer // does not compile

// compiles - only called if validate customer returns Some(GenuineCustomer _) 
customer
|> validateCustomer
|> Option.map sendWelcomeEmail
