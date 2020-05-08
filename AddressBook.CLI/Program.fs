// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open AddressBook
open Contact
open Person

[<DebuggerStepThrough>]
let doWhile f c =
    f()
    while c() = false do
        f()

module Menu =
    type MenuSelection =
        | CreateNew
        | ListAll
        | ExitApp
    type MenuChoice = MenuSelection option
       
    let private validateKeyChoice (key:ConsoleKeyInfo) =
        match (string key.KeyChar).ToUpperInvariant() with
        | "C" -> Some CreateNew
        | "L" -> Some ListAll
        | "X" -> Some ExitApp
        | _ -> None
        
    let private printMenu () =
        printfn "What would you like to do:
1. (C)reate a new person in the address book
2. (L)ist all people in the address book
3. E(x)it the program"
    
    let getMenuOption () =
        printMenu ()
        let key = Console.ReadKey ()
        printfn "\n"
        validateKeyChoice key

module CreateContactWorkflow =
    let execute (addressBook: AddressBook.AddressBook) updateFunc =
        printf "Enter First Name: "
        let firstNameString = Console.ReadLine ()
        printf "Enter Last Name: "
        let lastNameString = Console.ReadLine ()
        let contact = PersonalContact <| create firstNameString lastNameString
        updateFunc(contact :: addressBook)
        
module ListAllContactsWorkflow =
    let execute (addressBook: AddressBook.AddressBook) =
        printfn "All entries in the address book:"
        addressBook
        |> List.iter (function
            | PersonalContact c -> printfn "Contact Name: %s %s" c.FirstName c.LastName
           )
        printfn ""

module ExitAppWorkflow =
    let execute f =
        f() |> ignore

[<EntryPoint>]
let main argv =
    
    let mutable (addressBook: AddressBook.AddressBook) = []
    let mutable exitCondition = false
    doWhile (fun () ->
        let selection = Menu.getMenuOption ()
        match selection with
            | None -> ()
            | Some Menu.CreateNew -> CreateContactWorkflow.execute addressBook (fun newBook -> addressBook <- newBook)
            | Some Menu.ListAll -> ListAllContactsWorkflow.execute addressBook
            | Some Menu.ExitApp -> ExitAppWorkflow.execute (fun () -> exitCondition <- true)
    ) (fun () -> exitCondition)
    0 // return an integer exit code
