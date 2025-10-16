using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly RebateService _service = new RebateService();
    private readonly Product _supportedProduct = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
    private readonly Rebate _validRebate = new Rebate { Amount = 5.00m };
    private readonly CalculateRebateRequest _validRequest = new CalculateRebateRequest { Volume = 10 };
    [Fact]
    public void GetResultFixedCashAmount_ShouldSucceed_WhenValid()
    {
        // Arrange
        var rebate = new Rebate { Amount = 10.00m, Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        // Act
        RebateResult result = _service.GetResultFixedCashAmount(rebate, product);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(10.00m, result.Amount);
    }

    [Fact]
    public void GetResultFixedCashAmount_ShouldFail_WhenProductDoesNotSupportIncentive()
    {
        // Arrange
        var rebate = new Rebate { Amount = 10.00m, Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };

        // Act
        RebateResult result = _service.GetResultFixedCashAmount(rebate, product);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void GetResultFixedCashAmount_ShouldFail_WhenRebateAmountIsZero()
    {
        // Arrange
        var rebate = new Rebate { Amount = 0m, Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        // Act
        RebateResult result = _service.GetResultFixedCashAmount(rebate, product);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void GetResultAmountPerUom_ShouldFail_WhenRebateIsNull()
    {
        // Act
        RebateResult result = _service.GetResultAmountPerUom(null, _supportedProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void GetResultAmountPerUom_ShouldFail_WhenProductIsNull()
    {
        // Act
        RebateResult result = _service.GetResultAmountPerUom(_validRebate, null, _validRequest);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void GetResultAmountPerUom_ShouldFail_WhenIncentiveIsNotSupported()
    {
        // Arrange
        var unsupportedProduct = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        // Act
        RebateResult result = _service.GetResultAmountPerUom(_validRebate, unsupportedProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
    }

    // The zero amount/volume tests were covered previously but are critical:
    [Fact]
    public void GetResultAmountPerUom_ShouldFail_WhenRebateAmountIsZero()
    {
        // Arrange
        var zeroAmountRebate = new Rebate { Amount = 0m };

        // Act
        RebateResult result = _service.GetResultAmountPerUom(zeroAmountRebate, _supportedProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
    }

    // --- Success Test ---

    [Fact]
    public void GetResultFixedRateRebate_ShouldCalculateCorrectly_WhenValid()
    {
        // --- ARRANGE ---
        // Override the fields from the common setup to match the requirements 
        // of a Fixed Rate Rebate calculation (Price, Percentage, FixedRateRebate support).

        // 1. Rebate needs a Percentage value and the correct Incentive Type (though not checked in helper method signature)
        var fixedRateRebate = new Rebate { Percentage = 0.10m };

        // 2. Product needs a Price and must support FixedRateRebate
        var fixedRateProduct = new Product
        {
            Price = 20.00m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };

        // 3. Request needs a Volume
        var fixedRateRequest = new CalculateRebateRequest { Volume = 5 };

        // The expected calculation: Price * Percentage * Volume
        const decimal ExpectedAmount = 20.00m * 0.10m * 5; // Result: 10.00m

        // --- ACT ---
        RebateResult result = _service.GetResultFixedRateRebate(
            fixedRateRebate,
            fixedRateProduct,
            fixedRateRequest
        );

        // --- ASSERT ---
        Assert.True(result.Success);
        Assert.Equal(ExpectedAmount, result.Amount);
    }

    // --- Null Input Failure Tests ---

    [Fact]
    public void GetResultFixedRateRebate_ShouldFail_WhenRebateIsNull()
    {
        // Act
        RebateResult result = _service.GetResultFixedRateRebate(null, _supportedProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void GetResultFixedRateRebate_ShouldFail_WhenProductIsNull()
    {
        // Act
        RebateResult result = _service.GetResultFixedRateRebate(_validRebate, null, _validRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    // --- Zero Input Failure Tests ---

    [Fact]
    public void GetResultFixedRateRebate_ShouldFail_WhenRebatePercentageIsZero()
    {
        // Arrange
        var zeroPercentageRebate = new Rebate { Percentage = 0m };

        // Act
        RebateResult result = _service.GetResultFixedRateRebate(zeroPercentageRebate, _supportedProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void GetResultFixedRateRebate_ShouldFail_WhenProductPriceIsZero()
    {
        // Arrange
        var zeroPriceProduct = new Product { Price = 0m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };

        // Act
        RebateResult result = _service.GetResultFixedRateRebate(_validRebate, zeroPriceProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void GetResultFixedRateRebate_ShouldFail_WhenRequestVolumeIsZero()
    {
        // Arrange
        var zeroVolumeRequest = new CalculateRebateRequest { Volume = 0 };

        // Act
        RebateResult result = _service.GetResultFixedRateRebate(_validRebate, _supportedProduct, zeroVolumeRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }

    // --- Incentive Support Failure Test ---

    [Fact]
    public void GetResultFixedRateRebate_ShouldFail_WhenIncentiveIsNotSupported()
    {
        // Arrange
        var unsupportedProduct = new Product
        {
            Price = 20.00m,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount // Only supports a different type
        };

        // Act
        RebateResult result = _service.GetResultFixedRateRebate(_validRebate, unsupportedProduct, _validRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0m, result.Amount);
    }
}
