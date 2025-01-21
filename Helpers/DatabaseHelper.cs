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
        private static string connectionString = "C:\\Users\\serge\\OneDrive\\Рабочий стол\\Studying\\sample-cab-data";

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
    }
}
