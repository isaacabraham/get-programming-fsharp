#I @"..\..\..\..\packages"
#r "YamlDotNet/lib/net35/YamlDotNet.dll"
#r "SwaggerProvider/lib/net45/SwaggerProvider.dll"
#r "SwaggerProvider/lib/net45/SwaggerProvider.Runtime.dll"

open SwaggerProvider
type BankApi = SwaggerProvider<"http://localhost:8080/swagger/docs/v1">

let bankApi = BankApi()

let account = bankApi.BankAccountGetAccount "Isaac"
account.Balance