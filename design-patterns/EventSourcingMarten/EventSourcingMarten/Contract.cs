namespace EventSourcingMarten;

public record CreateOrderRequest(string ProductName, string DeliveryAddress);

public record DeliveryAddressUpdateRequest(string DeliveryAddress);