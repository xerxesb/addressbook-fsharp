namespace AddressBook


module Person =
    let private (>>=) a f = Result.bind f a

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
        // TODO: Handle case when name is null
        if (string name.[0]).ToUpperInvariant() <> (string name.[0])
        then Error (sprintf "Name [%s] does not have Title Case" name)
        else Ok name
    
    let meetsSomeArbitraryLengthCriteria (name:string) =
        match name.Length with
        | 1 | 2 -> Error (sprintf "Name [%s] is too short" name)
        | x when x = 6 -> Error (sprintf "We dont accept people with 6 letter names [%s]" name)
        | _ -> Ok name
        
    // Future Xerx is going to look at this and wonder wat?
    let validateFirstName name =
        name
        |> isNotBlank       // This line uses pipelineing to pass Name into isNotBlank
        >>= hasTitleCase    // This line uses Result.bind in the definition of a private bind operator (See top of file)
    
    let validateLastName name =
        name
        |> isNotBlank
        >>= hasTitleCase
        >>= meetsSomeArbitraryLengthCriteria
    
    let validateInput firstName lastName =
        let firstName = validateFirstName firstName
        let lastName = validateLastName lastName
        
        // Something smarter can be done here than this...
        // Need to learn about Kleisli (fish) operator?
        // ROP also should have a better answer
        // https://fsharpforfunandprofit.com/posts/recipe-part2/
        let errors = [firstName; lastName]
                     |> List.map (function
                         | Ok _ -> ""
                         | Error e -> e
                         )
                     |> List.filter (fun x -> x <> "")
        if errors.Length = 0
        then Ok ()
        else Error (String.concat "\n" errors)
    
    // Validation here (and in helper methods above) is an example of Error Handling
    // when doing Railway Oriented Programming
    // https://medium.com/@kai.ito/test-post-3df1cf093edd
    let create firstName lastName =
        let validationResult = validateInput firstName lastName
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
        
