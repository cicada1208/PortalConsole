namespace Lib.Wpf.Routes
{
    public class UAACRoute : BaseRoute
    {
        /// <summary>
        /// API Url
        /// </summary>
        public static string Service() => Service(ApiName.UAAC);

        public static string Token { get; set; }

        public class Auth
        {
            public const string Controller = "Auth/";
            public const string GetUserInfo = "Auth/GetUserInfo/";
        }

        public class Permission
        {
            public const string GetSysGroup = "Permission/GetSysGroup/";
            public const string GetEntireSysGroup = "Permission/GetEntireSysGroup/";
            public const string GetFavoriteSysGroup = "Permission/GetFavoriteSysGroup/";
            public const string GetFuncGroup = "Permission/GetFuncGroup/";
            public const string GetEntireFuncGroup = "Permission/GetEntireFuncGroup/";
            public const string GetFavoriteFuncGroup = "Permission/GetFavoriteFuncGroup/";
            public const string GetSysFuncPermission = "Permission/GetSysFuncPermission/";
            public const string GetSysFuncPermissionDetail = "Permission/GetSysFuncPermissionDetail/";
            public const string GetSysFuncPermissionDetailDistinct = "Permission/GetSysFuncPermissionDetailDistinct/";
            public const string GetUserPermission = "Permission/GetUserPermission/";
            public const string GetRolePermission = "Permission/GetRolePermission/";
            public const string GetRoleUserPermission = "Permission/GetRoleUserPermission/";
            public const string GetUserRole = "Permission/GetUserRole/";
        }

        public class SysLog
        {
            public const string Controller = "SysLog/";
        }

        public class Mg_mnid
        {
            public const string Controller = "Mg_mnid/";
            public const string GetUser = "Mg_mnid/GetUser/";
            public const string GetDr = "Mg_mnid/GetDr/";
        }

        public class WORKERS
        {
            public const string GetWORKER = "WORKERS/GetWORKER/";
        }

        public class MISSYSTEM
        {
            public const string GetUSERMISSYSTEM = "MISSYSTEM/GetUSERMISSYSTEM/";
        }

        public class SysApp
        {
            public const string Controller = "SysApp/";
            public const string GetSysCatalogAddList = "SysApp/GetSysCatalogAddList/";
            public const string GetSysCatalogSortList = "SysApp/GetSysCatalogSortList/";
        }

        public class Func
        {
            public const string Controller = "Func/";
            public const string GetFuncCatalogAddList = "Func/GetFuncCatalogAddList/";
            public const string GetFuncCatalogSortList = "Func/GetFuncCatalogSortList/";
        }

        public class Role
        {
            public const string Controller = "Role/";
        }

        public class RoleUser
        {
            public const string Controller = "RoleUser/";
        }

        public class RolePermission
        {
            public const string Controller = "RolePermission/";
            public const string BatchPatchAuth = "RolePermission/BatchPatchAuth/";
        }

        public class UserPermission
        {
            public const string Controller = "UserPermission/";
            public const string BatchPatchAuth = "UserPermission/BatchPatchAuth/";
        }

        public class SysCatalog
        {
            public const string BatchInsertSysApp = "SysCatalog/BatchInsertSysApp/";
            public const string BatchPatchDeactivate = "SysCatalog/BatchPatchDeactivate/";
            public const string BatchPatchSeq = "SysCatalog/BatchPatchSeq/";
        }

        public class FuncCatalog
        {
            public const string BatchInsertFunc = "FuncCatalog/BatchInsertFunc/";
            public const string BatchPatchDeactivate = "FuncCatalog/BatchPatchDeactivate/";
            public const string BatchPatchSeq = "FuncCatalog/BatchPatchSeq/";
        }

        public class SysUserFavorite
        {
            public const string Controller = "SysUserFavorite/";
            public const string PatchDeactivate = "SysUserFavorite/PatchDeactivate/";
            public const string BatchPatchSeq = "SysUserFavorite/BatchPatchSeq/";
        }

        public class FuncUserFavorite
        {
            public const string Controller = "FuncUserFavorite/";
            public const string PatchDeactivate = "FuncUserFavorite/PatchDeactivate/";
            public const string BatchPatchSeq = "FuncUserFavorite/BatchPatchSeq/";
        }

    }
}
