using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class ManageRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public bool Value { get; set; }

        public ICollection<Permission> Permissions { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ManageRole()
        {
            Permissions = new List<Permission>();
            Roles = new List<Role>();
        }
    }
}
