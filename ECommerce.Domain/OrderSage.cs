namespace ECommerce.Domain
 {
	public class OrderSaga
	{
	   public int Id { get; set; }
	   public int OrderId { get; set; }
	   public SagaState CurrentState { get; set; }
	   public DateTime CreatedAt { get; set; }
	   public DateTime? UpdatedAt { get; set; }
	   public string? ErrorMessage { get; set; }


	   public Order Order { get; set; }
	
	 }


 }