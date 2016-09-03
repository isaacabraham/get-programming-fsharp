// Listing 3.1
let currentTime = System.DateTime.UtcNow;;
currentTime.TimeOfDay.ToString();;

// Listing 3.2
let greetPerson name age =
    sprintf "Hello, %s. You are %d years old" name age

let greeting = greetPerson "Fred" 25

