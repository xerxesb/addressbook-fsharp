namespace AddressBook.Tests

open Expecto

module PersistenceTests =
    open AddressBook.Core
    open Person
    open Persistence
    open System
    
    [<Tests>]
    let addressBookTests =
        testList "Address Book" [
            testCase "fetching all addresses" <| fun _ ->
                let book = fetchAllAddresses ()
                Expect.hasLength book 5 "Hard coded to 5 elements in the list"
                
                // Check one element for sanity
                let (PersonalContact p) = book.Head
                Expect.equal p.Age 55 "Age should be the hardcoded value of the last entry"

            testCase "simulating a database connection that takes time" <| fun _ ->
                let startTime = DateTime.Now 
                fetchAllAddresses () |> ignore
                let endTime = DateTime.Now
                let diff = endTime - startTime

                Expect.isGreaterThan (int diff.TotalSeconds) 3 "Should have taken 3 seconds"
        ]
        
