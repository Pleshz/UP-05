using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UP.Classes.Common;
using UP.Interfaces;
using UP.Models;

namespace UP.Classes.Context
{
    public class UserContext : User, IUser
    {
        public List<UserContext> AllUsers()
        {
            List<UserContext> allUsers = new List<UserContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Users`", connection);

            while (data.Read())
            {
                UserContext user = new UserContext
                {
                    Id = data.GetInt32(0),
                    Login = data.GetString(1),
                    Email = data.GetString(2),
                    Password = data.GetString(3),
                    CreatedAt = data.GetDateTime(4),
                    UpdateAt = data.GetDateTime(5),
                    FullName = data.GetString(6),
                    Bio = data.GetString(7)
                };
                allUsers.Add(user);
            }
            return allUsers;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Users` " +
                                    "SET " +
                                        $"`Login` = '{this.Login}', " +
                                        $"`Email` = '{this.Email}', " +
                                        $"`Password` = '{this.Password}', " +
                                        $"`CreatedAt` = '{this.CreatedAt}', " +
                                        $"`UpdateAt` = '{this.UpdateAt}', " +
                                        $"`FullName` = '{this.FullName}', " +
                                        $"`Bio` = '{this.Bio}' " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Users` " +
                                    "(`Login`, `Email`, `Password`, `CreatedAt`, `UpdateAt`, `FullName`, `Bio`) " +
                                 $"VALUES ('{this.Login}', '{this.Email}', '{this.Password}', '{this.CreatedAt}', '{this.UpdateAt}', '{this.FullName}', '{this.Bio}')", connection);
            }
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `Users` WHERE `Id` = {this.Id}", connection);
            MySqlConnection.ClearPool(connection);
        }
    }
}
