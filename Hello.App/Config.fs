namespace Hello.App

open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging

type Config =
    {
        PersonUrl: string
        LocationUrl: string
    }

    static member From(configuration: IConfiguration) =
        {
            PersonUrl = configuration.GetValue("Services:Person")
            LocationUrl = configuration.GetValue("Services:Location")
        }

    member this.Print() =
        printfn "Person API: %s" this.PersonUrl
        printfn "Location API: %s" this.LocationUrl