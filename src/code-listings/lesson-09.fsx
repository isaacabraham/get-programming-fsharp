(* Note the listings below make use of the "do" keyword. These simply allow us to declare
   an abitrary scope so that we can re-use symbols e.g. "name" without getting warnings from the
   compiler. This is only required for symbols declared directly within scripts. *)

// Listing 9.3
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

// Listing 9.4
do
    let nameAndAge = ("Joe", "Bloggs"), 28
    let name, age = nameAndAge
    let (forename, surname), theAge = nameAndAge
    ()

// Listing 9.5
do
    let nameAndAge = "Jane", "Smith", 25
    let forename, surname, _ = nameAndAge
    ()

// Listing 9.6
let explicit : int * int = 10,5
let implicit = 10,5

let addNumbers arguments =
    let a, b = arguments
    a + b

// Listing 9.7
let addNumbersGeneric arguments =
    let a, b, c, _ = arguments
    a + b

