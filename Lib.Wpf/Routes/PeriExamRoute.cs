namespace Lib.Wpf.Routes
{
    public class PeriExamRoute : BaseRoute
    {
        /// <summary>
        /// API Url
        /// </summary>
        public static string Service() => Service(ApiName.PeriExam);

        public class SYSNid
        {
            public const string Controller = "SYSNid/";
        }

        public class ChkMachine
        {
            public const string Controller = "ChkMachine/";
        }

        public class ChkInTime
        {
            public const string Controller = "ChkInTime/";
        }

        public class Medication
        {
            public const string Controller = "Medication/";
        }

        public class CheckMed
        {
            public const string Controller = "CheckMed/";
        }

        public class AssignDr
        {
            public const string Controller = "AssignDr/";
        }

        public class CheckDr
        {
            public const string Controller = "CheckDr/";
        }

        public class SpecCheckDr
        {
            public const string Controller = "SpecCheckDr/";
        }

        public class CheckItem
        {
            public const string Controller = "CheckItem/";
        }

        public class MachineSchedule
        {
            public const string Controller = "MachineSchedule/";
            public const string QueryDrSchedule = "MachineSchedule/QueryMachineSchedule/";
        }

        public class DrSchedule
        {
            public const string Controller = "DrSchedule/";
            public const string QueryDrSchedule = "DrSchedule/QueryDrSchedule/";
        }

        public class DrDateSchedule
        {
            public const string Controller = "DrDateSchedule/";
            public const string QueryDrSchedule = "DrDateSchedule/QueryDrDateSchedule/";
            public const string PatchDrDateSchByTime = "DrDateSchedule/PatchDrDateSchByTime/";
            public const string PatchDrDateSchRest = "DrDateSchedule/PatchDrDateSchRest/";
            public const string PostDrDateSch = "DrDateSchedule/PostDrDateSch/";
        }

        public class MonthSchedule
        {
            public const string Controller = "MonthSchedule/";
            public const string QueryMonthSchedule = "MonthSchedule/QueryMonthSchedule/";
        }

        public class MachDateSchedule
        {
            public const string Controller = "MachDateSchedule/";
            public const string QueryDrSchedule = "MachDateSchedule/QueryMachDateSchedule/";
            public const string PatchMachDateSchByTime = "MachDateSchedule/PatchMachDateSchByTime/";
        }

        public class PeriItem
        {
            public const string Controller = "PeriItem/";
        }

        public class Diction
        {
            public const string Controller = "Diction/";
        }

        public class Draft
        {
            public const string Controller = "Draft/";
        }

        public class Zd_mcld
        {
            public const string Controller = "Zd_mcld/";
            //public const string QueryHoliday = "Draft/QueryHoliday/";
        }

        public class Ch_prs
        {
            public const string Controller = "Ch_prs/";
            public const string GetCh_prs_Ext = "Ch_prs/GetCh_prs_Ext/";
        }

        public class Mp_mvdr
        {
            public const string Controller = "Mp_mvdr/";
            public const string GetMp_mvdr_Ext = "Mp_mvdr/GetMp_mvdr_Ext/";
        }

    }
}
