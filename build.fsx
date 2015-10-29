// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System.IO
open System

RestorePackages()

// Properties
let buildDir = "./build/"

// Filesets
let sqlDbProjRef = 
  !! "./**/*.sqlproj"

let appReferences  = 
    !! "./**/*.fsproj" 

// SQL Properties
let server = "\"(localdb)\\v11.0\""
let scriptPath = buildDir </> "TestDb_Create.sql"
let sqlcmdOutputPath = buildDir </> "TestDb_Create.out"
 
// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir]
)

Target "BuildSqlProj" (fun _ ->
    sqlDbProjRef
      |> MSBuildRelease buildDir "Build"
      |> Log "Database-Build-Output: "
)

let runSqlcmd server input output = 
    let result = ExecProcess (fun info -> 
        info.FileName <- "SQLCMD.EXE"
        info.Arguments <- "-S " + server + " -i " + input + " -o " + output) (TimeSpan.FromMinutes 5.0)

    if result <> 0 then failwithf "SQLCMD.EXE returned with a non-zero exit code"

    log (File.ReadAllText(output))

Target "RunCreateScript" (fun _ ->
    runSqlcmd server scriptPath sqlcmdOutputPath
)

Target "BuildApp" (fun _ ->
    appReferences
      |> MSBuildRelease buildDir "Build"
      |> Log "Build-Output: "
)

// Dependencies
"Clean"
  ==> "BuildSqlProj"
  ==> "RunCreateScript"
  ==> "BuildApp"

// start build
RunTargetOrDefault "BuildApp"