namespace PaintShop

open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Linq

module DataParser =
    let parse (lineSource : seq<string>) = 
        let normalizeString (line: string) = 
            line.Trim().ToUpper()

        let getLine (line: string) =
            let splitted = line.Split ' ' 
            if splitted.Length % 2 <> 0 then failwith ErrorMessage.incorrectPreference 
            splitted

        let validateAndGetNumberOfProducts (line: string) =
            match Int32.TryParse(line) with
            | true, n -> n
            | _ -> failwith ErrorMessage.incorrectNumberOfProducts
                
        let createData (clientId: CustomerId) (line: array<string>) =
            [| for i in [0..2..line.Count() - 1] do 
                let colourId = line.[i] |> int |> ColourId
                let colourType = line.[i + 1] |> Types.getColourType
                yield ColourPreference.Create colourId colourType clientId |]
        
        let lines = lineSource |> Seq.toList
        let numberOfProduct = validateAndGetNumberOfProducts <| List.head lines
        (numberOfProduct, [| 
            for i, x in Seq.zip (Seq.initInfinite (fun x -> x + 1)) (List.tail lines) do
            yield! normalizeString x |> getLine |> createData (CustomerId i)
        |] |> Seq.sortBy (fun x -> x.Client, x.Colour))