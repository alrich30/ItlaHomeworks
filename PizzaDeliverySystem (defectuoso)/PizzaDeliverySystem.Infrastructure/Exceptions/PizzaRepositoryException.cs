using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDeliverySystem.Infrastructure.Exceptions
{
    public class PizzaRepositoryException : Exception
    {
        public PizzaRepositoryException(string message, Exception? inner = null)
            : base(message, inner)
        {
        }
    }
}
