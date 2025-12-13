// Domain/Exceptions/UserRepositoryException.cs
using System;

namespace PizzaDeliverySystem.Domain.Exceptions;

public class UserRepositoryException : Exception
{
    public UserRepositoryException()
    {
    }

    public UserRepositoryException(string message)
        : base(message)
    {
    }

    public UserRepositoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}