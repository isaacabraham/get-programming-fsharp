// Listing 5.6
let add (a:int, b:int) : int =
    let answer:int = a + b
    answer

// Listing 5.7
let getLength name = sprintf "Name is %d letters." name.Length
let getLength (name:string) = sprintf "Name is %d letters." name.Length
let foo(name) = "Hello! " + getLength(name)

// Listing 5.8
open System.Collections.Generic
let numbers = List<_>()
numbers.Add(10)
numbers.Add(20)

let otherNumbers = List()
otherNumbers.Add(10)
otherNumbers.Add(20)

// Listing 5.9
let createList(first, second) =
    let output = List()
    output.Add(first)
    output.Add(second)
    output

// Listing 5.10
let sayHello(someValue) =
    let innerFunction(number) =
        if number > 10 then "Isaac"
        elif number > 20 then "Fred"
        else "Sara"

    let resultOfInner =
        if someValue < 10.0 then innerFunction(5)
        else innerFunction(15)
        
    "Hello " + resultOfInner

let result = sayHello(10.5)
