open System
open AddressBook.Core.Person
open Elmish
open Elmish.WPF
open AddressBook.Core
open AddressBook.Elmish.Views

type Model =
    { Book : Contact list }

let init =
    { Book = [] }

type Msg =
    | LoadBook

let update msg m =
    match msg with
    | LoadBook -> { m with Book = AddressBook.Core.Persistence.fetchAllAddresses }

let bindings () : Binding<Model, Msg> list =  [
    "LoadBook" |> Binding.cmd LoadBook
    "Addresses" |> Binding.subModelSeq(
          (fun m -> m.Book),
          (fun e -> (Contact.getPerson e).FirstName),
          (fun () -> [
                "FirstName" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).FirstName)
                "LastName" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).LastName)
                "Age" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).Age)
                "Email" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).Email)
          ])
      )
]

[<EntryPoint>]
[<STAThread>]
let main argv =
    Program.mkSimpleWpf (fun _ -> init) update bindings
    |> Program.withConsoleTrace
    |> Program.runWindowWithConfig
        { ElmConfig.Default with LogConsole = true; Measure = true }
        (MainWindow())
