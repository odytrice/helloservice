module Hello.Location.Handlers

open Giraffe
open FSharp.Control.Tasks.V2.ContextInsensitive
open Microsoft.Extensions.Logging

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler: HttpHandler =
    fun next ctx ->
        task {
            let result = RandomLocation.getLocation()
            let logger = ctx.GetLogger("Location Handler")

            match result with
            | Some location ->
                logger.LogInformation("Location: "+ location)
                return! text location next ctx
            | None -> return! RequestErrors.NOT_FOUND "Not Found" next ctx
        }

let webApp: HttpHandler =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]