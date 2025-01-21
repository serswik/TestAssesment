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

            // Note: To optimize and improve the program in the future, you can implement a mechanism to avoid adding duplicate rows
            // when running the program multiple times. For instance, by checking a hash of the original file or comparing the data
            // in the database with the file content. If there are no changes in the source file, simply display the row count
            // in the database without adding new rows.

            var (uniqueTrips, duplicates) = CsvHelperUtility.ProcessDuplicatesAndSave(filePath);
            DatabaseHelper.SaveDataToDatabase(connectionString, uniqueTrips);

            Console.WriteLine("Process completed.");
        }
    }
}
