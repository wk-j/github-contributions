open System
open FSharp.Control.Tasks
open AngleSharp
open System.Globalization

type Rect = { 
    Count: int
    Date: DateTime 
    Fill: string
}

let symbols = [
    "❔"
    "🥤"
    "☕️"
    "🍺"
    "🥂"
    "◻️"
    "📒"
    "📙"
    "📗"
    "📕"
    "📘"
    "🔰"
    "❔"
    "💛"
    "🧡"
    "❤️"
    "💙"
]
let toCode = function
    | "#ebedf0" -> symbols.[0]
    | "#c6e48b" -> symbols.[1]
    | "#7bc96f" -> symbols.[2]
    | "#239a3b" -> symbols.[3]
    | "#196127" -> symbols.[4]
    | _ -> "?"

let loadSvgData user =
    let createRect (x: Dom.IElement) = 
        let en = CultureInfo("en-US")
        let date = x.GetAttribute "data-date"
        let count = x.GetAttribute "data-count"
        let fill = x.GetAttribute "fill"

        { Count = Int32.Parse count
          Date = DateTime.ParseExact(date, "yyyy-MM-dd", en)
          Fill = fill }

    task { 
        let address = sprintf "https://github.com/users/%s/contributions" user
        let config = Configuration.Default.WithDefaultLoader()
        let! document = BrowsingContext.New(config).OpenAsync(address) 
        return 
            document.QuerySelectorAll("rect") 
            |> Seq.map createRect 
    }

[<EntryPoint>]
let main argv =
    task {
        let! svg = loadSvgData argv.[0]
        let days = 
            svg |> Seq.groupBy (fun x -> x.Date.DayOfWeek)
            |> Seq.toList

        let show (day, rects) = 
            let code = rects |> Seq.map (fun x -> toCode x.Fill) 
            printfn "%10s  %s" (day.ToString()) (String.Join(" ", code))

        days |> List.map show |> ignore
    } 
    |> Async.AwaitTask
    |> Async.RunSynchronously
    0 