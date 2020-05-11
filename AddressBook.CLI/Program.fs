// Learn more about F# at http://fsharp.org

open System
open AddressBook
open Person
open SortAddressBook

module Menu =
    type MenuSelection =
        | CreateNew
        | ListAll
        | SortAddressBook
        | ExitApp
    type MenuChoice = MenuSelection option
       
    let private validateChoice (key:ConsoleKeyInfo) =
        match (string key.KeyChar).ToUpperInvariant() with
        | "C" -> Some CreateNew
        | "L" -> Some ListAll
        | "S" -> Some SortAddressBook
        | "X" -> Some ExitApp
        | _ -> None
        
    let private printMenu () =
        printf "Menu:
1. (C)reate a new person in the address book
2. (L)ist all people in the address book
3. (S)ort the address book
4. E(x)it the program

Enter your selection: "
    
    let getMenuOption () =
        printMenu ()
        let key = Console.ReadKey ()
        printfn "\n"
        validateChoice key


module CreateContactWorkflow =
    
    // Should this really be here, or should this be a function on Person?
    // This is input validation, not domain validation, so it seems to make sense to be here...
    // IRRESPECTIVE: if the input age is invalid (e.g. they enter a string) then i think we need to handle
    // it better than this implementation in execute
    let parseAge (ageAsString:string) =
        match Int32.TryParse ageAsString with
        | false, _ -> Error "An invalid age was entered"
        | true, age -> Ok age
        
    let execute addressBook onComplete =
        printf "Enter First Name: " 
        let firstName = Console.ReadLine ()
        printf "Enter Last Name: "
        let lastName = Console.ReadLine ()
        printf "Enter Age: "
        let ageResult = Console.ReadLine () |> parseAge 
        
        match ageResult with
            | Ok age -> match create firstName lastName age with
                            | Ok c ->
                                AddressBook.addToAddressBook addressBook c
                                |> onComplete
                            | Error m ->
                                printfn "%s" m
                                onComplete addressBook
            | Error m ->
                printfn "%s" m
                onComplete addressBook


module ListAllContactsWorkflow =
    let private allEntries addressBook =
        addressBook
        |> List.map (function
            | PersonalContact c -> printContact c)
        |> String.concat "\n"
    
    let execute addressBook onComplete =
        match List.length addressBook with
            | 0 -> printfn "There are no entries in the address book\n"
            | _ -> printfn "All entries in the address book:\n%s\n" <| allEntries addressBook
        onComplete ()

module SortContactsWorkflow =
    let private validateChoice (choice:ConsoleKeyInfo) =
        match (string choice.KeyChar).ToUpperInvariant () with
            | "A" -> Some Ascending
            | "D" -> Some Descending
            | _ -> None
        
    let execute addressBook onComplete =
        printf "What order? (A)scending or (D)escending? "
        match Console.ReadKey () |> validateChoice with
            | Some Ascending -> SortAddressBook.sort addressBook Ascending
            | Some Descending -> SortAddressBook.sort addressBook Descending
            | None -> addressBook
        |> onComplete
        

[<EntryPoint>]
let main _ =
    let rec programLoop addressBook exitCondition =
        let startAgain = (fun () -> programLoop addressBook exitCondition)
        let updateAndStartAgain = (fun newBook -> programLoop newBook exitCondition)
        
        if exitCondition then () else
            let selection = Menu.getMenuOption ()
            match selection with
                | None -> programLoop addressBook exitCondition
                | Some Menu.ListAll -> ListAllContactsWorkflow.execute addressBook startAgain
                | Some Menu.SortAddressBook -> SortContactsWorkflow.execute addressBook updateAndStartAgain
                | Some Menu.CreateNew -> CreateContactWorkflow.execute addressBook updateAndStartAgain
                | Some Menu.ExitApp -> ()
        ()
        
    let (addressBook: AddressBook.AddressBook) = []
    programLoop addressBook false
    
    0 // return an integer exit code
