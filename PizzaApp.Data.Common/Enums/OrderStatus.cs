namespace PizzaApp.Data.Common.Enums
{
    public enum OrderStatus
    {
        Received = 1,       // Order was received and is awaiting to start preparation
        Preparing,          // Order is being prepared
        WaitingForTransit,  // Order is waiting for available driver
        InTransit,          // Order is in transit
        Delivered,          // Order was successfully delivered
        Cancelled,          // Order was cancelled by the user
        FailedToDeliver     // The order was prepared but delivery was unsuccessful
    }
}
