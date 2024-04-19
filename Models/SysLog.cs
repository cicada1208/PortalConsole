using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "SysLog")]
    public class SysLog : BaseModel<SysLog>
    {
        private string _LogDateTime;
        public string LogDateTime
        {
            get => _LogDateTime;
            set => Set(ref _LogDateTime, value);
        }

        private string _UserId;
        public string UserId
        {
            get => _UserId;
            set => Set(ref _UserId, value);
        }

        private string _UserIP;
        public string UserIP
        {
            get => _UserIP;
            set => Set(ref _UserIP, value);
        }

        private string _SysId;
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private string _ProcId;
        public string ProcId
        {
            get => _ProcId;
            set => Set(ref _ProcId, value);
        }

        private string _ActionType;
        public string ActionType
        {
            get => _ActionType;
            set => Set(ref _ActionType, value);
        }

        private string _ActionTarget;
        public string ActionTarget
        {
            get => _ActionTarget;
            set => Set(ref _ActionTarget, value);
        }

        private string _ControllerClass;
        public string ControllerClass
        {
            get => _ControllerClass;
            set => Set(ref _ControllerClass, value);
        }

        private string _ActionMethod;
        public string ActionMethod
        {
            get => _ActionMethod;
            set => Set(ref _ActionMethod, value);
        }

        private bool? _State;
        public bool? State
        {
            get => _State;
            set => Set(ref _State, value);
        }

        private string _Msg;
        public string Msg
        {
            get => _Msg;
            set => Set(ref _Msg, value);
        }

    }
}
