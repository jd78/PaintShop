namespace PaintShop
 
open System.Collections.Generic
open System.Linq
open System
 
module Shop =
    
    let solve (data: seq<ColourPreference>) (nColours: int) =
        
        let colours = Dictionary<ColourId, ColourType> ()
        let satisfied = HashSet<CustomerId> ()

        let customersOnePreference = 
            data 
            |> Seq.groupBy (fun x -> x.Client) 
            |> Seq.map (fun (k, v) -> k, Seq.length v) 
            |> Seq.where (fun t -> snd(t) = 1)
            |> Seq.map fst 
            |> fun x -> HashSet<CustomerId> x

        Solver.resolveColoursCustomerWithOnePreference customersOnePreference data colours satisfied

        let remainingCustomers = 
            data 
            |> Seq.filter (fun x -> not (customersOnePreference.Contains x.Client))
            |> Seq.groupBy (fun x -> x.Client)
            |> Seq.toList
            |> fun l -> l.ToDictionary (fst, snd)
        
        let isCustomerSatisfied (xs: seq<ColourPreference>) =
            xs |> Seq.exists (fun p -> colours.ContainsKey p.Colour && colours.[p.Colour] = p.ColourType)
        
        if colours.Count > 0 then 
            remainingCustomers
            |> Seq.filter (fun x -> isCustomerSatisfied x.Value)
            |> Seq.iter (fun x -> satisfied.Add x.Key |> ignore)

        let allClientsAreHappy =
            let l = data |> Seq.distinctBy (fun p -> p.Client) |> Seq.length
            satisfied.Count = l
        
        let fillColours () =
            for i = 1 to nColours do
                if not (colours.ContainsKey (ColourId i)) then
                    colours.Add (ColourId i, Gloss)

        if allClientsAreHappy then
            fillColours ()
        else 
            match Solver.resolveColoursByPermutations nColours colours remainingCustomers with
            | Some solution ->  solution
                                |> Seq.iter (fun s -> 
                                    if not (colours.ContainsKey (fst s)) then colours.Add s)
            | None -> failwith ErrorMessage.impossibleCombination

        colours
        |> Seq.sortBy(fun (KeyValue(k, _)) -> k)
        |> Seq.map (fun x -> x.Value)
        