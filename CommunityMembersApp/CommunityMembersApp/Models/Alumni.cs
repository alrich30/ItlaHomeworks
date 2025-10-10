using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public class Alumni : CommunityMember
    {
        public DateTime GraduationDate { get; set; } = DateTime.UtcNow;
        public override string GetRole() => "Alumni";
    }
}
