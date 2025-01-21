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
        private static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=TaxiTripsDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static void CreateDatabaseIfNotExists(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TaxiTripsDb') CREATE DATABASE TaxiTripsDb;", connection);
                command.ExecuteNonQuery();
            }
        }

        public static void CreateTableIfNotExists(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TaxiTrips')
            BEGIN
                CREATE TABLE TaxiTrips (
                    tpep_pickup_datetime DATETIME,
                    tpep_dropoff_datetime DATETIME,
                    passenger_count INT,
                    trip_distance FLOAT,
                    store_and_fwd_flag NVARCHAR(10),
                    PULocationID INT,
                    DOLocationID INT,
                    fare_amount DECIMAL(18, 2),
                    tip_amount DECIMAL(18, 2)
                );
            END";

                var command = new SqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
            }
        }

        // For processing large files (e.g., 10GB), the file can be split into chunks (e.g., 100,000 rows)
        // and processed sequentially using SqlBulkCopy for each chunk to avoid loading the entire file into memory.
        public static void SaveDataToDatabase(string connectionString, List<TaxiTrip> trips)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "TaxiTrips";
                    bulkCopy.WriteToServer(GenerateDataTable(trips));
                }

                DisplayRowCount(connectionString);
            }
        }

        private static System.Data.DataTable GenerateDataTable(List<TaxiTrip> trips)
        {
            var dataTable = new System.Data.DataTable();

            dataTable.Columns.Add("tpep_pickup_datetime", typeof(DateTime));
            dataTable.Columns.Add("tpep_dropoff_datetime", typeof(DateTime));
            dataTable.Columns.Add("passenger_count", typeof(int));
            dataTable.Columns.Add("trip_distance", typeof(float));
            dataTable.Columns.Add("store_and_fwd_flag", typeof(string));
            dataTable.Columns.Add("PULocationID", typeof(int));
            dataTable.Columns.Add("DOLocationID", typeof(int));
            dataTable.Columns.Add("fare_amount", typeof(decimal));
            dataTable.Columns.Add("tip_amount", typeof(decimal));

            foreach (var trip in trips)
            {
                dataTable.Rows.Add(trip.tpep_pickup_datetime, trip.tpep_dropoff_datetime, trip.passenger_count, trip.trip_distance, trip.store_and_fwd_flag, trip.PULocationID, trip.DOLocationID, trip.fare_amount, trip.tip_amount);
            }

            return dataTable;
        }

        public static void DisplayRowCount(string connectiongString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string countQuery = "SELECT COUNT(*) FROM TaxiTrips";
                var countCommand = new SqlCommand(countQuery, connection);
                int rowCount = (int)countCommand.ExecuteScalar();

                Console.WriteLine($"Count of rows in TaxiTrips table: {rowCount}");
            }
        }

    }
}
