namespace AddressBook

//type PersonalContact =
//    | Name of string
    
type Person = {
    Name: string
}
    
type Contact =
    | PersonalContact of Person
    
type AddressBook = Contact list

module Test =
    let test1 =
        let luigi = { Name = "Luigi" }
        let mario = { Name = "Mario" }
        let contact1 = PersonalContact luigi
        let contact2 = PersonalContact mario
        let addressBook = [contact1; contact2]
        
        addressBook
        |> List.iter (function
            | PersonalContact c -> printfn "Name: %s" c.Name)
