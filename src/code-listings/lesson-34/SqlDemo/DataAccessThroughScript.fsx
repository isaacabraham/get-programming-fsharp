#load @"..\..\..\..\.paket\load\netstandard2.0\main.group.fsx"
      "CustomerRepository.fs"

let scriptConnectionString = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"

CustomerRepository.printCustomers(scriptConnectionString)
