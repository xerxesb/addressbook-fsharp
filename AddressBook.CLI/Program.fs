// Learn more about F# at http://fsharp.org

open System
open AddressBook
open Contact
open Person

let doWhile f c =
    f()
    while c() = false do
        f()

[<EntryPoint>]
let main argv =
    let mutable (addressBook: AddressBook.AddressBook) = []
    let mutable exitCondition = false
    doWhile (fun () ->
        printf "Enter First Name: "
        let firstNameString = Console.ReadLine ()
        
        printf "Enter Last Name: "
        let lastNameString = Console.ReadLine ()
        
        let contact = PersonalContact <| create firstNameString lastNameString
        addressBook <- contact :: addressBook
        exitCondition <- firstNameString = "EXIT"
    ) (fun () -> exitCondition)
    
    printfn "...Listing all contacts..."
    addressBook
    |> List.iter (function
        | PersonalContact c -> printfn "Contact Name: %s" c.LastName )
    0 // return an integer exit code
