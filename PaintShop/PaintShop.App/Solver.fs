namespace PaintShop

open System.Collections.Generic
open System.Linq

module Solver =
    module Seq =
        let intersect (ps : seq<'a>) (qs : seq<'a>) = ps.Intersect qs

    module private Private =
        let tryResolvePreference (colourSolution: List<ColourId * ColourType>) (list : list<ColourId * ColourType>) =
            let length = Seq.intersect list colourSolution |> Seq.length
            length <> 0
        
        let clientToPreference (kvp : KeyValuePair<CustomerId, seq<ColourPreference>>) =
            kvp.Value |> Seq.map (fun m -> (m.Colour, m.ColourType)) |> Seq.toList

        let allClientsHappy colourSolution (clients : Dictionary<CustomerId, seq<ColourPreference>>) =
            clients |> Seq.map clientToPreference |> Seq.forall (tryResolvePreference colourSolution)

    let resolveColoursCustomerWithOnePreference (customerOnePreference: seq<CustomerId>) (data: seq<ColourPreference>) 
        (colours: Dictionary<ColourId, ColourType>) (satisfied: HashSet<CustomerId>) =
        let tryResolve i = 
            let x = data |> Seq.find (fun x -> x.Client = i)
            if colours.ContainsKey x.Colour then
                let t = colours.[x.Colour]
                if t <> x.ColourType then failwith ErrorMessage.impossibleCombination
            else colours.Add (x.Colour, x.ColourType) 
            satisfied.Add i |> ignore 

        customerOnePreference |> Seq.iter tryResolve

    let resolveColoursByPermutations (nColours) (colours: Dictionary<ColourId, ColourType>) (customers: Dictionary<CustomerId, seq<ColourPreference>>) =
        let permutations = Permutation.permutations (nColours - colours.Count)
        let colourSolution = List<(ColourId * ColourType)> ()
        let validSolution =
            permutations 
            |> Seq.exists (fun p ->
                colourSolution.Clear ()
                let mutable permutationIndex = 0
                for i = 0 to nColours - 1 do
                    let colourId = ColourId (i+1)
                    if not (colours.ContainsKey colourId) then
                        colourSolution.Add (colourId, p.[permutationIndex])
                        permutationIndex <- permutationIndex + 1
                    else colourSolution.Add (colourId, colours.[colourId])
                customers
                |> (Private.allClientsHappy colourSolution))
        match validSolution with
        | true -> Some colourSolution
        | _ -> None