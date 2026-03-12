using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public interface IActivityAttendants
    {
        IEnumerable<ActivityAttendant> GetAll();
        ActivityAttendant GetById(int id);
        void Add(ActivityAttendant ActivityAttendant);
        ActivityAttendant Update(int id, ActivityAttendant ActivityAttendant);
        void Delete(int id);
    }
}
