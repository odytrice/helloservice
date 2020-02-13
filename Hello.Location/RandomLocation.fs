module Hello.Location.RandomLocation

open FSharp.Data
type RandomUser = JsonProvider<"sample.json">

let getLocation() =
    let users = RandomUser.Load("https://randomuser.me/api/")

    users.Results
    |> Array.map(fun r -> sprintf "%s %s, %s" r.Location.City r.Location.State r.Location.Country)
    |> Array.tryHead