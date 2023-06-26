using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework
        // and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public void AddWalk(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Insert into Walks ([Date], Duration, WalkerId, DogId)
                                        output Inserted.Id
                                        values (@date, @duration, @walkerId, @dogId)";
                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);

                    int id = (int)cmd.ExecuteScalar();
                    walk.Id = id;
                }
            }
        }

        public List<Walk> GetAllWalks()
        {
            List<Walk> walks = new();
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select w.Id wId, o.Id as oId, o.Name as oName, [Date], Duration, WalkerId, DogId from Walks w
                                        left join Dog d
                                        on d.Id = w.DogId
                                        left join Owner o
                                        on o.Id = d.OwnerId";

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Owner _owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("oId")),
                                Name = reader.GetString(reader.GetOrdinal("oName"))
                            };
                            Walk _walk = new Walk()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerID")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Client = _owner
                            };
                            walks.Add(_walk);
                        }
                    }
                }
            }
            return walks;
        }

        public List<Walk> GetWalksByWalkerId(int id)
        {
            List<Walk> walks = new();
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select o.Id as oId, o.Name, w.Id as wId, [Date], Duration, WalkerId, DogId 
                                        from Walks w
                                        left join Dog d
                                        on w.DogId = d.Id
                                        left join Owner o
                                        on d.OwnerId = o.Id
                                        where WalkerId = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Owner _owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("oId")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            Walk _walk = new Walk()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("wId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerID")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Client = _owner
                            };
                            walks.Add(_walk);
                        }
                    }
                }
            }
            return walks;
        }
    }
}
