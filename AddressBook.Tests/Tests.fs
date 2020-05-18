module Tests

open Xunit
open AddressBook
open Person

let getContact (r:Result<Person.Contact, _>) =
    match r with
    | Ok x -> match x with 
            | PersonalContact c -> c
    | Error _ -> failwith "Cannot retrieve Error for expected Result"

[<Fact>]
let ``Can create person`` () =
   let peach = Person.create "Peach" "The Princess" 24
   let luigi = Person.create "Luigi" "The Brother" 25

   let peachContact = getContact peach
   Assert.Equal("Peach", peachContact.FirstName)



