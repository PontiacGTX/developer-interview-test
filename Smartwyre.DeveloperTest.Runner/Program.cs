using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {

        // --- Setup and Configuration (The DI part) ---
        var serviceProvider = new ServiceCollection()
            // Register the RebateService and its dependencies
            .AddSingleton<RebateDataStore>()
            .AddSingleton<ProductDataStore>()
 
            .AddSingleton<IRebateService, RebateService>()
            .BuildServiceProvider();

        // --- Execution ---
        Console.WriteLine("--- Rebate Calculation Runner ---");

        // 1. Gather Input
        Console.Write("Enter Rebate Identifier: ");
        string rebateId = Console.ReadLine();
        Console.Write("Enter Product Identifier: ");
        string productId = Console.ReadLine();
        Console.Write("Enter Volume: ");
        decimal volume = decimal.Parse(Console.ReadLine());

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateId,
            ProductIdentifier = productId,
            Volume = volume
        };

        // 2. Resolve Service
        var rebateService = serviceProvider.GetService<IRebateService>();

        // 3. Execute Calculation
        RebateResult result = rebateService.Calculate(request);

        // 4. Display Result
        Console.WriteLine("\n--- Calculation Result ---");
        Console.WriteLine($"Success: {result.Success}");
        Console.WriteLine($"Calculated Amount: {result.Amount:C}");
        Console.WriteLine("--------------------------");
    }
}
