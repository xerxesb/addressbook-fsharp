// Learn more about F# at http://fsharp.org

open System
open AddressBook

let doWhile f c =
    f()
    while c() = false do
        f()

[<EntryPoint>]
let main argv =
    let mutable (addressBook: AddressBook) = []
    let mutable exitCondition = false
    doWhile (fun () ->
        printf "Enter Name: "
        let nameString = Console.ReadLine ()
        let contact = PersonalContact { Name = nameString }
        addressBook <- contact :: addressBook
        exitCondition <- nameString = "EXIT"
    ) (fun () -> exitCondition)
    
    printfn "...Listing all contacts..."
    addressBook
    |> List.iter (function
        | PersonalContact c -> printfn "Contact Name: %s" c.Name )
    0 // return an integer exit code
