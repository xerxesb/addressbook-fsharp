namespace AddressBook

//type PersonalContact =
//    | Name of string
    
module Person =
    type Person = {
        FirstName: string
        LastName: string
    }
    
    let create name age =
        { FirstName = name
          LastName = age }
        
    
module Contact =
    open Person
    
    type Contact =
        | PersonalContact of Person
    
module AddressBook =
    open Contact
    
    type AddressBook = Contact list
    
    let addToAddressBook book person =
        person :: book
    

module Test =
    open Person
    open Contact
    let test1 =
        let peach = PersonalContact <| create "Peach" "The Princess"
        let luigi = PersonalContact <| create "Luigi" "The Brother"
        let mario = PersonalContact <| create "Mario" "The Plumber"
        let addressBook = [peach; mario; luigi]
        
        addressBook
        |> List.iter (function
            | PersonalContact c -> printfn "Contact Name: %s %s" c.FirstName c.LastName)
