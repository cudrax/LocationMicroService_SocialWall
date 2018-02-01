using LocationMicroService_SocialWall.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using URISUtil.DataAccess;
using URISUtil.Logging;
using URISUtil.Response;

namespace LocationMicroService_SocialWall.DataAccess
{
    public class LocationDB
    {

        private static Location ReadRow(SqlDataReader reader)
        {
            Location retVal = new Location();

            retVal.Id = (int)reader["Id"];
            retVal.Longitude = (decimal)reader["Longitude"];
            retVal.Latitude = (decimal)reader["Latitude"];
            retVal.Active = (bool)reader["Active"];

            return retVal;
        }

        private static int ReadId(SqlDataReader reader)
        {
            return (int)reader["Id"];
        }

        private static string AllColumnSelect
        {
            get
            {
                return @"
                    [Location].[Id],
                    [Location].[Longitude],
	                [Location].[Latitude],
	                [Location].[Active]
                ";
            }
        }

        private static void FillData(SqlCommand command, Location location)
        {
            command.AddParameter("@Id", SqlDbType.Int, location.Id);
            command.AddParameter("@Longitude", SqlDbType.Decimal, location.Latitude);
            command.AddParameter("@Latitude", SqlDbType.Decimal, location.Latitude);
            command.AddParameter("@Active", SqlDbType.Bit, location.Active);
        }

        public static List<Location> GetAllLocations(ActiveStatusEnum active)
        {
            try
            {
                List<Location> retVal = new List<Location>();

                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        SELECT {0} FROM 
                        [dbo].[Location] 
                        WHERE @Active IS NULL OR [dbo].[Location].active = @Active
                    ", AllColumnSelect);
                    command.Parameters.Add("@Active", SqlDbType.Bit);
                    switch (active)
                    {
                        case ActiveStatusEnum.Active:
                            command.Parameters["@Active"].Value = true;
                            break;
                        case ActiveStatusEnum.Inactive:
                            command.Parameters["@Active"].Value = false;
                            break;
                        case ActiveStatusEnum.All:
                            command.Parameters["@Active"].Value = DBNull.Value;
                            break;
                    }
                    System.Diagnostics.Debug.WriteLine(command.CommandText);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retVal.Add(ReadRow(reader));
                        }
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static Location GetLocation(int id)
        {
            try
            {
                Location retVal = new Location();

                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        SELECT {0} FROM 
                        [dbo].[Location] 
                        WHERE [dbo].[Location].id = @Id
                    ", AllColumnSelect);

                    command.AddParameter("@Id", SqlDbType.Int, id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retVal = ReadRow(reader);
                        }
                    }

                }
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static Location InsertLocation(Location location)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        INSERT INTO [dbo].[Location] 
                        (
                            [Longitude],
                            [Latitude],
                            [Active]
                        )
                        VALUES 
                        (
                            @Longitude,
                            @Latitude,
                            @Active
                        )
                    ");
                    FillData(command, location);
                    connection.Open();
                    int id = 0; 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = ReadId(reader);
                        }
                    }
                    return GetLocation(id);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex); ;
            }
        }

        public static Location UpdateLocation(Location location, int id)
        {
            try
            {
                using (SqlConnection connenction = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connenction.CreateCommand();
                    command.CommandText = String.Format(@"
                        UPDATE [dbo].[Location] 
                        SET 
                        [Longitude] = @Longitude  
                        AND
                        [Latitude] = @Latitude
                        WHERE [id] = @Id;
                    ");
                    command.AddParameter("@Id", SqlDbType.Int, id);
                    command.AddParameter("@Longitude", SqlDbType.Decimal, location.Longitude);
                    command.AddParameter("@Latitude", SqlDbType.Decimal, location.Latitude);
                    connenction.Open();
                    command.ExecuteNonQuery();
                }
                return GetLocation(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static void DeleteLocation(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        UPDATE [dbo].[Location] 
                        SET [Active] = 0 
                        WHERE [Id] = @Id;
                    ");
                    command.AddParameter("@Id", SqlDbType.Int, id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}