using System.Collections.Generic;

namespace Istra.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }

        public ICollection<ManageRole> ManageRoles { get; set; }
        public Permission()
        {
            ManageRoles = new List<ManageRole>();
        }
    }
}