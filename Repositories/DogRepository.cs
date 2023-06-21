using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework
        // and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
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
        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Insert into Dog ([Name], Breed, Notes, OwnerId, ImageUrl)
                                        Output Inserted.Id
                                        Values (@name, @breed, @notes, @ownerId, @imageUrl)";
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        public void DeleteDog(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Delete from Dog where Dog.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Dog> GetAllDogs()
        {
            List<Dog> dogs = null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select d.Id, d.Name, d.OwnerId, d.Breed, d.Notes, d.ImageUrl, o.Id as oId, o.Name as oName, o.Email, o.Address, o.Phone, o.NeighborhoodId
                                        from Dog d
                                        left join Owner o
                                        on o.Id = d.OwnerId";
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            dogs = new List<Dog>();
                        }
                        while(reader.Read())
                        {
                            Owner _owner = new Owner()
                            {
                                Name = reader.GetString(reader.GetOrdinal("oName")),
                                Id = reader.GetInt32(reader.GetOrdinal("oId")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };
                            Dog _dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = reader.GetString(reader.GetOrdinal("Notes")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Owner = _owner
                            };
                            dogs.Add(_dog);
                        }
                    }
                }
                return dogs;
            }
        }

        public Dog GetDogById(int id)
        {
            Dog _dog = null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select d.Id, d.Name, d.OwnerId, d.Breed, d.Notes, d.ImageUrl, o.Id as oId, o.Name as oName, o.Email, o.Address, o.Phone, o.NeighborhoodId
                                        from Dog d
                                        left join Owner o
                                        on o.Id = d.OwnerId
                                       Where d.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            Owner _owner = new Owner()
                            {
                                Name = reader.GetString(reader.GetOrdinal("oName")),
                                Id = reader.GetInt32(reader.GetOrdinal("oId")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };
                            _dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = reader.GetString(reader.GetOrdinal("Notes")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Owner = _owner
                            };
                        }
                    }
                }
            }
            return _dog;
        }

        public void UpdateDog(Dog dog)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Update Dog
                                        Set
                                            [Name] = @name,
                                            Breed = @breed,
                                            Notes = @notes,
                                            OwnerId = @ownerId
                                            ImageUrl = @imageUrl
                                            where Id = @id";
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    cmd.Parameters.AddWithValue("@id", dog.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
