module Tests

open Expecto
open AddressBook
open Person

let getContact (r:Result<Person.Contact, _>) =
  match r with 
    | Ok x -> x
    | Error _ -> failwith "Cannot unwrap Error for Result"

let getPerson (r:Result<Person.Contact, _>) =
  match getContact r with
    | PersonalContact p -> p

[<Tests>]
let contactTests =
  testList "Contact" [
    testCase "Can create contact" <| fun _ ->
      let peach = getPerson <| Person.create "Peach" "The Princess" 24
      
      Expect.equal peach.FirstName "Peach" "First name should be set"
      Expect.equal peach.LastName "The Princess" "Last name should be set"
      Expect.equal peach.Age 24 "The age should be set"
        
    test "Object equality" {
      let peach1 = getPerson <| Person.create "Peach" "The Princess" 24
      let peach2 = getPerson <| Person.create "Peach" "The Princess" 24
      let luigi = getPerson <| Person.create "Luigi" "The Brother" 25
      
      Expect.equal peach1 peach2 "Two instances of the same type should have object equality"
      Expect.notEqual peach1 luigi "Two different instances should be different" 
    }
  ]

open AddressBook.AddressBook

[<Tests>]
let addressBookTests =
  let flip f a b = 
    f b a

  testList "AddressBook"  [
    testCase "Can add to addressbook" <| fun _ ->
      let peach = getContact <| create "Peach" "The Princess" 24
      let luigi = getContact <| create "Luigi" "The Brother" 25

      let book = 
        AddressBook.Empty
        |> flip addToAddressBook peach
        |> flip addToAddressBook luigi

      Expect.contains book peach "Peach should be in the book"
      Expect.contains book luigi "Luigi should be in the book"

    testCase "Can sort addressbook" <| fun _ ->
      let peach = getContact <| create "Peach" "The Princess" 24
      let luigi = getContact <| create "Luigi" "The Brother" 25
      let mario = getContact <| create "Mario" "The Plumber" 26

      let book = 
        AddressBook.Empty
        |> (flip addToAddressBook) peach
        |> (flip addToAddressBook) luigi
        |> (flip addToAddressBook) mario
      
      Expect.sequenceContainsOrder book [mario ; luigi ; peach] "Insertion order"
      Expect.sequenceContainsOrder (sort book Ascending) [luigi ; mario ; peach] "Ascending order"
      Expect.sequenceContainsOrder (sort book Descending) [peach; mario ; luigi] "Descending order"
  ]
