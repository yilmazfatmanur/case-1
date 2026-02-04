namespace ECommerce.Domain
{
	public enum SagaState
	{
	   OrderCreated = 1,
	   InventoryReserving = 2,
	   InventoryReserved = 3,
	   PaymentProcessing = 4, 
	   PaymentProcessed = 5,
	   ShippingPreparing = 6,
	   Completed = 7,
	   InventoryReservationFailed = 8,
	   PaymentFailed = 9,
	   ShippingFailed = 10,
	   Cancelled = 11
		
	}

}