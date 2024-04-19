namespace Lib.Wpf.Routes
{
    public class HrRoute : BaseRoute
    {
        /// <summary>
        /// API Url
        /// </summary>
        public static string Service() => Service(ApiName.Hr);

        public class SearchEmp
        {
            public const string UserInfos = "SearchEmp/UserInfos/";
        }

        public class BasicInfo
        {
            public const string UserInfosV2 = "BasicInfo/V2/";
            public const string GetPositions = "BasicInfo/DepnoQuickChange/GetPositions/";
            public const string GetAttributeSets = "BasicInfo/PositionTransaction/GetAttributeSets";
        }

        public class Org
        {
            public const string GetCompany = "Org/GetCompany/";
            public const string GetDept = "Org/Depts/";
        }

    }
}
