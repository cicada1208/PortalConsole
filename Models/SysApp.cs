using Params;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "SysApp")]
    public class SysApp : SysAppBase<SysApp>
    {
    }
}
