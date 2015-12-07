module FileReadTests

open NUnit
open NUnit.Framework
open PaintShop
open Should
open System

[<Test>]
[<ExpectedException(typedefof<Exception>, ExpectedMessage="File not formatted correctly: Number of products is incorrect")>]
let ``Number of product must be an integer``() = 
    let data = ["AAA"]
    DataParser.parse data |> ignore

[<Test>]
[<ExpectedException(typedefof<Exception>, ExpectedMessage="The file is not formatted correctly: Incorrect preference")>]
let ``Client preferences must be formatted as "Colour ColourType"``() = 
    let data = ["5"; "1 G 1"]
    DataParser.parse data |> ignore

[<Test>]
let ``Correct combination``() = 
    let data = ["5"; "1 G"]
    let expectedProducts = 5
    let expectedPreferences = [| ColourPreference.Create (ColourId 1) Gloss (CustomerId 1) |]
    DataParser.parse data |> fun x -> 
        fst(x).ShouldEqual(expectedProducts)
        snd(x).ShouldEqual(expectedPreferences)

[<Test>]
let ``Unordered combination must be ordered``() = 
    let data = ["2"; "2 G 1 M"]
    let expectedProducts = 2
    let expectedPreferences = [| ColourPreference.Create (ColourId 1) Matte (CustomerId 1); ColourPreference.Create (ColourId 2) Gloss (CustomerId 1) |]
    DataParser.parse data |> fun x -> 
        fst(x).ShouldEqual(expectedProducts)
        snd(x).ShouldEqual(expectedPreferences)