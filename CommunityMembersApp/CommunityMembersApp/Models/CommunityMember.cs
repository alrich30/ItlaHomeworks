using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityMembersApp.Models
{
    public abstract class CommunityMember {
        public int Id { get; set; } 
        public string FullName { get; set; } = null!; 
        public string? Email { get; set; } 
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow; 
        public virtual string GetRole() => "Member"; 
        public override string ToString() => $"{Id} | {FullName} | {GetRole()}"; }
}
