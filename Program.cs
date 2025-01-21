using System.ComponentModel.DataAnnotations.Schema;
using TestAssesment.Helpers;
using TestAssesment.Models;

namespace TestAssesment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = @"C:\Users\serge\OneDrive\Рабочий стол\Studying\TestAssesment\sample-cab-data.csv";

            DatabaseHelper.CreateTable();

            var (uniqueTrips, duplicates) = CsvHelperUtility.ProcessDuplicatesAndSave(filePath);

            foreach(var trip in uniqueTrips)
            {
                DatabaseHelper.InsertRecord(trip);
            }

            int rowCount = DatabaseHelper.GetRowCount();
            Console.WriteLine($"Count of rows in TaxiTrips table: {rowCount}");

            Console.WriteLine("Process completed.");
        }
    }
}
