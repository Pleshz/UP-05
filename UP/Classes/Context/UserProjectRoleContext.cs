using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UP.Classes.Common;
using UP.Interfaces;
using UP.Models;

namespace UP.Classes.Context
{
    public class UserProjectRoleContext : UserProjectRole, IUserProjectRole
    {
        public List<UserProjectRoleContext> AllUserProjectRoles()
        {
            List<UserProjectRoleContext> allRoles = new List<UserProjectRoleContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `UserProjectRoles`", connection);

            while (data.Read())
            {
                UserProjectRoleContext role = new UserProjectRoleContext
                {
                    Id = data.GetInt32(0),
                    UserId = data.GetInt32(1),
                    ProjectId = data.GetInt32(2),
                    Role = data.GetString(3)
                };
                allRoles.Add(role);
            }
            Connection.CloseConnection(connection);
            return allRoles;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `UserProjectRoles` " +
                                    "SET " +
                                        $"`UserId` = {this.UserId}, " +
                                        $"`ProjectId` = {this.ProjectId}, " +
                                        $"`Role` = '{this.Role}' " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `UserProjectRoles` " +
                                    "(`UserId`, `ProjectId`, `Role`) " +
                                 $"VALUES ({this.UserId}, {this.ProjectId}, '{this.Role}')", connection);
            }
            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `UserProjectRoles` WHERE `Id` = {this.Id}", connection);
            Connection.CloseConnection(connection);
        }
    }
}
