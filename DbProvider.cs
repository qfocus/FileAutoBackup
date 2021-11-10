using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace AutoBackup
{
    public class DbProvider
    {
        private string addSql = @"INSERT INTO Files 
                                 (Name, Status, CreatedTime, LastModifiedTime) 
                                 VALUES
	                             ($Name, $Status, $CreatedTime, $LastModifiedTime)";

        private string getAllSql = @"SELECT Id, Name, Status, LastModifiedTime FROM Files";

        private string updateSql = @"UPDATE Files SET
                                     Status=$Status, 
                                     Name=$Name, 
                                     LastModifiedTime=$LastModifiedTime
                                     WHERE Id = $Id";

        private string getSql = @"SELECT Id, Name, Status, LastModifiedTime FROM Files WHERE Name = $Name";

        private SqliteConnection conn;

        public DbProvider()
        {
            this.conn = new SqliteConnection("Data Source=files.db");
        }

        public FileModel Get(string name)
        {
            this.conn.Open();

            var command = this.conn.CreateCommand();
            command.CommandText = getSql;
            command.Parameters.AddWithValue("$Name", name);

            SqliteDataReader reader = command.ExecuteReader();

            FileModel file = null;

            while (reader.Read())
            {
                long id = reader.GetInt64(0);

                Status status = (Status)reader.GetInt32(2);
                DateTime lastModified = reader.GetDateTime(3);
                file = new FileModel
                {
                    Id = id,
                    Name = reader.GetString(1),
                    Status = status,
                    LastModifiedTime = lastModified
                };
            }


            this.conn.Close();

            return file;
        }


        public void Add(FileModel source)
        {
            List<FileModel> files = new List<FileModel>
            {
                source
            };

            Add(files);
        }

        public void Add(List<FileModel> files)
        {
            this.conn.Open();

            foreach (var source in files)
            {
                var command = this.conn.CreateCommand();
                command.CommandText = addSql;
                command.Parameters.AddWithValue("$Name", source.Name);
                command.Parameters.AddWithValue("$Status", source.Status);
                command.Parameters.AddWithValue("$CreatedTime", DateTime.Now);
                command.Parameters.AddWithValue("$LastModifiedTime", source.LastModifiedTime);

                command.ExecuteNonQuery();
            }
            this.conn.Close();
        }

        public Dictionary<String, FileModel> Getall()
        {
            Dictionary<String, FileModel> result = new Dictionary<string, FileModel>();

            this.conn.Open();

            var command = this.conn.CreateCommand();
            command.CommandText = getAllSql;

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                long id = reader.GetInt64(0);
                string name = reader.GetString(1);
                Status status = (Status)reader.GetInt32(2);
                DateTime lastModified = reader.GetDateTime(3);
                FileModel file = new FileModel
                {
                    Id = id,
                    Name = name,
                    Status = status,
                    LastModifiedTime = lastModified
                };
                result.Add(name, file);
            }

            this.conn.Close();

            return result;
        }

        public void Update(FileModel source)
        {
            this.conn.Open();

            var command = this.conn.CreateCommand();
            command.CommandText = updateSql;
            command.Parameters.AddWithValue("$Id", source.Id);
            command.Parameters.AddWithValue("$Name", source.Name);
            command.Parameters.AddWithValue("$Status", source.Status);
            command.Parameters.AddWithValue("$LastModifiedTime", source.LastModifiedTime);

            command.ExecuteNonQuery();

            this.conn.Close();
        }

    }
}
