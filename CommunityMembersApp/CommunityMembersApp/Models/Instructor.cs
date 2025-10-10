using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public class Instructor : Teacher // “Maestro”
    {
        public string Grade { get; set; } = "Primary";
        public override string GetRole() => "Instructor";
    }
}
