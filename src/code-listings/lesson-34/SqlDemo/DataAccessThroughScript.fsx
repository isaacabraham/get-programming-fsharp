#load "scripts/load-references-debug.fsx"
#load "CustomerRepository.fs"

let scriptConnectionString = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"

CustomerRepository.printCustomers(scriptConnectionString)
