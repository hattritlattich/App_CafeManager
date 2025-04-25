using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace AppQLQuanCaPhe.Model
{
    
    internal class DBConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private string connect;

        private static DBConnect instance;

        public static DBConnect Instance
        {
            get { if (instance == null) instance = new DBConnect(); return DBConnect.instance; }
            private set { DBConnect.instance = value; }
        }
        private string connectionSTR = @"Data Source=ALBERT\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;";
        public string myConnect()
        {
            connect = @"Data Source=ALBERT\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;";
            return connect;
        }
        public DataTable getTable(string query)
        {
            cn.ConnectionString = myConnect();
            cm = new SqlCommand(query, cn);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();
            }
            return data;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}


//public String getPassword(string username)
//{
//    string password = "";
//    cn.ConnectionString = myConnect();
//    cn.Open();
//    cm = new SqlCommand("select password from tbUser where username = '" + username + "'", cn);
//    dr = cm.ExecuteReader();
//    dr.Read();
//    if (dr.HasRows)
//    {
//        password = dr["password"].ToString();
//    }
//    dr.Close();
//    cn.Close();
//    return password;
//}

