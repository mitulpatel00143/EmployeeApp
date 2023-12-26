using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstract
{
    public interface IUnitOfWork
    {
        DataSet GetDataSet(string sql);
        DataSet GetDataSet(string sql, List<SqlParameter> parameters);
        void ExecuteScalarStoredProcedure(string psName, List<SqlParameter> parameters, bool timeoutNull);
        void ExecuteNonQueryStoredProcedure(string psName, List<SqlParameter> parameters, bool timeoutNull);
    }
}
