// Change this to where your packages folder is located!
#I @"C:\users\isaac\.nuget\packages\"

#r @"Humanizer.Core\2.1.0\lib\netstandard1.0\Humanizer.dll"
#r @"Newtonsoft.Json\9.0.1\lib\netstandard1.0\Newtonsoft.Json.dll"
#load "Library1.fs"

open Humanizer
"ScriptsAreAGreatWayToExplorePackages".Humanize(LetterCasing.AllCaps)

Library1.getPerson()
