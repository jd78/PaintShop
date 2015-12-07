namespace PaintShop

type ColourType = 
    | Gloss
    | Matte

type CustomerId = CustomerId of int
type ColourId = ColourId of int

type ColourPreference = { Colour: ColourId; ColourType: ColourType; Client: CustomerId } with
    static member Create colour colourType client = { Colour = colour; ColourType = colourType; Client = client }

module Types =
    let getColourTypeString x =
        match x with
        | Gloss -> "G"
        | Matte -> "M"

    let getColourType x =
        match x with
        | "G" -> Gloss
        | "M" -> Matte
        | _ -> failwith ErrorMessage.incorrectColourType