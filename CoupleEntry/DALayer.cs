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

        public static object DataAccess { get; private set; }

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
        public static void UpsertUserPosition(string emailId, string latitude, string longitude)
        {
            string procName = "UpsertUserPosition";
            IDbCommand command = new SqlCommand(procName);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter() { ParameterName = "EmailId", SqlDbType = SqlDbType.NVarChar, Value = emailId });
            command.Parameters.Add(new SqlParameter() { ParameterName = "Latitude", SqlDbType = SqlDbType.NVarChar, Value = latitude });
            command.Parameters.Add(new SqlParameter() { ParameterName = "Longitude", SqlDbType = SqlDbType.NVarChar, Value = longitude });

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

        public static List<User> GetAllUsers(string emailId)
        {
            List<User> allUsers = new List<User>();

            string procName = "GetAllOtherUsers";
            IDbCommand command = new SqlCommand(procName);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter() { ParameterName = "EmailId", SqlDbType = SqlDbType.NVarChar, Value = emailId });

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;
                connection.Open();


                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            string name = GetStringFromReader("Name", reader);
                            string emailid = GetStringFromReader("EmailId", reader);
                            string imageurl = GetStringFromReader("ImageUrl", reader);
                            int age = Convert.ToInt32(GetStringFromReader("Age", reader));
                            string gender = GetStringFromReader("Gender", reader);
                            string latitude = GetStringFromReader("Latitude", reader);
                            string longitude = GetStringFromReader("Longitude", reader);
                            DateTime lastseen = Convert.ToDateTime(GetStringFromReader("LastSeen", reader));

                            allUsers.Add(new User() { Name = name, EmailId = emailid, ImageUrl = imageurl, Age= age, Gender = gender, Latitude = latitude, Longitude = longitude, LastSeen = lastseen });
                        }
                    }
                }

                connection.Close();
            }


            return allUsers;
        }

        public static string GetStringFromReader(string column, IDataReader reader)
        {
            string value = null;

            int index = reader.GetOrdinal(column);

            if (!reader.IsDBNull(index))
            {
                value = reader.GetValue(index)?.ToString()?.Trim();
                if (string.IsNullOrEmpty(value))
                {
                    value = null;
                }
            }

            return value;
        }
    }
}