// Learn more about F# at http://fsharp.org

open System
open AddressBook

[<EntryPoint>]
let main argv =
    let mutable (addressBook: AddressBook) = []
    printf "Enter Name: "
    let nameString = Console.ReadLine ()
    let contact = PersonalContact { Name = nameString }
    addressBook <- contact :: addressBook
    
    printfn "...Listing all contacts..."
    addressBook
    |> List.iter (function
        | PersonalContact c -> printfn "Contact Name: %s" c.Name )
    0 // return an integer exit code
