using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace WPFdatagrid.SQLiteHandler
{
    public class MyUser
    {
        public MyUser(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString() => $"User {Id}: ({Name}, {Age})";
    }

    public class SQLHandler
    {
        public SQLiteConnection sqlite_conn;
        public List<MyUser> databaseRepresentation;

        public SQLHandler() 
        {
            sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;");
            CreateConnection();
            databaseRepresentation = ReadData();
        }

        public void CreateConnection()
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex) 
            {
                Trace.TraceError(ex.Message);
            }
        }

        public void CloseConnection() 
        {
            try
            {
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public void CreateTable()
        {
            SQLiteCommand cmd;
            string command_query = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, name TEXT NOT NULL, age INTEGER NOT NULL)";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public void InsertData(int id, string name, int age) 
        {
            SQLiteCommand cmd;
            string command_query = "INSERT INTO users (id, name, age) VALUES (@id, @name, @age)";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            databaseRepresentation = ReadData();
        }

        public void InsertData(string name, int age)
        {
            SQLiteCommand cmd;
            string command_query = "INSERT INTO users (name, age) VALUES (@name, @age)";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            databaseRepresentation = ReadData();
        }
        
        public void InsertData(List<MyUser> users)
        {
            if (users.Count == 0) return;

            SQLiteCommand cmd;
            string command_query = "INSERT INTO users (id, name, age) VALUES ";

            try
            {
                
                cmd = sqlite_conn.CreateCommand();

                for (int i = 0; i < users.Count; i++)
                {
                    command_query += $"(@id{i}, @name{i}, @age{i}),";
                    cmd.Parameters.AddWithValue($"@id{i}", users[i].Id);
                    cmd.Parameters.AddWithValue($"@name{i}", users[i].Name);
                    cmd.Parameters.AddWithValue($"@age{i}", users[i].Age);
                }
                command_query = command_query.Remove(command_query.Length - 1);
                cmd.CommandText = command_query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            databaseRepresentation = ReadData();
        }

        public void UpdateData(int id, string name)
        {
            SQLiteCommand cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "UPDATE users SET name=@name WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);

            SendUpdate(cmd);
        }
        public void UpdateData(int id, int age)
        {
            SQLiteCommand cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "UPDATE users SET age=@age WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@age", age);

            SendUpdate(cmd);
        }
        public void UpdateData(int id, string name, int age)
        {
            SQLiteCommand cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "UPDATE users SET name=@name, age=@age WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@age", age);

            SendUpdate(cmd);
        }

        private void SendUpdate(SQLiteCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            databaseRepresentation = ReadData();
        }

        public void DeleteData(int id)
        {
            SQLiteCommand cmd;
            string command_query = "DELETE FROM users WHERE id=@id";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            databaseRepresentation = ReadData();
        }

        public void TruncateData()
        {
            SQLiteCommand cmd;
            string command_query = "DELETE FROM users";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            databaseRepresentation = ReadData();
        }

        public List<MyUser> ReadData()
        {
            SQLiteDataReader sqliteDataReader;
            SQLiteCommand cmd;
            List<MyUser> users = new List<MyUser>();

            string command_query = "SELECT * FROM users";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                sqliteDataReader = cmd.ExecuteReader();
                while ( sqliteDataReader.Read()) 
                {
                    MyUser user = new MyUser((int)(Int64)sqliteDataReader["id"], (string)sqliteDataReader["name"], (int)(Int64)sqliteDataReader["age"]);
                    users.Add(user);
                    Trace.WriteLine(user);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return users;
        }

        public MyUser SelectData(int id)
        {
            SQLiteDataReader sqliteDataReader;
            SQLiteCommand cmd;
            MyUser user;

            string command_query = "SELECT * FROM users WHERE id=@id";
            try
            {
                cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = command_query;
                cmd.Parameters.AddWithValue("@id", id);
                sqliteDataReader = cmd.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    user = new((int)(Int64)sqliteDataReader["id"], (string)sqliteDataReader["name"], (int)(Int64)sqliteDataReader["age"]);
                    Trace.WriteLine(user);
                    return user;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            throw new KeyNotFoundException("User not found");
        }
    }
}
