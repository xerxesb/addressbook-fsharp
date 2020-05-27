namespace AddressBook.Core

module Person =
    let private (>>=) a f = Result.bind f a

    // Look at making this type private so it can't be constructed
    // https://stackoverflow.com/questions/13925361/is-it-possible-to-enforce-that-a-record-respects-some-invariants/13925632#13925632
    //
    // Also want to explore having stronger types (e.g. an Age type that can't be negative)
    type Person = {
        FirstName: string
        LastName: string
        Age: int
        Email: string
    }
    
    type Contact =
        | PersonalContact of Person
        
    module Contact =
        let getPerson = function
            | PersonalContact x -> x
        
    let printContact c =
        [
         sprintf "Contact Name: %s %s" c.FirstName c.LastName 
         sprintf "Contact Age: %i" c.Age
         sprintf "E-mail: %s" c.Email
        ] |> String.concat "\n"
        
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
        | x when x = 6 -> Error (sprintf "We dont accept people with 6 letter names [%s]" name) // This is an example of a Match Expression
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
        
    let validateAge age =
        match age with
        | x when x <= 0 -> Error (sprintf "Age must be greater than 0")
        | _ -> Ok age

    let validateEmail (email:string) = 
        match email.Contains "@" with
        | true -> Ok email
        | false -> Error "Invalid E-mail address"
    
    let validateInput firstName lastName age email =
        let firstName = validateFirstName firstName
        let lastName = validateLastName lastName
        let age = validateAge age
        let email = validateEmail email
        
        let contact = firstName |> Result.bind (fun fname ->
            lastName |> Result.bind (fun lname ->
                age |> Result.bind (fun age ->
                    email |> Result.bind (fun e ->
                        Ok <| PersonalContact {
                                  FirstName = fname
                                  LastName = lname
                                  Age = age
                                  Email = e
                              }
                    )
                )
            )
        )
        
        // Something smarter can be done here than this...
        // Need to learn about Kleisli (fish) operator?
        // ROP also should have a better answer
        // https://fsharpforfunandprofit.com/posts/recipe-part2/
        //
        // DT also suggested using Computation Expressions
        // https://fsharpforfunandprofit.com/posts/computation-expressions-wrapper-types/#another-example
        // e.g.
        // let! n = validateName name
        // let! a = validateAge age
        // Person n a

        let err (o:Result<_, _>) =
            match o with
            | Error x -> Some x
            | Ok _ -> None

        let errors = [err firstName; err lastName; err age; err email] 
                     |> List.choose id
        if errors.Length = 0
        then match contact with
                | Ok c -> Ok c
                | Error _ -> Error (String.concat "\n" errors)
        else Error (String.concat "\n" errors)
    
    // Validation here (and in helper methods above) is an example of Error Handling
    // when doing Railway Oriented Programming
    // https://medium.com/@kai.ito/test-post-3df1cf093edd
    let create firstName lastName age email =
        validateInput firstName lastName age email
    
    
module AddressBook =
    open Person
    
    type AddressBook = Contact list
    
    type SortOrder =
        | Ascending
        | Descending
        // This technique allows you to attach functions to DUs. Unclear when you would exactly want it.
        // This here as an example of how to do it even if its not the best use case for a DU member function
        with        
            member x.sortAddresses addressBook =
                match x with
                    | Ascending -> List.sort addressBook
                    | Descending -> List.sortDescending addressBook
                
    let addToAddressBook book person =
        person :: book
    
    let sort addressBook (order: SortOrder) =
        order.sortAddresses addressBook
        
