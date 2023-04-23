module Server

open System
open System.IO
open Suave
open Filters
open Operators


[<EntryPoint>]
let main argv =

    let app: WebPart =
         choose [
              GET >=> path "/" >=> Files.browseFileHome "index.html"    
              GET >=> path "/test" >=> Successful.OK "Biden666"
              GET >=> Files.browseHome
              RequestErrors.NOT_FOUND "Page not found." 
              POST >=> path "/hello" >=> Successful.OK "Hello POST"]


    let portEnvVar = Environment.GetEnvironmentVariable "PORT"
    let port = if String.IsNullOrEmpty portEnvVar then 8080 else (int)portEnvVar
    let config = {
        defaultConfig with
            bindings = [HttpBinding.createSimple HTTP "127.0.0.1" port]
            homeFolder = Some(Path.GetFullPath "./Public") }
    startWebServer config app
    0



