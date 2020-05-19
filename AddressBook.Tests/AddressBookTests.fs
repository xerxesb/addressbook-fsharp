module Tests

open Expecto

open AddressBook.Core
open Person

let getError (r:Result<_, _>) =
  match r with
  | Error x -> x
  | Ok x -> failwith "Cannot unwrap Ok for Result expected to be an error"

let getContact (r:Result<Person.Contact, _>) =
  match r with 
    | Ok x -> x
    | Error _ -> failwith "Cannot unwrap Error for Result expected to be Ok"

let getPerson (r:Result<Person.Contact, _>) =
  match getContact r with
    | PersonalContact p -> p

[<Tests>]
let contactTests =
  testList "Contact" [
    testCase "Can create contact" <| fun _ ->
      let peach = getPerson <| Person.create "Peach" "The Princess" 24 "peach@nintendo.com"
      
      Expect.equal peach.FirstName "Peach" "First name should be set"
      Expect.equal peach.LastName "The Princess" "Last name should be set"
      Expect.equal peach.Age 24 "The age should be set"
        
    test "Object equality" {
      let peach1 = getPerson <| Person.create "Peach" "The Princess" 24 "peach@nintendo.com"
      let peach2 = getPerson <| Person.create "Peach" "The Princess" 24 "peach@nintendo.com"
      let luigi = getPerson <| Person.create "Luigi" "The Brother" 25 "luigi@nintendo.com"
      
      Expect.equal peach1 peach2 "Two instances of the same type should have object equality"
      Expect.notEqual peach1 luigi "Two different instances should be different" 
    }

    test "Print contact" {
      let peach = getPerson <| Person.create "Peach" "The Princess" 24 "peach@nintendo.com"
      let actualOutput = printContact peach
      let expectedOutput = "Contact Name: Peach The Princess\nContact Age: 24\nE-mail: peach@nintendo.com"
      
      Expect.equal actualOutput expectedOutput "Printed string should match intended format"
    }

    test "Email must be valid" {
      let validationError = getError <| Person.create "Peach" "The Princess" 24 "peach"
      Expect.equal validationError "Invalid E-mail address" "Email address should be valid"
    }
  ]


open AddressBook.Core.AddressBook

[<Tests>]
let addressBookTests =
  let flip f a b = 
    f b a

  testList "AddressBook"  [
    testCase "Can add to addressbook" <| fun _ ->
      let peach = getContact <| create "Peach" "The Princess" 24 "peach@nintendo.com"
      let luigi = getContact <| create "Luigi" "The Brother" 25 "luigi@nintendo.com"

      let book = 
        AddressBook.Empty
        |> flip addToAddressBook peach
        |> flip addToAddressBook luigi

      Expect.contains book peach "Peach should be in the book"
      Expect.contains book luigi "Luigi should be in the book"

    testCase "Can sort addressbook" <| fun _ ->
      let peach = getContact <| create "Peach" "The Princess" 24 "peach@nintendo.com"
      let luigi = getContact <| create "Luigi" "The Brother" 25 "luigi@nintendo.com"
      let mario = getContact <| create "Mario" "The Plumber" 26 "mario@nintendo.com"

      let book = 
        AddressBook.Empty
        |> (flip addToAddressBook) peach
        |> (flip addToAddressBook) luigi
        |> (flip addToAddressBook) mario
      
      Expect.sequenceContainsOrder book [mario ; luigi ; peach] "Insertion order"
      Expect.sequenceContainsOrder (sort book Ascending) [luigi ; mario ; peach] "Ascending order"
      Expect.sequenceContainsOrder (sort book Descending) [peach; mario ; luigi] "Descending order"
  ]
