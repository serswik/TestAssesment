using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestAssesment.Models;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Buffers.Text;
using System.Diagnostics;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAssesment.Helpers
{
    public static class CsvHelperUtility
    {
        // Reads a CSV file and maps its data to a list of TaxiTrip objects.
        public static List<TaxiTrip> ReadCsv(string filepath)
        {
            var trips = new List<TaxiTrip>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                BadDataFound = null
            };

            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<TaxiTripMap>();

                var records = csv.GetRecords<TaxiTrip>();

                foreach (var record in records)
                {
                    record.store_and_fwd_flag = record.store_and_fwd_flag?.Trim().ToUpper() == "Y" ? "Yes" : "No";
                    record.tpep_pickup_datetime = DateTime.SpecifyKind(record.tpep_pickup_datetime, DateTimeKind.Utc);
                    record.tpep_dropoff_datetime = DateTime.SpecifyKind(record.tpep_dropoff_datetime, DateTimeKind.Utc);

                    trips.Add(record);
                }
            }

            return trips;
        }
        // Processes a CSV file to separate unique trips and duplicates based on key criteria.
        public static (List<TaxiTrip> uniqueTrips, List<TaxiTrip> duplicates) ProcessDuplicatesAndSave(string filepath)
        {
            var allTrips = ReadCsv(filepath);

            var uniqueTrips = new List<TaxiTrip>();
            var duplicates = new List<TaxiTrip>();

            var seenTrips = new HashSet<string>();

            foreach(var trip in allTrips)
            {
                var tripKey = $"{trip.tpep_pickup_datetime}_{trip.tpep_dropoff_datetime}_{trip.passenger_count}";

                if(seenTrips.Contains(tripKey))
                {
                    duplicates.Add(trip);
                }
                else
                {
                    uniqueTrips.Add(trip);
                    seenTrips.Add(tripKey);
                }
            }

            WriteDuplicatesToFile(duplicates);

            return (uniqueTrips, duplicates);
        }

        // Writes duplicate trips to a separate CSV file for later review.
        private static void WriteDuplicatesToFile(List<TaxiTrip> duplicates)
        {
            string duplicatesFilePath = @"C:\Users\serge\OneDrive\Рабочий стол\Studying\TestAssesment\duplicates.csv";

            using (var writer = new StreamWriter(duplicatesFilePath))
            using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(duplicates);
            }
            Console.WriteLine("Duplicates were successfully saved in 'duplicates.csv' file.");
        }

        // Custom converter to handle null or empty integer fields in CSV data.
        public class CustomInt32Converter : CsvHelper.TypeConversion.Int32Converter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return 0;
                }

                return base.ConvertFromString(text, row, memberMapData);
            }
        }
    }
}
