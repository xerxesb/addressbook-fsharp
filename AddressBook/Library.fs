namespace AddressBook

type Person = {
    FirstName: string
    LastName: string
}

type Contact =
    | PersonalContact of Person

type AddressBook = Contact list

type SortOrder =
    | Ascending
    | Descending
    
    
module Person =
    let create firstName lastName = {
          FirstName = firstName
          LastName = lastName
    }
        
    
module Contact =
    let printContact c =
        sprintf "Contact Name: %s %s" c.FirstName c.LastName
    

module SortAddressBook =
    let sort (addressBook:AddressBook) (order: SortOrder) =
        match order with
            | Ascending -> List.sort addressBook
            | Descending -> List.sortDescending addressBook
        
    
module AddressBook =
    let addToAddressBook book person =
        person :: book
    

module Test =
    let test1 =
        let peach = PersonalContact <| Person.create "Peach" "The Princess"
        let luigi = PersonalContact <| Person.create "Luigi" "The Brother"
        let mario = PersonalContact <| Person.create "Mario" "The Plumber"
        let addressBook = [peach; mario; luigi]
        
        addressBook
        |> List.iter (function
            | PersonalContact c -> printfn "Contact Name: %s %s" c.FirstName c.LastName)
