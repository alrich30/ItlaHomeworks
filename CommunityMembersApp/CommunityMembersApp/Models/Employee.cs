using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public abstract class Employee : CommunityMember
    {
        public decimal BaseSalary { get; set; }
        public override string GetRole() => "Employee";
        public virtual decimal CalculateSalary() => BaseSalary;
    }
}
