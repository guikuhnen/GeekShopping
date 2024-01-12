namespace GeekShopping.PaymentProcessor
{
	public interface IProcessPayment
	{
		Task<bool> PaymentProcessor();
	}
}
