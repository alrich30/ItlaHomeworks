using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public class Student : CommunityMember
    {
        public string StudentNumber { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public override string GetRole() => "Student";
    }
}
