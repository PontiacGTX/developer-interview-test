# Smartwyre Developer Test Instructions

You have been selected to complete our candidate coding exercise. Please follow the directions in this readme.

Clone, **DO NOT FORK**, this repository to your account on the online Git resource of your choosing (GitHub, BitBucket, GitLab, etc.). Your solution should retain previous commit history and you should utilize best practices for committing your changes to the repository.

You are welcome to use whatever tools you normally would when coding — including documentation, libraries, frameworks, or AI tools (such as ChatGPT or Copilot).

However, it is important that you fully understand your solution. As part of the interview process, we will review your code with you in detail. You should be able to:

- Explain the design choices you made.
- Walk us through how your solution works.
- Make modifications or extensions to your code during the review.

Please note: if your submission appears to have been generated entirely by an AI agent or another third party, without your own understanding or contribution, it will not meet our evaluation criteria.

# The Exercise

In the 'RebateService.cs' file you will find a method for calculating a rebate. At a high level the steps for calculating a rebate are:

 1. Lookup the rebate that the request is being made against.
 2. Lookup the product that the request is being made against.
 2. Check that the rebate and request are valid to calculate the incentive type rebate.
 3. Store the rebate calculation.

What we'd like you to do is refactor the code with the following things in mind:

 - Adherence to SOLID principles
 - Testability
 - Readability
 - Currently there are 3 known incentive types. In the future the business will want to add many more incentive types. Your solution should make it easy for developers to add new incentive types in the future.

We’d also like you to 
 - Add some unit tests to the Smartwyre.DeveloperTest.Tests project to show how you would test the code that you’ve produced 
 - Run the RebateService from the Smartwyre.DeveloperTest.Runner console application accepting inputs (either via command line arguments or via prompts is fine)

The only specific "rules" are:

- The solution must build
- All tests must pass

You are free to use any frameworks/NuGet packages that you see fit. You should plan to spend around 1 hour completing the exercise.

Feel free to use code comments to describe your changes. You are also welcome to update this readme with any important details for us to consider.

Once you have completed the exercise either ensure your repository is available publicly or contact the hiring manager to set up a private share.

//Design Choices

Encapsulation and Structure
I ensured a clean, high-level workflow by separating the core business logic from the main sequence of operations. This means the main Calculate method is kept clean, focusing only on the steps: retrieve data, delegate calculation, and store the result. I achieved this separation by defining dedicated public methods for each incentive type, such as GetResultFixedCashAmount. This structure facilitates Single Responsibility Principle (SRP) compliance, as each helper method is solely responsible for its specific incentive type's business rules and mathematics.

Robustness and Validation
The design incorporates defensive programming and the Fail-Fast principle. I placed all necessary validation—checking for null inputs, zero values, and product support—at the beginning of the helper methods (GetResult...). This ensures that if the input is invalid, the function immediately returns a failed result, preventing unnecessary computation and improving performance. Additionally, using the null-conditional operator (rebate?.Incentive switch) in GetRebateResult helps guard against potential NullReferenceException crashes if data retrieval fails.

Testability of Logic
By making the core calculation methods public, I deliberately enabled direct unit testing of the business rules. This design choice isolates the critical logic, allowing for thorough verification of the mathematical accuracy and failure conditions without requiring a complex mocking setup for the data access layer, ensuring the core calculations are proven reliable.
