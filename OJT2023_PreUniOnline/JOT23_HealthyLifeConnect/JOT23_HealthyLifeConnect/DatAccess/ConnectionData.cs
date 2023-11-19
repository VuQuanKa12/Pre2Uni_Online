﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
    public class ConnectionData
    {
        static string connectionString = "server=DESKTOP-KF9V6N0;database=PreUniOnline_Last;uid=sa;pwd=123;TrustServerCertificate=true;Encrypt=false";
     
        public static DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    if (parameter != null)
                    {
                        string[] temp = query.Split(' ');
                        List<string> listParameter = new List<string>();

                        foreach (string item in temp)
                        {
                            if (item.StartsWith("@"))
                            {
                                listParameter.Add(item.ToString());
                            }
                        }

                        for (int i = 0; i < parameter.Length; i++)
                        {
                            sqlCommand.Parameters.AddWithValue(listParameter[i], parameter[i]);
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                    {

                        adapter.Fill(data);
                    }
                }

                connection.Close();
            }
            return data;
        }

		public static bool ExecuteUpdate(string query, object[] parameter = null)
		{
			bool result = false;
			int value = 0;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand sqlCommand = new SqlCommand(query, connection))
				{
					if (parameter != null)
					{
						string[] temp = query.Split(' ');
						List<string> listParameter = new List<string>();

						foreach (string item in temp)
						{
							if (item.StartsWith("@"))
							{
								listParameter.Add(item.ToString());
							}
						}

						for (int i = 0; i < parameter.Length; i++)
						{
							sqlCommand.Parameters.AddWithValue(listParameter[i], parameter[i]);
						}
					}
					value = sqlCommand.ExecuteNonQuery();

					connection.Close();
				}
			}
			return result = value > 0 ? true : false;
		}



	}
}
