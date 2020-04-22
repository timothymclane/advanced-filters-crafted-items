open Fake.IO.Globbing
#r "paket:
nuget Fake.Core.Target
nuget Fake.IO.Zip //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators

// Properties
let buildDir = "./build/"
let releaseDir = "./releases/"

// Targets
Target.create "Clean" (fun _ ->
  Directory.ensure buildDir
  Shell.cleanDir buildDir
)

Target.create "Copy" (fun _ -> 
  !! "**/*.lua"
  ++ "LICENSE"
  ++ "**/*.txt"
  ++ "**/*.xml"
  ++ "**/*.md"
  -- "**.fake/**/*"
  |> GlobbingPattern.setBaseDir "./"
  |> Shell.copyFilesWithSubFolder "build/AdvancedFilters_CraftedItems/"
)

Target.create "Deploy" (fun p ->
  Directory.ensure releaseDir
  (sprintf "%s/advanced-filters-crafted-items-%s.zip" releaseDir p.Context.Arguments.Head, !! "build/AdvancedFilters_CraftedItems/**")
  ||> Zip.zip "build/"
  Shell.cleanDir buildDir
)

Target.create "Default" ignore

// Dependencies
open Fake.Core.TargetOperators

"Clean"
  ==> "Copy"
  ==> "Default"
  ==> "Deploy"

// start build
Target.runOrDefaultWithArguments "Default"