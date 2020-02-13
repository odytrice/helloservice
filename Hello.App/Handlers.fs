module Hello.App.Handlers

open Giraffe
open Giraffe.GiraffeViewEngine
open FSharp.Control.Tasks.V2.ContextInsensitive
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration

// ---------------------------------
// Web app
// ---------------------------------

type Model = { Name : string; Location : string; }


let view (model: Model) =
    html [] [
        head [] [
            title [] [ str "Hello Microservices" ]
            link [ _href "main.css"; _rel "stylesheet" ]
        ]
        body [] [
            div [_class "container"] [
                p [ _class "greetings" ] [
                    str "Hello "
                    strong [] [ str model.Name ]
                    str " from "
                    strong [] [ str model.Location ]
                ]
            ]

        ]
    ]

let indexHandler next (ctx: HttpContext) =
    task {
        let config = ctx.GetService<IConfiguration>() |> Config.From
        let! name = Services.getName(config)
        let! location = Services.getLocation(config)
        let model = { Name = name; Location = location }
        return! htmlView (view model) next ctx
    }


let webApp: HttpHandler =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]