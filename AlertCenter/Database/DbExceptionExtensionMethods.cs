using System.Data.Common;

namespace AlertCenter.Database
{
    public static class DbExceptionExtensions
    {
        public static int ErrorNumber(this DbException x)
        {
            return int.Parse(x.Message.Substring(0, x.Message.IndexOf(":")));
        }
    }
}
