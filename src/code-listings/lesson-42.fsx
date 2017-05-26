#r @"..\..\packages\Selenium.WebDriver\lib\net40\WebDriver.dll"
#r @"..\..\packages\canopy\lib\canopy.dll"
open canopy

chromeDir <- "drivers"

// Now you try #1
start chrome
url "https://www.manning.com/books/get-programming-with-f-sharp"

first "#Submit" |> click
click ".cart-button"
click ".btn-primary"

"#email" << "Fred.Smith@fakemail.com"
elements ".btn-primary" |> Seq.find(fun e -> e.Text = "checkout as a guest") |> click

"#country" << "United States"
"#firstName" << "Fred"
"#lastName" << "Smith"
"#company" << "Super F# Developers Ltd."
"#address1" << "23 The Street"
"#address2" << "The Town"
"#city" << "The City"
"#USStateSelector" << "CA"
"#zip" << "90210"
"#addressPhone" << "0800 123 456"

quit()

// Listing 42.1
once (fun _ -> start chrome)
before (fun _ -> url "https://www.manning.com/books/get-programming-with-f-sharp")
lastly (fun _ -> quit())

// Listing 42.2
context "Sample Tests"
reporter <- reporters.LiveHtmlReporter(BrowserStartMode.Chrome, "drivers") :> reporters.IReporter

test (fun _ -> "#chapter_id_1" == "LESSON 1 THE VISUAL STUDIO EXPERIENCE")
"49 lessons in total" &&& fun _ -> count ".sect1" 49
"There's a web programming unit" &&& fun _ ->  ".sect0" *= "UNIT 8: WEB PROGRAMMING"

run()

