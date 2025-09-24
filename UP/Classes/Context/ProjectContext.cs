using MySql.Data.MySqlClient;
using System.Collections.Generic;
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
            MySqlDataReader data = Connection.Query("SELECT * FROM `Projects`", connection);

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
            Connection.CloseConnection(connection);
            return allProjects;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Projects` " +
                                    "SET " +
                                        $"`Name` = '{this.Name}', " +
                                        $"`Description` = '{this.Description}', " +
                                        $"`IsPublic` = {(this.IsPublic ? 1 : 0)}, " +
                                        $"`CreatorId` = {this.CreatorId}, " +
                                        $"`CreatedAt` = '{this.CreatedAt}', " +
                                        $"`UpdateAt` = '{this.UpdateAt}' " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Projects` " +
                                    "(`Name`, `Description`, `IsPublic`, `CreatorId`, `CreatedAt`, `UpdateAt`) " +
                                 $"VALUES ('{this.Name}', '{this.Description}', {(this.IsPublic ? 1 : 0)}, {this.CreatorId}, '{this.CreatedAt}', '{this.UpdateAt}')", connection);
            }
            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `Projects` WHERE `Id` = {this.Id}", connection);
            Connection.CloseConnection(connection);
        }
    }
}
