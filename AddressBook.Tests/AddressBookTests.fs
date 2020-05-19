namespace AddressBook.Tests

open Expecto

module AddressBookTests =
    open AddressBook.Core
    open Person

    [<Tests>]
    let contactTests =
        testList "Contact" [
              // I dont think theres any difference between testCase and test - just different block syntax 
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

              testCase "Email must be valid" <| fun _ ->
                  let validationError = getError <| Person.create "Peach" "The Princess" 24 "peach"
                  Expect.equal validationError "Invalid E-mail address" "Email address should be valid"
        ]


    open AddressBook.Core.AddressBook

    [<Tests>]
    let addressBookTests =
        testList "AddressBook" [
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

                let folder = fun book p -> addToAddressBook book p
                let book = List.fold folder AddressBook.AddressBook.Empty [ peach; luigi; mario ]

                Expect.sequenceContainsOrder book [ mario; luigi; peach ] "Insertion order"
                Expect.sequenceContainsOrder (sort book Ascending) [ luigi; mario; peach ] "Ascending order"
                Expect.sequenceContainsOrder (sort book Descending) [ peach; mario; luigi ] "Descending order"
        ]
