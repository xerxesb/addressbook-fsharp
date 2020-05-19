namespace AddressBook.Core

open AddressBook.Core

module Persistence =
    open AddressBook
    open Person
    
    let fetchAllAddresses =
        System.Threading.Thread.Sleep 4000
        let p1 = getContact <| create "FirstN 1" "LastN 1" 11 "test1@email.com"
        let p2 = getContact <| create "FirstN 2" "LastN 2" 22 "test2@email.com"
        let p3 = getContact <| create "FirstN 3" "LastN 3" 33 "test3@email.com"
        let p4 = getContact <| create "FirstN 4" "LastN 4" 44 "test4@email.com"
        let p5 = getContact <| create "FirstN 5" "LastN 5" 55 "test5@email.com"
        
        let list = [ p1 ; p2 ; p3 ; p4 ; p5 ]
        let folder = fun book p -> addToAddressBook book p
        List.fold folder AddressBook.AddressBook.Empty list 
    ()

