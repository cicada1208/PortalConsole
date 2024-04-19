namespace Lib.Wpf.Routes
{
    public class PdsRoute : BaseRoute
    {
        /// <summary>
        /// API Url
        /// </summary>
        public static string Service() => Service(ApiName.Pds);

        public class Schema
        {
            public const string CreateModel = "Schema/CreateModel/";
        }

        public class DB
        {
            public const string QueryDB = "DB/QueryDB/";
        }

        public class Users
        {
            public const string QueryUser = "Users/QueryUser/";
            public const string UpdateUser = "Users/UpdateUser/";
        }

        public class Mg_mnid
        {
            public const string QueryUser = "Mg_mnid/QueryUser/";
            public const string QueryMg_mnid = "Mg_mnid/QueryMg_mnid/";
        }

        public class SysParameter
        {
            public const string QuerySysParameter = "SysParameter/QuerySysParameter/";
        }

        public class Ch_prs_code
        {
            public const string QueryCh_prs_code = "Ch_prs_code/QueryCh_prs_code/";
            public const string SaveCh_prs_code = "Ch_prs_code/SaveCh_prs_code/";
        }

        public class Ch_prs
        {
            public const string QueryCh_prs = "Ch_prs/QueryCh_prs/";
        }

        public class PdsPatInfo
        {
            public const string QueryPdsPatInfo = "PdsPatInfo/QueryPdsPatInfo/";
        }

        public class PdsMedInfo
        {
            public const string QueryPdsMedInfo = "PdsMedInfo/QueryPdsMedInfo/";
        }

        public class Rec_code
        {
            public const string QueryRec_code = "Rec_code/QueryRec_code/";
        }

        public class Pds_rec
        {
            public const string QueryPds_rec = "Pds_rec/QueryPds_rec/";
            public const string QueryPdsRecMicbcode = "Pds_rec/QueryPdsRecMicbcode/";
            public const string QueryPdsRecAC = "Pds_rec/QueryPdsRecAC/";
            public const string QueryFstAvg = "Pds_rec/QueryFstAvg/";
            public const string SavePds_rec = "Pds_rec/SavePds_rec/";
            public const string SavePds_rec_S = "Pds_rec/SavePds_rec_S/";
        }

        public class Pds_recd
        {
            public const string QueryPds_recd = "Pds_recd/QueryPds_recd/";
        }

        public class Ch_udrec
        {
            public const string QueryCh_udrec = "Ch_udrec/QueryCh_udrec/";
        }

        public class Ch_udrec_chk
        {
            public const string QueryCh_udrec_chk = "Ch_udrec_chk/QueryCh_udrec_chk/";
            public const string InsertCh_udrec_chk = "Ch_udrec_chk/InsertCh_udrec_chk/";
            public const string UpdateCh_udrec_chk = "Ch_udrec_chk/UpdateCh_udrec_chk/";
        }

        public class Mi_micbcode
        {
            public const string QueryMi_micbcode = "Mi_micbcode/QueryMi_micbcode/";
            public const string QueryLstComplete = "Mi_micbcode/QueryLstComplete/";
        }

        public class Ch_bcode
        {
            public const string QueryCh_bcode = "Ch_bcode/QueryCh_bcode/";
        }

        public class Mch_msen
        {
            public const string QueryMch_msen = "Mch_msen/QueryMch_msen/";
        }

        public class Mr_lstud
        {
            public const string QueryMr_lstud = "Mr_lstud/QueryMr_lstud/";
        }

        public class Pds_note
        {
            public const string QueryPds_note = "Pds_note/QueryPds_note/";
            public const string QueryPdsNoteMicbcode = "Pds_note/QueryPdsNoteMicbcode/";
            public const string SavePds_note = "Pds_note/SavePds_note/";
        }

        public class Ch_remakemar
        {
            public const string QueryCh_remakemar = "Ch_remakemar/QueryCh_remakemar/";
        }

    }
}
