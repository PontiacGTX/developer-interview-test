using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Data.Contracts;
using Smartwyre.DeveloperTest.Data.Contracts.Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore) : IRebateService
{
    public RebateResult Calculate(CalculateRebateRequest request)
    {
        decimal rebateAmount = 0m;
        RebateResult rebateResult =  new RebateResult();
        Rebate rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate == null || product == null)
            return rebateResult;

        rebateResult = GetRebateResult(rebate, product, request);
        rebateAmount = rebateResult.Amount;
         
        if (!rebateResult.Success)
            return  rebateResult;


        rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return  rebateResult;
    }

    public RebateResult GetRebateResult(Rebate rebate,Product product, CalculateRebateRequest request)
    {
        return rebate?.Incentive switch
        {
            IncentiveType.FixedCashAmount => GetResultFixedCashAmount(rebate, product),
            IncentiveType.FixedRateRebate => GetResultFixedRateRebate(rebate, product, request),
            IncentiveType.AmountPerUom => GetResultAmountPerUom(rebate, product, request),
            _ => new RebateResult { Success = false }
        };
    }

    public RebateResult GetResultAmountPerUom(Rebate rebate,Product product, CalculateRebateRequest request)
    {
        RebateResult result = new RebateResult();

        if ( !product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) ||
            (rebate.Amount == 0 || request.Volume == 0))
            return result;

        result.Amount += rebate.Amount * request.Volume;
        result.Success = true;

        return result;
    }

    public RebateResult GetResultFixedRateRebate(Rebate rebate,Product product, CalculateRebateRequest request)
    {
        RebateResult result = new RebateResult();

        if((rebate?.Percentage == 0 || product?.Price == 0 || request?.Volume == 0) ||
             !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
            return result;

        result.Amount += product.Price * rebate.Percentage * request.Volume;
        result.Success = true;

        return result;
    }

    public RebateResult GetResultFixedCashAmount(Rebate rebate,Product product)
    {
        RebateResult result = new RebateResult();

        if(!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) || rebate?.Amount == 0)
            return result;

        result.Success = true;
        result.Amount = rebate.Amount;  

        return result;
    }
}
