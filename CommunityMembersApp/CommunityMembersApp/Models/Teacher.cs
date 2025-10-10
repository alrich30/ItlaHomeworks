using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public abstract class Teacher : Employee
    {
        public string Subject { get; set; } = "General";
        public override string GetRole() => "Teacher";
    }
}
