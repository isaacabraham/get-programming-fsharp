#I @"..\packages\"
#r @"Humanizer.Core.2.1.0\lib\netstandard1.0\Humanizer.dll"
#r @"Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll"
#load "Library1.fs"

//#load @"Scripts\load-project-debug.fsx"

open Humanizer
"ScriptsAreAGreatWayToExplorePackages".Humanize(LetterCasing.AllCaps)

Library1.getPerson()
