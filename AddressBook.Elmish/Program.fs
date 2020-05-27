open System
open Elmish
open Elmish.WPF
open AddressBook.Elmish.Views

type Model =
    { Greeting: string }

let init =
    { Greeting = "Hello from VM!" }

type Msg =
    | Greet

let update msg m =
    match msg with
    | Greet -> { m with Greeting = "You pressed the button!" }

let bindings () : Binding<Model, Msg> list =  [
    "GreetingMessage" |> Binding.oneWay (fun m -> m.Greeting)
    "Greet" |> Binding.cmd Greet
]

[<EntryPoint>]
[<STAThread>]
let main argv =
    Program.mkSimpleWpf (fun _ -> init) update bindings
    |> Program.withConsoleTrace
    |> Program.runWindow(MainWindow())
