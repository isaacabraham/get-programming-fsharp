// Listing 7.3
open System

let describeAge age =
    let ageDescription =
        if age < 18 then "Child!"
        elif age < 65 then "Adult!"
        else "OAP!"

    let greeting = "Hello"
    Console.WriteLine("{0}! You are a '{1}'.", greeting, ageDescription)

// Listing 7.6
let writeTextToDisk text =
    let path = System.IO.Path.GetTempFileName()
    System.IO.File.WriteAllText(path, text)
    path
    
let createManyFiles() =
    writeTextToDisk "The quick brown fox jumped over the lazy dog"
    writeTextToDisk "The quick brown fox jumped over the lazy dog"
    writeTextToDisk "The quick brown fox jumped over the lazy dog"

createManyFiles()

// Listing 7.7
let createManyFilesIgnore() =
    ignore(writeTextToDisk "The quick brown fox jumped over the lazy dog")
    ignore(writeTextToDisk "The quick brown fox jumped over the lazy dog")
    writeTextToDisk "The quick brown fox jumped over the lazy dog"

// Listing 7.8
let now = System.DateTime.UtcNow.TimeOfDay.TotalHours

if now < 12.0 then Console.WriteLine "It's morning"
elif now < 18.0 then Console.WriteLine "It's afternoon"
elif now < 20.0 then ignore(5 + 5)
else () 