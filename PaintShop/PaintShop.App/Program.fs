namespace PaintShop

open System.Linq
open System.Collections.Generic

module Program =
    [<EntryPoint>]
    let main argv = 

        if argv.Length = 0 then
            failwith ErrorMessage.file

        try 
            let data = DataParser.parse (System.IO.File.ReadLines argv.[0])
            let processedPreferences = Shop.solve (snd data) (fst data)
            if Seq.length processedPreferences <> fst (data) then failwith ErrorMessage.noProductMatch
            processedPreferences |> Seq.iter (Types.getColourTypeString >> printf "%s ")
        with
        | ex -> printfn "%s" ex.Message
        
        System.Console.ReadLine () |> ignore
        0
