namespace sacpapi.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public string? TableName { get; set; }
        public int? RecordId { get; set; }
        public string? Action { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Username { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
