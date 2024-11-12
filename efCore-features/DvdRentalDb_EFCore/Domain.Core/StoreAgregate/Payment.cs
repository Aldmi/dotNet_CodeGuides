using Domain.Core.CustomerAgregate;
using Domain.Core.StaffAgregate;

namespace Domain.Core.StoreAgregate;

/// <summary>
/// оплата
/// </summary>
public class Payment
{
    public int PaymentId { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    public Customer Customer { get; set; }  //Заказшик
    public Staff Staff { get; set; }        //Кто выдал платеж
    public Rental Rental { get; set; }      //
}