// Learn more about F# at http://fsharp.org

open System
open Elmish
open Elmish.WPF
open AddressBook.Elmish.Views

type Model =
    { Text: string }

let init =
    { Text = "hello" }

type Msg =
    | DoNothing

let update msg m =
    m

let bindings () : Binding<Model, Msg> list =  []

[<EntryPoint>]
[<STAThread>]
let main argv =
    Program.mkSimpleWpf (fun _ -> init) update bindings
    |> Program.runWindow(MainWindow())
