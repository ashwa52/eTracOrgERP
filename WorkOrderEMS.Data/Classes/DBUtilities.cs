using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Data.Classes
{
    public class DBUtilities
    {
        string Connstr = ConfigurationManager.AppSettings["SQLConnection"].ToString();
        int SQLTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SQLCommandTimeout"] ?? "6000");
        public DataTable GetDTResponse(string Squery)
        {
            DataTable dataTable = new DataTable();
            var conn = new SqlConnection(Connstr);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Squery;
                cmd.CommandTimeout = SQLTimeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Close();
                da.Fill(dataTable);
                da.Dispose();
                conn.Close();
            }
            return dataTable;
        }
        public DataSet GetDSResponse(string Squery, bool isStoreExecution = false, string ModuleName = "Cygnux")
        {
            DataSet DS = new DataSet();
            var conn = new SqlConnection(Connstr);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Squery;
                cmd.CommandTimeout = SQLTimeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Close();
                da.Fill(DS);
                da.Dispose();
                conn.Close();
            }
            return DS;
        }
        public void SetWOResponse(string Squery)
        {
            var conn = new SqlConnection(Connstr);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Squery;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
            }
        }
        public DataTable GetDTResponseFromParams(string Squery, IDictionary<string, string> parameters)
        {
            DataTable DT = new DataTable();
            var conn = new SqlConnection(Connstr);

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Squery;
                cmd.CommandTimeout = SQLTimeout;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var itm in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + itm.Key, itm.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Close();
                da.Fill(DT);
                da.Dispose();
                conn.Close();
            }
            return DT;
        }
    }
}