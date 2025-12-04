using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDeliverySystem.Domain.Core
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } //= Guid.NewGuid();
        public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; protected set; }

        protected void Touch() => UpdatedAtUtc = DateTime.UtcNow;
    }
}
