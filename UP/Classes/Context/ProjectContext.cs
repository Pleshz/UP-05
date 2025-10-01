using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UP.Classes.Common;
using UP.Interfaces;
using UP.Models;

namespace UP.Classes.Context
{
    public class ProjectContext : Project, IProject
    {
        public List<ProjectContext> AllProjects()
        {
            List<ProjectContext> allProjects = new List<ProjectContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = null;
            try
            {
                data = Connection.Query("SELECT * FROM `Projects`", connection);
                while (data.Read())
                {
                    ProjectContext project = new ProjectContext
                    {
                        Id = data.GetInt32(0),
                        Name = data.GetString(1),
                        Description = data.GetString(2),
                        IsPublic = data.GetBoolean(3),
                        CreatorId = data.GetInt32(4),
                        CreatedAt = data.GetDateTime(5),
                        UpdateAt = data.GetDateTime(6)
                    };
                    allProjects.Add(project);
                }
            }
            finally
            {
                if (data != null) data.Close();
                Connection.CloseConnection(connection);
            }
            return allProjects;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            MySqlCommand cmd = null;
            try
            {
                if (Update)
                {
                    string query = "UPDATE `Projects` SET " +
                                   "`Name` = @Name, " +
                                   "`Description` = @Description, " +
                                   "`IsPublic` = @IsPublic, " +
                                   "`CreatorId` = @CreatorId, " +
                                   "`CreatedAt` = @CreatedAt, " +
                                   "`UpdateAt` = @UpdateAt " +
                                   "WHERE `Id` = @Id";
                    cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", this.Name);
                    cmd.Parameters.AddWithValue("@Description", this.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPublic", this.IsPublic);
                    cmd.Parameters.AddWithValue("@CreatorId", this.CreatorId);
                    cmd.Parameters.AddWithValue("@CreatedAt", this.CreatedAt);
                    cmd.Parameters.AddWithValue("@UpdateAt", this.UpdateAt);
                    cmd.Parameters.AddWithValue("@Id", this.Id);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string query = "INSERT INTO `Projects` " +
                                   "(`Name`, `Description`, `IsPublic`, `CreatorId`, `CreatedAt`, `UpdateAt`) " +
                                   "VALUES (@Name, @Description, @IsPublic, @CreatorId, @CreatedAt, @UpdateAt)";
                    cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", this.Name);
                    cmd.Parameters.AddWithValue("@Description", this.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPublic", this.IsPublic);
                    cmd.Parameters.AddWithValue("@CreatorId", this.CreatorId);
                    cmd.Parameters.AddWithValue("@CreatedAt", this.CreatedAt);
                    cmd.Parameters.AddWithValue("@UpdateAt", this.UpdateAt);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT LAST_INSERT_ID();";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Сохранение роли создателя
                if (!Update)
                {
                    var role = new UserProjectRoleContext
                    {
                        UserId = this.CreatorId,
                        ProjectId = this.Id,
                        RoleEnum = ProjectRole.Creator
                    };
                    role.Save(false); // Вызов метода Save другого контекста
                }
            }
            catch (Exception ex)
            {
                throw ex; // Прокидываем исключение для обработки выше
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
                var columnContext = new ColumnContext();
                var columns = columnContext.AllColumns().Where(c => c.ProjectId == this.Id).ToList();

                foreach (var col in columns)
                {
                    col.Delete();
                }

                var roleContext = new UserProjectRoleContext();
                var roles = roleContext.AllUserProjectRoles().Where(r => r.ProjectId == this.Id).ToList();
                foreach (var role in roles)
                {
                    role.Delete();
                }

                string query = "DELETE FROM `Projects` WHERE `Id` = @Id";
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
