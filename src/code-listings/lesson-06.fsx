// Listing 6.1
let name = "isaac"
name = "kate"

// Listing 6.2
name <- "kate"

// Listing 6.3
let mutable name = "isaac"
name <- "kate"

// Listing 6.4
open System.Windows.Forms
let form = new Form()
form.Show()
form.Width <- 400
form.Height <- 400
form.Text <- "Hello from F#!"

// Listing 6.5
open System.Windows.Forms
let form = new Form(Text = "Hello from F#!", Width = 300, Height = 300)
form.Show()

// Listing 6.6
let mutable petrol = 100.0

let drive(distance) =
    if distance = "far" then petrol <- petrol / 2.0
    elif distance = "medium" then petrol <- petrol - 10.0
    else petrol <- petrol - 1.0

drive("far")
drive("medium")
drive("short")

petrol

// Listing 6.7
let drive(petrol, distance) =
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

let petrol = 100.0
let firstState = drive(petrol, "far")
let secondState = drive(firstState, "medium")
let finalState = drive(secondState, "short")

// Now you try
do
    let drive(petrol, distance) =
        if distance > 50 then petrol / 2.0
        elif distance > 25 then petrol - 10.0
        elif distance > 0 then petrol - 1.
        else petrol

    let petrol = 100.0
    let firstState = drive(petrol, 75)
    let secondState = drive(firstState, 40)
    let finalState = drive(secondState, 10)
    ()
