// Listing 33.1
#I @"..\..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
open FSharp.Data

type Package = HtmlProvider< @"..\..\data\sample-package.html">

// Listing 33.1
let getDownloadsForPackage packageName =
    let package = Package.Load(sprintf "https://www.nuget.org/packages/%s" packageName)
    package.Tables.``Version History``.Rows
    |> Seq.sumBy(fun p -> p.Downloads)

// Listing 33.2
let getPackage packageName =
    packageName |> sprintf "https://www.nuget.org/packages/%s" |> Package.Load
let getDetailsForVersion versionText packageName =
    let package = getPackage packageName
    package.Tables.``Version History``.Rows |> Seq.tryFind(fun p -> p.Version.Contains versionText)


// Listing 33.3
(* You should comment out / delete the lines above before uncommenting these ones - otherwise, as the names
of the declared symbols clash, you'll get errors in the IDE. *)

// let getPackage =
//     sprintf "https://www.nuget.org/packages/%s" >> Package.Load
// let getVersionsForPackage (package:Package) =
//     package.Tables.``Version History``.Rows
// let loadPackageVersions = getPackage >> getVersionsForPackage

// let getDownloadsForPackage =
//     loadPackageVersions >> Seq.sumBy(fun p -> p.Downloads)

// let getDetailsForVersion versionText =
//     loadPackageVersions >> Seq.tryFind(fun p -> p.Version.Contains versionText)

// Now you try step 10
// let getDetailsForCurrentVersion = getDetailsForVersion "(this version)"

// Listing 33.4

open System

type PackageVersion =
    | CurrentVersion
    | Prerelease
    | Old
type VersionDetails =
    { Version : Version
      Downloads : decimal
      PackageVersion : PackageVersion
      LastUpdated : DateTime }
type NuGetPackage =
    { PackageName : string
      Versions : VersionDetails list }

// Listing 33.5
let parse (versionText:string) =
    let getVersionPart (version:string) isCurrent =
        match version.Split '-', isCurrent with
        | [| version; _ |], true
        | [| version |], true -> Version.Parse version, CurrentVersion
        | [| version; _ |], false -> Version.Parse version, Prerelease
        | [| version |], false -> Version.Parse version, Old
        | _ -> failwith "unknown version format"

    let parts = versionText.Split ' ' |> Seq.toList |> List.rev
    match parts with
    | [] -> failwith "Must be at least two elements to a version"
    | "version)" :: "(this" :: version :: _ -> getVersionPart version true
    | version :: _ -> getVersionPart version false

let enrich (versionHistory:Package.VersionHistory.Row seq) = 
    { PackageName =
        match versionHistory |> Seq.map(fun row -> row.Version.Split ' ' |> Array.toList |> List.rev) |> Seq.head with
        | "version)" :: "(this" :: _ :: name | _ :: name -> List.rev name |> String.concat " "
        | _ -> failwith "Unable to parse version name"
      Versions =
        versionHistory 
        |> Seq.map(fun versionHistory ->
            let version, packageVersion = parse versionHistory.Version
            { Version = version
              Downloads = versionHistory.Downloads
              LastUpdated = versionHistory.``Last updated``
              PackageVersion = packageVersion })
        |> Seq.toList }

// Listing 33.6
(* You should comment out / delete the clashing function definitions from Listing 33.3 in order to remove
any errors in the IDE *)

// let loadPackageVersions = getPackage >> getVersionsForPackage >> enrich >> (fun p -> p.Versions)
// let getDetailsForVersion version = loadPackageVersions >> Seq.find(fun p -> p.Version = version)
// let getDetailsForCurrentVersion = loadPackageVersions >> Seq.find(fun p -> p.PackageVersion = CurrentVersion)

// "Newtonsoft.Json" |> getDetailsForVersion (Version.Parse "9.0.1")

// let getDownloadsForPackage = loadPackageVersions >> Seq.sumBy(fun p -> p.Downloads)
