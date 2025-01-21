using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TestAssesment.Models;

namespace TestAssesment.Helpers
{
    public static class DatabaseHelper
    {
        private static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=DotNetStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static void CreateTable()
        {
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TaxiTrips' AND xtype='U')
                CREATE TABLE TaxiTrips (
                    tpep_pickup_datetime DATETIME NOT NULL,
                    tpep_dropoff_datetime DATETIME NOT NULL,
                    passenger_count INT NOT NULL,
                    trip_distance FLOAT NOT NULL,
                    store_and_fwd_flag NVARCHAR(3) NOT NULL,
                    PULocationID INT NOT NULL,
                    DOLocationID INT NOT NULL,
                    fare_amount DECIMAL(10, 2) NOT NULL,
                    tip_amount DECIMAL(10, 2) NOT NULL
                )";

                using (var command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

            }
        }

        // For large files, we would have to use SqlBulkCopy for bulk inserts instead of inserting one record at a time
        public static void InsertRecord(TaxiTrip trip)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"
                INSERT INTO TaxiTrips (
                    tpep_pickup_datetime, tpep_dropoff_datetime, passenger_count, 
                    trip_distance, store_and_fwd_flag, PULocationID, DOLocationID, 
                    fare_amount, tip_amount
                ) VALUES (
                    @Pickup, @Dropoff, @PassengerCount, @TripDistance, @StoreFlag, 
                    @PULocationID, @DOLocationID, @FareAmount, @TipAmount
                )";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Pickup", trip.tpep_pickup_datetime);
                    command.Parameters.AddWithValue("@Dropoff", trip.tpep_dropoff_datetime);
                    command.Parameters.AddWithValue("@PassengerCount", trip.passenger_count);
                    command.Parameters.AddWithValue("@TripDistance", trip.trip_distance);
                    command.Parameters.AddWithValue("@StoreFlag", trip.store_and_fwd_flag);
                    command.Parameters.AddWithValue("@PULocationID", trip.PULocationID);
                    command.Parameters.AddWithValue("@DOLocationID", trip.DOLocationID);
                    command.Parameters.AddWithValue("@FareAmount", trip.fare_amount);
                    command.Parameters.AddWithValue("@TipAmount", trip.tip_amount);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetRowCount()
        {
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string countQuery = "SELECT COUNT(*) FROM TaxiTrips";

                using (var command = new SqlCommand(countQuery, connection))
                {
                    var rowCount = (int)command.ExecuteScalar();
                    return rowCount;
                }
            }
        }
    }
}
