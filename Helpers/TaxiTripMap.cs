using CsvHelper.Configuration;
using static TestAssesment.Helpers.CsvHelperUtility;

namespace TestAssesment.Helpers
{
    // This class is used to define how the fields in the CSV are mapped to the properties of the TaxiTrip model.
    // It also includes a custom date-time converter for properly parsing date-time strings in a specific format.
    public sealed class TaxiTripMap : ClassMap<Models.TaxiTrip>
    {
        public TaxiTripMap()
        {
            Map(m => m.tpep_pickup_datetime).TypeConverter<CustomDateTimeConverter>();
            Map(m => m.tpep_dropoff_datetime).TypeConverter<CustomDateTimeConverter>();
            Map(m => m.passenger_count).TypeConverter<CustomInt32Converter>();
            Map(m => m.trip_distance);
            Map(m => m.store_and_fwd_flag);
            Map(m => m.PULocationID);
            Map(m => m.DOLocationID);
            Map(m => m.fare_amount);
            Map(m => m.tip_amount);
        }
    }
}
