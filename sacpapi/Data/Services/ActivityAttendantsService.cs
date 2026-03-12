using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class ActivityAttendantsService : IActivityAttendants
    {
        private readonly ApplicationDbContext _context;
        public ActivityAttendantsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(ActivityAttendant ActivityAttendant)
        {
            _context.ActivityAttendants.Add(ActivityAttendant);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ActivityAttendant> GetAll()
        {
            throw new NotImplementedException();
        }

        public ActivityAttendant GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ActivityAttendant Update(int id, ActivityAttendant ActivityAttendant)
        {
            throw new NotImplementedException();
        }
    }
}
