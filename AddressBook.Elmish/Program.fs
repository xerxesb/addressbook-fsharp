open System
open AddressBook.Core.Person
open Elmish
open Elmish.WPF
open AddressBook.Core
open AddressBook.Elmish.Views

type Model =
    { Book : Contact list
      SelectedId: int option }

let init =
    { Book = []
      SelectedId = None }

type Msg =
    | LoadBook
    | Select of int option

let update msg m =
    match msg with
    | LoadBook -> { m with Book = AddressBook.Core.Persistence.fetchAllAddresses () }
    | Select x -> System.Diagnostics.Trace.WriteLine <| sprintf "Selected model with id %A" x
                  { m with SelectedId = x }

let bindings () : Binding<Model, Msg> list =  [
    "LoadBook" |> Binding.cmd LoadBook
    "Addresses" |> Binding.subModelSeq(
          (fun m -> m.Book),
          (fun e -> (Contact.getPerson e).Id),
          (fun () -> [
                "Id" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).Id)
                "FirstName" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).FirstName)
                "LastName" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).LastName)
                "Age" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).Age)
                "Email" |> Binding.oneWay (fun (_, e) -> (Contact.getPerson e).Email)
          ])
          
      )
    // The api docs say not to use this method and instead use binding.twoway, but i couldnt get it to work
    // It spits out binding errors to the trace
//    "SelectedPerson" |> Binding.twoWay ((fun m -> m.SelectedId), (fun v -> Select v))
    "SelectedPerson" |> Binding.subModelSelectedItem("Addresses", (fun m -> m.SelectedId), Select)
]

[<EntryPoint>]
[<STAThread>]
let main argv =
    Program.mkSimpleWpf (fun _ -> init) update bindings
    |> Program.withConsoleTrace
    |> Program.runWindowWithConfig
        { ElmConfig.Default with LogConsole = true; Measure = true }
        (MainWindow())
