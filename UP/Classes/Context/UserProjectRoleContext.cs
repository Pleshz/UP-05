using MySql.Data.MySqlClient;
using System;
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
            MySqlDataReader data = null;
            try
            {
                data = Connection.Query("SELECT * FROM `UserProjectRoles`", connection);
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
            }
            finally
            {
                if (data != null) data.Close();
                Connection.CloseConnection(connection);
            }
            return allRoles;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            MySqlCommand cmd = null;
            try
            {
                if (Update)
                {
                    string query = "UPDATE `UserProjectRoles` SET " +
                                   "`UserId` = @UserId, " +
                                   "`ProjectId` = @ProjectId, " +
                                   "`Role` = @Role " +
                                   "WHERE `Id` = @Id";
                    cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", this.UserId);
                    cmd.Parameters.AddWithValue("@ProjectId", this.ProjectId);
                    cmd.Parameters.AddWithValue("@Role", this.Role);
                    cmd.Parameters.AddWithValue("@Id", this.Id);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string query = "INSERT INTO `UserProjectRoles` " +
                                   "(`UserId`, `ProjectId`, `Role`) " +
                                   "VALUES (@UserId, @ProjectId, @Role)";
                    cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", this.UserId);
                    cmd.Parameters.AddWithValue("@ProjectId", this.ProjectId);
                    cmd.Parameters.AddWithValue("@Role", this.Role);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                Connection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            try
            {
                string query = "DELETE FROM `UserProjectRoles` WHERE `Id` = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", this.Id);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                Connection.CloseConnection(connection);
            }
        }
    }
}
