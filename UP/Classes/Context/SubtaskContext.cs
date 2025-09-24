using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UP.Classes.Common;
using UP.Interfaces;
using UP.Models;

namespace UP.Classes.Context
{
    public class SubtaskContext : Subtask, ISubtask
    {
        public List<SubtaskContext> AllSubtasks()
        {
            List<SubtaskContext> allSubtasks = new List<SubtaskContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Subtasks`", connection);

            while (data.Read())
            {
                SubtaskContext subtask = new SubtaskContext
                {
                    Id = data.GetInt32(0),
                    Name = data.GetString(1),
                    Description = data.GetString(2),
                    TaskId = data.GetInt32(3),
                    AssignedUserId = data.GetInt32(4),
                    ColumnId = data.GetInt32(5),
                    DueDate = data.GetDateTime(6),
                    CreatedAt = data.GetDateTime(7),
                    UpdatedAt = data.GetDateTime(8)
                };
                allSubtasks.Add(subtask);
            }
            Connection.CloseConnection(connection);
            return allSubtasks;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Subtasks` " +
                                    "SET " +
                                        $"`Name` = '{this.Name}', " +
                                        $"`Description` = '{this.Description}', " +
                                        $"`TaskId` = {this.TaskId}, " +
                                        $"`AssignedUserId` = {this.AssignedUserId}, " +
                                        $"`ColumnId` = {this.ColumnId}, " +
                                        $"`DueDate` = '{this.DueDate}', " +
                                        $"`CreatedAt` = '{this.CreatedAt}', " +
                                        $"`UpdatedAt` = '{this.UpdatedAt}' " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Subtasks` " +
                                    "(`Name`, `Description`, `TaskId`, `AssignedUserId`, `ColumnId`, `DueDate`, `CreatedAt`, `UpdatedAt`) " +
                                 $"VALUES ('{this.Name}', '{this.Description}', {this.TaskId}, {this.AssignedUserId}, {this.ColumnId}, '{this.DueDate}', '{this.CreatedAt}', '{this.UpdatedAt}')", connection);
            }
            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `Subtasks` WHERE `Id` = {this.Id}", connection);
            Connection.CloseConnection(connection);
        }
    }
}
