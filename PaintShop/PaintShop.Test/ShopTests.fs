module ShopTests
 
open NUnit
open NUnit.Framework
open PaintShop
open Should
open System
 
 [<Test>]
let ``First``() =
    let data = [
        ColourPreference.Create (ColourId 1) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 3)
    ]
 
    let expected = [ Gloss; Matte; Matte; ]
 
    Shop.solve data 3 |> fun result -> result.ShouldEqual(expected) |> ignore

[<Test>]
[<ExpectedException(typedefof<Exception>, ExpectedMessage=ErrorMessage.impossibleCombination)>]
let ``Impossible combination: not possible to satisfy all the clients as we can create only one batch for each colour``() =
    let data = [
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 1) Matte (CustomerId 2)
    ]
    Shop.solve data 1 |> ignore
 
[<Test>]
[<ExpectedException(typedefof<Exception>, ExpectedMessage=ErrorMessage.impossibleCombination)>]
let ``Impossible combination: not possible to satisfy all the clients, additional test``() =
    let data = [
        ColourPreference.Create (ColourId 1) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 3)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 4)
    ]
    Shop.solve data 3 |> ignore

[<Test>]
let ``Three clients test``() =
    let data = [
        ColourPreference.Create (ColourId 1) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 2)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 4) Gloss (CustomerId 2)
        ColourPreference.Create (ColourId 5) Matte (CustomerId 3)
    ]
 
    let expected = [ Gloss; Gloss; Gloss; Gloss; Matte ]
 
    Shop.solve data 5 |> fun result -> result.ShouldEqual(expected) |> ignore
 
[<Test>]
let ``14 Customers test``() =
    let data = [
        ColourPreference.Create (ColourId 2) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 2)
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 3)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 4)
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 4)
        ColourPreference.Create (ColourId 4) Matte (CustomerId 4)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 5)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 6)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 7)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 7)
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 7)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 8)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 9)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 10)
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 10)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 11)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 12)
        ColourPreference.Create (ColourId 4) Matte (CustomerId 13)
        ColourPreference.Create (ColourId 5) Gloss (CustomerId 14)
        ColourPreference.Create (ColourId 4) Matte (CustomerId 14)
    ]
 
    let expected = [ Gloss; Matte; Gloss; Matte; Gloss ]
 
    Shop.solve data 5 |> fun result -> result.ShouldEqual(expected) |> ignore
 
[<Test>]
let ``Customer satisfaction computation``() =
    let data = [
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 1) Matte (CustomerId 2)
    ]
 
    let expected = [ Matte; Matte; ]
 
    Shop.solve data 2 |> fun result -> result.ShouldEqual(expected) |> ignore
 
[<Test>]
let ``A scenario that involves the quorum``() =
    let data = [
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 1) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 1) Matte (CustomerId 3)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 3)
    ]
 
    let expected = [ Matte; Matte; ]
 
    Shop.solve data 2 |> fun result -> result.ShouldEqual(expected) |> ignore
 
[<Test>]
let ``A client that likes only one type``() =
    let data = [
        ColourPreference.Create (ColourId 1) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 1) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 2)
    ]
 
    let expected = [ Gloss; Gloss; Matte; ]
 
    Shop.solve data 3 |> fun result -> result.ShouldEqual(expected) |> ignore
 
[<Test>]
let ``2 clients that have only one colour type preference``() =
    let data = [
        ColourPreference.Create (ColourId 1) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 1) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 2) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 2)
    ]

    let expected = [ Gloss; Gloss; Matte; ]
 
    Shop.solve data 3 |> fun result -> result.ShouldEqual(expected) |> ignore

[<Test>]
let ``3 clients test that involves quorum, satisfaction and swappig``() =
    let data = [
        ColourPreference.Create (ColourId 1) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 3) Gloss (CustomerId 1)
        ColourPreference.Create (ColourId 5) Matte (CustomerId 1)
        ColourPreference.Create (ColourId 2) Gloss (CustomerId 2)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 2)
        ColourPreference.Create (ColourId 4) Gloss (CustomerId 2)
        ColourPreference.Create (ColourId 5) Matte (CustomerId 3)
        ColourPreference.Create (ColourId 3) Matte (CustomerId 3)
    ]

    let expected = [ Gloss; Gloss; Gloss; Gloss; Matte]
 
    Shop.solve data 5 |> fun result -> result.ShouldEqual(expected) |> ignore