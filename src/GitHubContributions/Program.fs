// Learn more about F# at http://fsharp.org

open System
open FSharp.Control.Tasks
open AngleSharp
open System.Threading.Tasks
open System.Linq
open System
open System.Globalization

type Rect = { 
    Count: int
    Date: DateTime 
    Fill: string
}
let loadSvgData user =
    let createRect (x: Dom.IElement) = 
        let en = CultureInfo("en-US")
        let date = x.GetAttribute("data-date")
        let count = x.GetAttribute("data-count")
        let fill = x.GetAttribute("fill")

        { Count = Int32.Parse count
          Date = DateTime.ParseExact(date, "yyyy-MM-dd", en)
          Fill = fill }

    task { 
        let address = sprintf "https://github.com/users/%s/contributions" user
        let config = Configuration.Default.WithDefaultLoader()
        let! document = BrowsingContext.New(config).OpenAsync(address) 
        let svg = document.QuerySelectorAll("rect") |> Seq.map createRect
        return svg
    }

[<EntryPoint>]
let main argv =
    task {
        let! rs = loadSvgData "wk-j"
        printfn "%A" rs
    } 
    |> Async.AwaitTask
    |> Async.RunSynchronously
    
    0 // return an integer exit code
