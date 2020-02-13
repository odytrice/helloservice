module Hello.App.Services
open Microsoft.Extensions.DependencyInjection
open FSharp.Data

let getName (config: Config) =
    async {
        let! response = Http.AsyncRequestString config.PersonUrl
        return response
    }

let getLocation(config: Config) =
    async {
        let! response = Http.AsyncRequestString config.LocationUrl
        return response
    }
