using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UP.Classes.Common;
using UP.Interfaces;

namespace UP.Classes.Context
{
    public class TaskContext : Models.Task, ITask
    {
        public List<TaskContext> AllTasks()
        {
            List<TaskContext> allTasks = new List<TaskContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Tasks`", connection);

            while (data.Read())
            {
                TaskContext task = new TaskContext
                {
                    Id = data.GetInt32(0),
                    Title = data.GetString(1),
                    Description = data.GetString(2),
                    ColumnId = data.GetInt32(3),
                    ProjectId = data.GetInt32(4),
                    CreatorId = data.GetInt32(5),
                    CreatedAt = data.GetDateTime(6),
                    UpdatedAt = data.GetDateTime(7),
                    DueDate = data.IsDBNull(8) ? (DateTime?)null : data.GetDateTime(8)
                };
                allTasks.Add(task);
            }
            Connection.CloseConnection(connection);
            return allTasks;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Tasks` " +
                                    "SET " +
                                        $"`Title` = '{this.Title}', " +
                                        $"`Description` = '{this.Description}', " +
                                        $"`ColumnId` = {this.ColumnId}, " +
                                        $"`ProjectId` = {this.ProjectId}, " +
                                        $"`CreatorId` = {this.CreatorId}, " +
                                        $"`UpdatedAt` = '{this.UpdatedAt.ToString("yyyy-MM-dd HH:MM:ss")}', " +
                                        $"`DueDate` = {(this.DueDate.HasValue ? $"'{this.DueDate.Value.ToString("yyyy-MM-dd HH:MM:ss")}'" : "NULL")} " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Tasks` " +
                                    "(`Title`, `Description`, `ColumnId`, `ProjectId`, `CreatorId`, `CreatedAt`, `UpdatedAt`, `DueDate`) " +
                                 $"VALUES ('{this.Title}', '{this.Description}', {this.ColumnId}, {this.ProjectId}, {this.CreatorId}, '{this.CreatedAt.ToString("yyyy-MM-dd HH:MM:ss")}', '{this.UpdatedAt.ToString("yyyy-MM-dd HH:MM:ss")}', {(this.DueDate.HasValue ? $"'{this.DueDate.Value.ToString("yyyy-MM-dd HH:MM:ss")}'" : "NULL")})", connection);
            }
            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();

            try
            {
                var subtaskContext = new SubtaskContext();
                var subtasks = subtaskContext.AllSubtasks().Where(s => s.TaskId == this.Id).ToList();

                foreach (var sub in subtasks)
                {
                    sub.Delete();
                }

                Connection.Query($"DELETE FROM `Tasks` WHERE `Id` = {this.Id}", connection);
            }
            finally
            {
                Connection.CloseConnection(connection);
            }
        }
    }
}
