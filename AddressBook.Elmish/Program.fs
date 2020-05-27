// Learn more about F# at http://fsharp.org

open System
open Elmish
open Elmish.WPF
open AddressBook.Elmish.Views

type Model =
    { Greeting: string }

let init =
    { Greeting = "Hello from VM!" }

type Msg =
    | DoNothing

let update msg m =
    m

let bindings () : Binding<Model, Msg> list =  [
    "Greeting" |> Binding.oneWay (fun m -> m.Greeting)
]

[<EntryPoint>]
[<STAThread>]
let main argv =
    Program.mkSimpleWpf (fun _ -> init) update bindings
    |> Program.runWindow(MainWindow())
