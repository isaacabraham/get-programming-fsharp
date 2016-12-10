// Listing 23.1
type Customer =
    { CustomerId : string
      Email : string
      Telephone : string
      Address : string }

// Listing 23.2
let createCustomer customerId email telephone address =
    { CustomerId = telephone
      Email = customerId
      Telephone = address
      Address = email }
let customer = createCustomer "C-123" "nicki@myemail.com" "029-293-23" "1 The Street"


