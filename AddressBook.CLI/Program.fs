// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open AddressBook
open Contact
open Person

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
    let execute (addressBook: AddressBook.AddressBook) onComplete =
        printf "Enter First Name: " 
        let firstNameString = Console.ReadLine ()
        printf "Enter Last Name: "
        let lastNameString = Console.ReadLine ()
        
        PersonalContact <| create firstNameString lastNameString
            |> AddressBook.addToAddressBook addressBook
            |> onComplete


module ListAllContactsWorkflow =
    let execute (addressBook: AddressBook.AddressBook) onComplete =
        printfn "All entries in the address book:"
        addressBook
        |> List.map (function
            | PersonalContact c -> printContact c)
        |> String.concat "\n"
        |> printfn "%s\n"
        onComplete()


module ExitAppWorkflow =
    let execute onComplete =
        onComplete() |> ignore


[<EntryPoint>]
let main argv =
    
    let rec programLoop addressBook exitCondition =
        if exitCondition then () else
            let selection = Menu.getMenuOption ()
            match selection with
                | None -> programLoop addressBook exitCondition
                | Some Menu.CreateNew -> CreateContactWorkflow.execute addressBook (fun newBook -> programLoop newBook exitCondition)
                | Some Menu.ListAll -> ListAllContactsWorkflow.execute addressBook (fun () -> programLoop addressBook exitCondition)
                | Some Menu.ExitApp -> ExitAppWorkflow.execute (fun () -> programLoop addressBook true)
        ()
        
    let (addressBook: AddressBook.AddressBook) = []
    programLoop addressBook false
    
    0 // return an integer exit code
