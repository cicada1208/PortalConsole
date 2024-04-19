using Params;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "Func")]
    public class Func : FuncBase<Func>
    {
    }
}
