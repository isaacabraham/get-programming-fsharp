// Listing 8.3
do
    let parseName(name:string) =
        let parts = name.Split(' ')
        let forename = parts.[0]
        let surname = parts.[1]
        forename, surname
    let name = parseName("Isaac Abraham")
    let forename, surname = name

    let fname, sname = parseName("Isaac Abraham")
    ()

// Now you try
let parse (person:string) =
    let parts = person.Split(' ')
    let age = int parts.[2]
    parts.[0], parts.[1], age

let fname, sname, age = parse "Mary Simpson 24"

// Listing 8.4
do
    let nameAndAge = ("Joe", "Bloggs"), 28
    let name, age = nameAndAge
    let (forename, surname), theAge = nameAndAge
    ()

// Listing 8.5
do
    let nameAndAge = "Jane", "Smith", 25
    let forename, surname, _ = nameAndAge
    ()

// Listing 8.6
let explicit : int * int = 10,5
let implicit = 10,5

let addNumbers arguments =
    let a, b = arguments
    a + b

// Listing 8.7
let addNumbersGeneric arguments =
    let a, b, c, _ = arguments
    a + b

