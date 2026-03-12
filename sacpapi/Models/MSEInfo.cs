namespace sacpapi.Models
{
    public class MSEInfo
    {
        public int Id { get; set; } // Primary key, usually not nullable

        public string? NameOfBusiness { get; set; }
        public string? TradingName { get; set; }
        public string? RegistrationStatus { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhysicalAddress { get; set; }
        public string? YearsOfOperation { get; set; }
        public string? OwnerSex { get; set; }
        public string? OwnerAge { get; set; }
        public string? OwnerDOB { get; set; }
        public string? ContactNo { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? GPS { get; set; }
        public string? GPSLatitude { get; set; }
        public string? GPSLongitude { get; set; }
        public string? GPSAltitude { get; set; }
        public string? GPSPrecision { get; set; }

        public int? NumberOfMales { get; set; }
        public int? NumberOfFemales { get; set; }
        public int? Total { get; set; }
        public int? MaleBeneficiaries { get; set; }
        public int? FemaleBeneficiaries { get; set; }
    }
}
