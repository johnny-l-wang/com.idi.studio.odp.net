using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace IDI.Studio.ODP
{
    public static class OracleExtention
    {
        public static List<T> ToList<T>(this OracleDataReader reader) where T : new()
        {
            throw new NotImplementedException();
        }
    }
}
