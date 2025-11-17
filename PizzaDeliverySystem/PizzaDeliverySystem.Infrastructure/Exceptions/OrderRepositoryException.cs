using System;

namespace PizzaDeliverySystem.Infrastructure.Exceptions
{
    public class OrderRepositoryException : Exception
    {
        public OrderRepositoryException()
        {
        }

        public OrderRepositoryException(string message)
            : base(message)
        {
        }

        public OrderRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
