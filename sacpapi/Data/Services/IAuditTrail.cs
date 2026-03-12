namespace sacpapi.Data.Services
{
    public interface IAuditTrail
    {
        void Log(string tableName, int recordId, string action, string oldValue, string newValue, string username);
    }
}
