namespace EventSourcingMarten;

/// <summary>
/// Модель заказа
/// </summary>
public class Order
{
	public Guid Id { get; set; }
	
	public string ProductName { get; set; }
	
	public string DeliveryAddress { get; set; }
	
	public DateTime? DispatchAtUtc { get; set; }  //Если время проверки заказа равно null, то стадия еще не пройдена.
	
	public DateTime? OutForDeliveryAtUtc { get; set; }
	
	public DateTime? DeliveredAtUtc { get; set; }

	
	
	public void Apply(Events.OrderCreated created)
	{
		ProductName = created.ProductName;
		DeliveryAddress = created.DeliveryAddress;
	}
	
	public void Apply(Events.OrderDispatched dispatched)
	{
		DispatchAtUtc = dispatched.DispatchedAtUtc;
	}
	
	public void Apply(Events.OrderOutForDelivery outForDelivery)
	{
		OutForDeliveryAtUtc = outForDelivery.OutForDeliveryAtUtc;
	}
	
	public void Apply(Events.OrderDelivered delivered)
	{
		DeliveredAtUtc = delivered.DeliveredAtUtc;
	}
	
	public void Apply(Events.OrderAddressUpdated updated)
	{
		DeliveryAddress = updated.DeliveryAddress;
	}
}