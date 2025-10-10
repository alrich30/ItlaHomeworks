using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public class Administrative : Employee
    {
        public string Department { get; set; } = "General";
        public override string GetRole() => "Administrative";
    }
}
