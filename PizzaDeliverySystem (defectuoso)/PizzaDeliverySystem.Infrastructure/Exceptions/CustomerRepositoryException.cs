using System;

namespace PizzaDeliverySystem.Infrastructure.Exceptions
{
    public class CustomerRepositoryException : Exception
    {
        public CustomerRepositoryException()
        {
        }

        public CustomerRepositoryException(string message)
            : base(message)
        {
        }

        public CustomerRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
