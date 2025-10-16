using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IRebateService
{
    RebateResult Calculate(CalculateRebateRequest request);
    public RebateResult GetResultAmountPerUom(Rebate rebate, Product product, CalculateRebateRequest request);
    public RebateResult GetResultFixedRateRebate(Rebate rebate, Product product, CalculateRebateRequest request);
    public RebateResult GetResultFixedCashAmount(Rebate rebate, Product product);
}
