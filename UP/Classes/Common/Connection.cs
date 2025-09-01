using MySql.Data.MySqlClient;

namespace UP.Classes.Common
{
    public class Connection
    {
        readonly static string path = "server=127.0.0.1;port=3306;database=UP;uid=root;";
        public static MySqlConnection OpenConnection() {
            MySqlConnection connection = new MySqlConnection(path);
            connection.Open();
            return connection;
        }
        public static MySqlDataReader Query(string SQL, MySqlConnection connection)
        {
            MySqlCommand command = new MySqlCommand(SQL, connection);
            return command.ExecuteReader();
        }

        public static void CloseConnection(MySqlConnection connection)
        {
            connection.Close();
            MySqlConnection.ClearPool(connection);
        }
    }
}
