module Hello.Person.RandomPerson

open FSharp.Data
type RandomUser = JsonProvider<"sample.json">

let getName () =
    let users = RandomUser.Load("https://randomuser.me/api/")

    users.Results
    |> Array.map(fun r -> sprintf "%s %s" r.Name.First r.Name.Last)
    |> Array.tryHead