using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework
        // and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
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
        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                List<Owner> owners = new List<Owner>();
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select ne.Id as nId, ne.Name as nName, ow.Id, ow.Name, ow.Email, ow.Address, ow.Phone, ow.NeighborhoodId from Owner ow
                                        left join Neighborhood ne
                                        on ne.Id = ow.NeighborhoodId";
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Neighborhood _neighborhood = new Neighborhood()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("nId")),
                                Name = reader.GetString(reader.GetOrdinal("nName"))
                            };
                            Owner _owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = _neighborhood
                            };
                            owners.Add(_owner);
                        }
                    }
                }
                return owners;
            }
        }

        public Owner GetOwnerById(int id)
        {
            Owner owner = null;
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select ne.Id as nId, ne.Name as nName, ow.Id, ow.Name, ow.Email, ow.Address, ow.Phone, ow.NeighborhoodId
                                        from Owner ow
                                        left join Neighborhood ne
                                        on ne.Id = ow.NeighborhoodId
                                        where ow.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            Neighborhood _neighborhood = new Neighborhood()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("nId")),
                                Name = reader.GetString(reader.GetOrdinal("nName"))
                            };
                            owner = new Owner()
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = _neighborhood
                            };
                        }
                    }
                }
            }
            return owner;
        }
    }
}
