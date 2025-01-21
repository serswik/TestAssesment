using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using TestAssesment.Helpers;
using TestAssesment.Models;

namespace TestAssesment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=TaxiTripsDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            string filePath = @"C:\Users\serge\OneDrive\Рабочий стол\Studying\TestAssesment\sample-cab-data.csv";

            DatabaseHelper.CreateDatabaseIfNotExists(connectionString);
            DatabaseHelper.CreateTableIfNotExists(connectionString);
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DatabaseHelper.CreateIndexesIfNotExist(connection);
            }

            var (uniqueTrips, duplicates) = CsvHelperUtility.ProcessDuplicatesAndSave(filePath);
            DatabaseHelper.SaveDataToDatabase(connectionString, uniqueTrips);

            Console.WriteLine("Process completed.");
        }
    }
}
