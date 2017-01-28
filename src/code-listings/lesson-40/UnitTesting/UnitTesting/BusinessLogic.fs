// Listing 40.1
module BusinessLogic

type Employee = { Name : string; Age : int }
type Department = { Name : string; Team : Employee list } 

let isLargeDepartment department = department.Team.Length > 10
let isLessThanTwenty person = person.Age < 20
let isLargeAndYoungTeam department =
    department |> isLargeDepartment
    && department.Team |> List.forall isLessThanTwenty