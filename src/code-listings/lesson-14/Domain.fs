namespace Capstone1.Domain

type Customer = { Name : string }
type Account = { AccountId : System.Guid; Owner : Customer; Balance : decimal }