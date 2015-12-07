namespace PaintShop

module ErrorMessage =
    [<Literal>]
    let impossibleCombination = "No solution exists"
    [<Literal>]
    let noProductMatch = "The number of products dones't match with the final elaboration"

    [<Literal>]
    let incorrectPreference = "The file is not formatted correctly: Incorrect preference"
    [<Literal>]
    let incorrectNumberOfProducts = "File not formatted correctly: Number of products is incorrect"

    [<Literal>]
    let file = "Filepath/filename is missing"

    [<Literal>]
    let incorrectColourType = "Incorrect colour type"