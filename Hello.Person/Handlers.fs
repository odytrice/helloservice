module Hello.Person.Handlers

open Giraffe
open FSharp
open FSharp.Control.Tasks.V2.ContextInsensitive
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (next:HttpFunc) (ctx: HttpContext) =
    task {
        let logger = ctx.GetLogger("Person Handler")
        let result = RandomPerson.getName()
        match result with
        | Some name ->
            logger.LogInformation("Person: "+ name)
            return! text name next ctx
        | None -> return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }


let webApp: HttpHandler =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]