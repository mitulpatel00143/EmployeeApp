using DAL.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class UnitOfWork : IUnitOfWork
    {

        public IConfiguration Configuration { get; }
        private readonly ILogger Logger;
        string connection = "";

        public UnitOfWork(ILogger<UnitOfWork> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            connection = Configuration.GetConnectionString("DefaultConnection");
        }

        public DataSet GetDataSet(string sql)
        {
            var result = new DataSet();
            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = sql;

                    try
                    {
                        con.Open();
                        var reader = command.ExecuteReader();

                        do
                        {
                            var tb = new DataTable();
                            tb.Load(reader);
                            result.Tables.Add(tb);
                        } while (!reader.IsClosed);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return result;
        }

        public DataSet GetDataSet(string sql, List<SqlParameter> parameters)
        {
            var result = new DataSet();
            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddRange(parameters.ToArray());

                    try
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        do
                        {
                            var tb = new DataTable();
                            tb.Load(reader);
                            result.Tables.Add(tb);
                        } while (!reader.IsClosed);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return result;
        }

        public void ExecuteScalarStoredProcedure(string psName, List<SqlParameter> parameters, bool timeoutNull)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.Connection = con;
                    command.CommandText = psName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());

                    if (timeoutNull)
                        command.CommandTimeout = 0;

                    command.ExecuteScalar();

                    con.Close();
                }
            }
        }

        public void ExecuteNonQueryStoredProcedure(string psName, List<SqlParameter> parameters, bool timeoutNull)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.Connection = con;
                    command.CommandText = psName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());

                    if (timeoutNull)
                        command.CommandTimeout = 0;

                    command.ExecuteNonQuery();

                    con.Close();
                }
            }
        }

        
    }
}
