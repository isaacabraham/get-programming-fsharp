// Listing 23.3
type Address = Address of string
let myAddress = Address "1 The Street"
let isTheSameAddress = (myAddress = "1 The Street")
let (Address addressData) = myAddress

// Now you try #1
type CustomerId = CustomerId of string
type Email = Email of string
type Telephone = Telephone of string
type Customer =
    { CustomerId : CustomerId
      Email : Email
      Telephone : Telephone
      Address : Address }

let createCustomer customerId email telephone address =
    { CustomerId = customerId
      Email = email
      Telephone = telephone
      Address = address }
let customer =
  createCustomer
    (CustomerId "C-123")
    (Email "nicki@myemail.com")
    (Telephone "029-293-23")
    (Address "1 The Street")