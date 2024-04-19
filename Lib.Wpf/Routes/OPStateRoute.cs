namespace Lib.Wpf.Routes
{
    public class OPStateRoute : BaseRoute
    {
        /// <summary>
        /// API Url
        /// </summary>
        public static string Service() => Service(ApiName.OPState);

        public class PatientLocation
        {
            public const string PatientOPList = "PatientOPList/";
        }

    }
}
