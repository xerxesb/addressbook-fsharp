namespace AddressBook

module Person =
    type Person = {
        FirstName: string
        LastName: string
    }
    
    type Contact =
        | PersonalContact of Person
        
    let printContact c =
        sprintf "Contact Name: %s %s" c.FirstName c.LastName
        
    let isNotBlank (name:string) =
        match name with
        | "" -> Error "Can't have empty name"
        | _ -> Ok name
        
    let hasTitleCase (name:string) =
        if (string name.[0]).ToUpperInvariant() <> (string name.[0])
        then Error "Name does not have Title Case"
        else Ok name
    
    let meetsSomeAribtraryLengthCriteria (name:string) =
        match name.Length with
        | 1 | 2 -> Error "Name is too short"
        | x when x = 6 -> Error "We dont accept people with 6 letter names"
        | _ -> Ok name
        
    let validateFirstName name =
        name
        |> isNotBlank
        |> Result.bind hasTitleCase
    
    let validateLastName name =
        name
        |> isNotBlank
        |> Result.bind hasTitleCase
        |> Result.bind meetsSomeAribtraryLengthCriteria
    
    let validateName firstName lastName =
        validateFirstName firstName
        |> Result.bind validateLastName
    
    let create firstName lastName =
        let validationResult = validateName firstName lastName
        match validationResult with
            | Error m -> Error m
            | Ok _ -> Ok (PersonalContact <| {
                    FirstName = firstName
                    LastName = lastName
                })
    
    
module AddressBook =
    open Person
    
    type AddressBook = Contact list

    let addToAddressBook book person =
        person :: book
    

module SortAddressBook =
    open AddressBook
    
    type SortOrder =
        | Ascending
        | Descending
    
    let sort (addressBook:AddressBook) (order: SortOrder) =
        match order with
            | Ascending -> List.sort addressBook
            | Descending -> List.sortDescending addressBook
        
    

module Test =
    open Person
    let test1 =
        let peach = Person.create "Peach" "The Princess"
        let luigi = Person.create "Luigi" "The Brother"
        let mario = Person.create "Mario" "The Plumber"
        
        let addressBook = [peach; mario; luigi]
        
        let printFunc = (fun (PersonalContact contact) ->
            printfn "Contact Name: %s %s" contact.FirstName contact.LastName
            Ok contact
        )
        let raisedFunc = Result.bind printFunc
        
        addressBook
            |> List.iter (fun c -> raisedFunc c |> ignore)
        
