module Tests

open Expecto
open AddressBook
open Person

let getResult (c:Result<Person.Contact, _>) =
    match c with 
      | Ok x -> match x with
                | PersonalContact c -> c
      | Error _ -> failwith "Cannot unwrap Error for Result"

[<Tests>]
let tests =
  testList "Contact" [
    testCase "Can create contact" <| fun _ ->
        let peach = getResult <| Person.create "Peach" "The Princess" 24
        
        Expect.equal peach.FirstName "Peach" "First name should be set"
        Expect.equal peach.LastName "The Princess" "Last name should be set"
        Expect.equal peach.Age 24 "The age should be set"
        
    test "Object equality" {
        let peach1 = getResult <| Person.create "Peach" "The Princess" 24
        let peach2 = getResult <| Person.create "Peach" "The Princess" 24
        let luigi = getResult <| Person.create "Luigi" "The Brother" 25
        
        Expect.equal peach1 peach2 "Two instances of the same type should have object equality"
        Expect.notEqual peach1 luigi "Two different instances should be different" 
    }



  ]
