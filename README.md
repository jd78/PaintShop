PAINT SHOP CODING EXERCISE
===============================

You run a paint shop, and there are a few different colors of paint you can prepare.  Each color can be either "gloss" or "matte".

You have a number of customers, and each have some colors they like, either gloss or matte.  No customer will like more than one color in matte.

You want to mix the colors, so that:
   * There is just one batch for each color, and it's either gloss or matte.
   * For each customer, there is at least one color they like.
   * You make as few mattes as possible (because they are more expensive).

Your program should accept an input file as a command line argument, and print a result to standard out.  An example input file is:

5
1 M 3 G 5 G
2 G 3 M 4 G
5 M

The first line specifies how many colors there are.

Each subsequent line describes a customer.  For example, the first customer likes color 1 in matte, color 3 in gloss and color 5 in gloss.

Your program should read an input file like this, and print out either that it is impossible to satisfy all the customer, or describe, for each of the colors, whether it should be made gloss or matte.

The output for the above file should be:

G G G G M

...because all customers can be made happy by every paint being prepared as gloss except number 5.

An example of a file with no solution is:

1
1 G
1 M

Your program should print

No solution exists

A slightly richer example is:

5
2 M
5 G
1 G
5 G 1 G 4 M
3 G
5 G
3 G 5 G 1 G
3 G
2 M
5 G 1 G
2 M
5 G
4 M
5 G 4 M

...which should print:

G M G M G

One more example.  The input:

2
1 G 2 M
1 M

...should produce

M M


Project structure
--------------------

The solution has been implemented in F#, the code is based on problem reduction and permutations.

- Gilt.Shop.App
	It's the main application

- Gilt.Shop.Tests
	It's the unit test projects

Gilt.Shop.App
-------------------

The entry point is Program.fs. The application will take a filepath\filename as input, the project has been configured to run in debug using the provided testFile.txt.
The application is precompiled, the binaries are in .\bin\debug\Gilt.Shop.App.exe
It can be run from command line as .\bin\debug\Gilt.Shop.App.exe filename.txt
Two tests file are included in the bin folder: testFile.txt and gilt-testfile.txt

Dataparser.fs
_______________

DataCreator is responsible to build the data that is needed by the shop application to do the computation.
The method createData gets in input the result of the file read passed as a Sequence of string. It normalize and validate the data and, if they are correct, it will create the table structure used by the Shop module.

Permutation.fs
_______________

Contains the logic to generate all the possible combination that might solve the problem

Solver.fs
_______________

Contains the logic to try to solve the problem.
The method "resolveColoursCustomerWithOnePreference" tries to aggregate the data for the customer satisfaction for the clients that have only one preference.
The method "resolveColoursByPermutations" is the final solver, if all the previous are failed. It tries to solve the problem using the generated permutations starting solving applying the best (eg GGGG) to the worst (eg MMMM).
If the clients are not satisfied for that permutation, the next permutation will be applied. 
During the elaboration, if one of the client is not happy with the generated permutations, the algorithm goes to the next one. This is a quite important step thinking about the performance.


Shop.fs
_______________

It's the core of the project. The processData function takes in input a Sequence of processData ColourPreference and the number of products.
ColorPreference is a table, created by the DataCreator, that contains colourId, colourType and customerId.
The table format has been used to simplify the data grouping and computation.

The steps of the algorithm are:

- Checking the clients that have only one preferences. In this step we can calculate the impossible combinations, and reduce the problem. A client that has only one preference generates a color resolver.
- If all clients are happy with the partial generated solution at the step 1, apply the solver filling the remaining slot with a Gloss colour.
- If not every client is happy, apply the permutation resolver and:
	- If there is a solution that satisfy all the client, apply it.
	- If not, the problem has no solution

Gilt.Shop.Tests
==================
The project contains the unit tests for both Shop and DataCreator module.
To run the test you need to install NUnit console runner and run the exec in the bin\debug folder. It's also possible to run the tests from visual studio using NUnitTestAdapter extension.


Something about improvements and scalability
============================================

The permutation solver can be easily implemented to run concurrently as a single set of permutation can be checked and applied without having race conditions.
To do that, we need to run the permutation solver in multithread and notify when one finds a solution that satisfy all the clients where the preference is G over M.
