open System
open AddressBook.Core.Person
open Elmish
open Elmish.WPF
open AddressBook.Core
open AddressBook.Elmish.Views

type Model =
    { book : Contact list }

let init =
    { book = [] }

type Msg =
    | LoadBook

let update msg m =
    match msg with
    | LoadBook -> { m with book = AddressBook.Core.Persistence.fetchAllAddresses }

let bindings () : Binding<Model, Msg> list =  [
    "GreetingMessage" |> Binding.oneWay (fun m -> m.book)
    "Greet" |> Binding.cmd LoadBook
]

[<EntryPoint>]
[<STAThread>]
let main argv =
    Program.mkSimpleWpf (fun _ -> init) update bindings
    |> Program.withConsoleTrace
    |> Program.runWindow(MainWindow())
