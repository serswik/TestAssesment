﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestAssesment.Models;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;

namespace TestAssesment.Helpers
{
    public static class CsvHelper
    {
        public static List<TaxiTrip> ReadCsv(string filepath)
        {
            var trips = new List<TaxiTrip>();
            using(var reader = new StreamReader(filepath))
            using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<TaxiTrip>();
                foreach(var record in records)
                {
                    record.store_and_fwd_flag = record.store_and_fwd_flag.Trim().ToUpper() == "Y" ? "Yes" : "No";
                    record.tpep_pickup_datetime = DateTime.SpecifyKind(record.tpep_pickup_datetime, DateTimeKind.Utc);
                    record.tpep_dropoff_datetime = DateTime.SpecifyKind(record.tpep_dropoff_datetime, DateTimeKind.Utc);
                    trips.Add(record);
                }
            }
            return trips;
        }

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

        private static void WriteDuplicatesToFile(List<TaxiTrip> duplicates)
        {
            string duplicatesFilePath = @"C:\Users\serge\OneDrive\Рабочий стол\Studying\duplicates.csv";

            using(var writer = new StreamWriter(duplicatesFilePath))
            using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(duplicates);
            }
            Console.WriteLine("Duplicates were successfully saved in 'duplicates.csv' file.");
        }
    }
}
