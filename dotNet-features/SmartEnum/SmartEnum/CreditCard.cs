namespace SmartEnum;


/// <summary>
/// CreditCard - сущность с огрниченным набором вариантов
/// </summary>
public abstract class CreditCard : Enumeration<CreditCard>
{
    public static CreditCard Standart = new StandartCreditCard();
    public static CreditCard Premium = new PremiumCreditCard();
    public static CreditCard Platinum = new PlatinumCreditCard();
    
    protected CreditCard(int value, string name) 
        : base(value, name)
    {
    }

    public abstract double Discount { get; }
    
    
    
    private sealed class StandartCreditCard : CreditCard
    {
        public StandartCreditCard() 
            : base(1, "Standart")
        {
        }

        public override double Discount => 0.01;
    }
    
    
    private sealed class PremiumCreditCard : CreditCard
    {
        public PremiumCreditCard() 
            : base(2, "Premium")
        {
        }

        public override double Discount => 0.05;
    }
    
    private sealed class PlatinumCreditCard : CreditCard
    {
        public PlatinumCreditCard() 
            : base(3, "Platinum")
        {
        }

        public override double Discount => 0.1;
    }
}

