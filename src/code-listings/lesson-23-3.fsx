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

// Listing 23.5
let createCustomer customerId contactDetails secondaryDetails =
    { CustomerId = customerId
      PrimaryContactDetails = contactDetails
      SecondaryContactDetails = secondaryDetails }

let customer = createCustomer (CustomerId "C-123") (Email "nicki@myemail.com") None

// Listing 23.6
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


// Listing 23.8
let insertContactUnsafe contactDetails =
  if contactDetails = (Email "nicki@myemail.com") then
    { CustomerId = CustomerId "ABC"
      PrimaryContactDetails = contactDetails
      SecondaryContactDetails = None }
  else failwith "Unable to insert  - customer already exists."

type Result<'a> =
| Success of 'a
| Failure of string

let insertContact contactDetails =
  if contactDetails = (Email "nicki@myemail.com") then
    Success { CustomerId = CustomerId "ABC"
              PrimaryContactDetails = contactDetails
              SecondaryContactDetails = None }
  else Failure "Unable to insert  - customer already exists."

match insertContact (Email "nicki@myemail.com") with
| Success customer -> printfn "Saved with %A" customer.CustomerId
| Failure error -> printfn "Unable to save: %s" error
