using sacpapi.Data;
using sacpapi.Data.Services;
using sacpapi.Models;

internal class AuditTrailService : IAuditTrail
{
    private readonly ApplicationDbContext _context;

    public AuditTrailService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Log(string tableName, int recordId, string action, string oldValue, string newValue, string username)
    {
        var auditTrail = new AuditTrail
        {
            TableName = tableName,
            RecordId = recordId,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            Username = username,
            Timestamp = DateTime.Now
        };

        _context.AuditTrail.Add(auditTrail);
        _context.SaveChanges();
    }

}