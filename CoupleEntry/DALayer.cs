using CoupleEntry.Models;
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

        public static void UpsertTokenValue(string tokenId, string emailId)
        {
            string procName = "UpsertTokenValue";
            IDbCommand command = new SqlCommand(procName);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter() { ParameterName = "CookieId", SqlDbType = SqlDbType.NVarChar, Value = tokenId });
            command.Parameters.Add(new SqlParameter() { ParameterName = "EmailId", SqlDbType = SqlDbType.NVarChar, Value = emailId });

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public static void AddNewUser(string userName, int age, string emailId, string gender, string imageUrl, string name)
        {
            string procName = "AddNewUser";
            IDbCommand command = new SqlCommand(procName);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter() { ParameterName = "UserName", SqlDbType = SqlDbType.NVarChar, Value = userName });
            command.Parameters.Add(new SqlParameter() { ParameterName = "Age", SqlDbType = SqlDbType.Int, Value = age });
            command.Parameters.Add(new SqlParameter() { ParameterName = "Gender", SqlDbType = SqlDbType.NVarChar, Value = gender });
            command.Parameters.Add(new SqlParameter() { ParameterName = "ImageUrl", SqlDbType = SqlDbType.NVarChar, Value = imageUrl });
            command.Parameters.Add(new SqlParameter() { ParameterName = "Name", SqlDbType = SqlDbType.NVarChar, Value = name });
            command.Parameters.Add(new SqlParameter() { ParameterName = "EmailId", SqlDbType = SqlDbType.NVarChar, Value = emailId });

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}