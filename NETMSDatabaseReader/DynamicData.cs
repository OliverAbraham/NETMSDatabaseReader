namespace NETMSDatabaseReader
{
    public class DynamicData
    {
        public DateTime TotalPowerLastRowTimestamp { get; set; }
        public DateTime DetailsHistoryLastRowTimestamp { get; set; }

        public DynamicData()
        {
            TotalPowerLastRowTimestamp = default(DateTime);
            DetailsHistoryLastRowTimestamp = default(DateTime);
        }
    }
}