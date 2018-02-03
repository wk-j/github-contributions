#r "../packages/AngleSharp/lib/net45/AngleSharp.dll"
#load "../paket-files/rspeele/TaskBuilder.fs/TaskBuilder.fs"

open AngleSharp
open FSharp.Control.Tasks
open System.Linq
let address = "https://github.com/users/wk-j/contributions"
let config = Configuration.Default.WithDefaultLoader()
let rs = 
    task { 
        let! document = BrowsingContext.New(config).OpenAsync(address) 
        let svg = document.QuerySelectorAll("svg")
        return svg
    }

printfn "%A" rs