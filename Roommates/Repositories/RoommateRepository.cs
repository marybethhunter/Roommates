using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT roommate.FirstName, roommate.LastName, roommate.RentPortion, room.Name, room.Id, room.MaxOccupancy FROM Roommate roommate LEFT JOIN Room room ON room.Id = roommate.RoomId WHERE roommate.Id = @id";
                    cmd.Parameters.AddWithValue("id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Room room = null;
                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        room = new Room
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                        };

                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = room,
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColPos = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColPos);

                        int firstNameColPos = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColPos);

                        int lastNameColPos = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColPos);

                        int rentPortionColPos = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionColPos);

                        int moveInDateColPos = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDateColPos);

                        int roomIdColPos = reader.GetOrdinal("RoomId");
                        int roomIdValue = reader.GetInt32(roomIdColPos);

                        Room room = new Room
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                        };

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            RentPortion = rentPortionColPos,
                            MovedInDate = moveInDateValue,
                            Room = room,
                        };

                        roommates.Add(roommate);
                    }
                    reader.Close();

                    return roommates;
                }
            }
        }
    }
}
