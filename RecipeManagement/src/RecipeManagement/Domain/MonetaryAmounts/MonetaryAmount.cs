namespace RecipeManagement.Domain.MonetaryAmounts;

using RecipeManagement.Domain.Percentages;
using SharedKernel.Domain;
using FluentValidation;

// source: https://github.com/asc-lab/better-code-with-ddd/blob/ef_core/LoanApplication.TacticalDdd/LoanApplication.TacticalDdd/DomainModel/MonetaryAmount.cs
public class MonetaryAmount : ValueObject
{
    public decimal Amount { get; }
        
    public static readonly MonetaryAmount Zero = new MonetaryAmount(0M);

    public MonetaryAmount(decimal amount) => Amount = decimal.Round(amount,2,MidpointRounding.ToEven);

    public MonetaryAmount Add(MonetaryAmount other) => new MonetaryAmount(Amount + other.Amount);

    public MonetaryAmount Subtract(MonetaryAmount other) => new MonetaryAmount(Amount - other.Amount);
        
    public MonetaryAmount MultiplyByPercent(Percent percent) => new MonetaryAmount((Amount * percent.Value)/100M);

    public static MonetaryAmount operator +(MonetaryAmount one, MonetaryAmount two) => one.Add(two);
        
    public static MonetaryAmount operator -(MonetaryAmount one, MonetaryAmount two) => one.Subtract(two);
        
    public static MonetaryAmount operator *(MonetaryAmount one, Percent percent) => one.MultiplyByPercent(percent);
        
    public static bool operator >(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)>0;
        
    public static bool operator <(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)<0;
        
    public static bool operator >=(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)>=0;
        
    public static bool operator <=(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)<=0;

    public int CompareTo(MonetaryAmount other)
    {
        return Amount.CompareTo(other.Amount);
    }
}