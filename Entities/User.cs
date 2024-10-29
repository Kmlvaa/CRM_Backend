using CRM.Enums;
using Microsoft.AspNetCore.Identity;

namespace CRM.Entities
{
    public class User : IdentityUser
    {
        public required string Name { get; set; }
        public RoleType Role { get; set; }  
    }
}
