using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select c.Id, c.Title, c.BeanVarietyId, bv.Name, bv.Region From Coffee c Left Join BeanVariety bv on bv.Id = c.BeanVarietyId;";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var coffees = new List<Coffee>();
                        while (reader.Read())
                        {
                            var coffee = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                BeanVariety = new BeanVariety()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Region = reader.GetString(reader.GetOrdinal("Region"))
                                }
                            };

                            coffees.Add(coffee);
                        }

                        return coffees;
                    }
                }
            }
        }

        public Coffee Get(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select c.Id, c.Title, c.BeanVarietyId, bv.Name, bv.Region From Coffee c Left Join BeanVariety bv on bv.Id = c.BeanVarietyId WHERE c.Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Coffee coffee = null;
                        if (reader.Read())
                        {
                            coffee = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                BeanVariety = new BeanVariety()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Name=reader.GetString(reader.GetOrdinal("Name")),
                                    Region = reader.GetString(reader.GetOrdinal("Region"))
                                }
                            };
                           
                        }

                        return coffee;
                    }
                }
            }
        }

        public void Add(Coffee coff)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Coffee (Title, BeanVarietyId)
                        OUTPUT INSERTED.ID
                        VALUES (@title, @bvid)";
                    cmd.Parameters.AddWithValue("@title", coff.Title);
                    cmd.Parameters.AddWithValue("@bvid", coff.BeanVarietyId);
                    

                    coff.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Coffee coff)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Coffee 
                           SET Title = @title, 
                               BeanVarietyId = @bvid
                         WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", coff.Id);
                    cmd.Parameters.AddWithValue("@title", coff.Title);
                    cmd.Parameters.AddWithValue("@bvid", coff.BeanVarietyId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Coffee WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
