namespace Models
{
    public class RoleUserPermissionParam : BaseModel<RoleUserPermissionParam>
    {
        private RoleUserPermissionParamOption? _Option;
        public RoleUserPermissionParamOption? Option
        {
            get => _Option;
            set => Set(ref _Option, value);
        }

        private string _UserId;
        public string UserId
        {
            get => _UserId;
            set => Set(ref _UserId, value);
        }

        private string _SysId;
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private string _RoleId;
        public string RoleId
        {
            get => _RoleId;
            set => Set(ref _RoleId, value);
        }

        private bool? _Activate;
        public bool? Activate
        {
            get => _Activate;
            set => Set(ref _Activate, value);
        }
    }

    public enum RoleUserPermissionParamOption
    {
        /// <summary>
        /// 依使用者查詢
        /// </summary>
        ByUserInfo = 1,
        /// <summary>
        /// 依系統代碼查詢
        /// </summary>
        BySysId = 2,
        /// <summary>
        /// 依角色群組查詢
        /// </summary>
        ByRoleId = 3
    }
}
