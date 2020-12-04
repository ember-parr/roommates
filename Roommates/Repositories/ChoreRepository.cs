using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System;


namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        // gets a chore by it's id number
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();

                    return chore;
                }
            }
        }

        // adds a new chore to the database
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        // assigns chores to a roommate & gives confirmation message
        public void AssignChore(int RoommateId, int ChoreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" INSERT INTO RoommateChore (RoommateId, ChoreId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@RoommateId, @ChoreId)";
                    cmd.Parameters.AddWithValue("@RoommateId", RoommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", ChoreId);
                    int id = (int)cmd.ExecuteScalar();

                    Chore thisChore = GetById(id);
                    Console.WriteLine($"{thisChore.Name} has been assigned.");
                }
            }

        }

        // gathers all chores that are not yet assigned to a roommate
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Chore.Id, Name FROM Chore FULL OUTER JOIN RoommateChore rc ON Chore.Id = rc.ChoreId WHERE RoommateId IS NULL";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);
                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        chores.Add(chore);

                    }
                    reader.Close();
                    return chores;
                }
            }
        }


        public List<string> GetChoreCounts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT COUNT (DISTINCT rc.ChoreId) as chores, r.FirstName
                                        FROM RoommateChore rc
                                        JOIN Chore c ON rc.ChoreId = c.Id
                                        JOIN Roommate r ON rc.RoommateId = r.Id
                                        GROUP BY r.FirstName";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<string> choreCounts = new List<string>();

                    while (reader.Read())
                    {
                        int roommateColumnPosition = reader.GetOrdinal("FirstName");
                        string roommateValue = reader.GetString(roommateColumnPosition);
                        int choreColumnPosition = reader.GetOrdinal("chores");
                        int choreValue = reader.GetInt32(choreColumnPosition);

                        if (choreValue == 1)
                        {
                            string choreCount = $"- {roommateValue} is assigned {choreValue} chore";
                            choreCounts.Add(choreCount);
                        } else
                        {
                            string choreCount = $"- {roommateValue} is assigned {choreValue} chores";
                            choreCounts.Add(choreCount);
                        }
                        

                    }

                    reader.Close();
                    return choreCounts;
                }
            }
        }






    }
}