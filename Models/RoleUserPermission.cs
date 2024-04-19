using System.Collections.Generic;

namespace Models
{
    public class RoleUserPermission : BaseModel<RoleUserPermission>
    {
        private List<RoleUser> _RoleUsers;
        public List<RoleUser> RoleUsers
        {
            get => _RoleUsers;
            set => Set(ref _RoleUsers, value);
        }

        private List<RolePermission> _RolePermissions;
        public List<RolePermission> RolePermissions
        {
            get => _RolePermissions;
            set => Set(ref _RolePermissions, value);
        }
    }
}
