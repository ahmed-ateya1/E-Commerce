namespace E_Commerce.Core.Helper
{
    public enum OrderStatus
    {
        Pending,
        Completed,
        Confirmed,
        Processing,
        Cancelled,       
        Returned,        
        FailedPayment ,
        Shipped,
        OutForDelivery,
        Delivered
    }
}
