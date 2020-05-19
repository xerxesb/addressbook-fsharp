namespace AddressBook.Core

[<AutoOpen>]
module GenericHelpers =
    let flip f a b =
        f b a

[<AutoOpen>]
module TestHelpers =
    open AddressBook.Core
    open Person

    let getError (r: Result<_, _>) =
        match r with
        | Error x -> x
        | Ok x -> failwith "Cannot unwrap Ok for Result expected to be an error"

    let getContact (r: Result<Person.Contact, _>) =
        match r with
        | Ok x -> x
        | Error _ -> failwith "Cannot unwrap Error for Result expected to be Ok"

    let getPerson (r: Result<Person.Contact, _>) =
        match getContact r with
        | PersonalContact p -> p
