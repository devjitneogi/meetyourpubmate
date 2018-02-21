using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CoupleEntry
{
    public class DALayer
    {
        private static string _connectionString = "Data Source=bgrsql01;;Initial Catalog=LocalTest;Integrated Security=False;user id=sa;password=Squeeze66";

        public static bool IsEmailPresentInDB(string emailId)
        {
            string procName = "IsEmailPresentInDB";
            SqlParameter returnParameter = new SqlParameter() { ParameterName = "IsExists", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            IDbCommand command = new SqlCommand(procName);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter() { ParameterName = "EmailId", SqlDbType = SqlDbType.VarChar, Value = emailId });
            command.Parameters.Add(returnParameter);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Convert.ToBoolean(returnParameter.Value);
        }

    }
}