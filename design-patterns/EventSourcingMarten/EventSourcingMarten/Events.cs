namespace EventSourcingMarten;


/// <summary>
/// События заказа
/// </summary>
public class Events
{
	/// <summary>
	/// Заказ создан
	/// </summary>
	public class OrderCreated
	{
		public Guid Id { get; set; } = Guid.CreateVersion7(); //!!! В событиях обязательно идентификатор по имени Id
		public string ProductName { get; set; }
		public string DeliveryAddress { get; set; }
	}
	
	/// <summary>
	/// адрес добавлен
	/// </summary>
	public class OrderAddressUpdated
	{
		public Guid Id { get; set; }
		public string DeliveryAddress { get; set; }
	}
	
	/// <summary>
	/// заказ подтвержден
	/// </summary>
	public class OrderDispatched
	{
		public Guid Id { get; set; }
		public DateTime DispatchedAtUtc { get; set; }
	}
	
	/// <summary>
	/// заказ отправлен
	/// </summary>
	public class OrderOutForDelivery
	{
		public Guid Id { get; set; }
		public DateTime OutForDeliveryAtUtc { get; set; }
	}
	
	/// <summary>
	/// заказ доставлен
	/// </summary>
	public class OrderDelivered
	{
		public Guid Id { get; set; }
		public DateTime DeliveredAtUtc { get; set; }
	}
}