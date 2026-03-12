using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class MSMEsService : IMSMEs
    {
        private readonly ApplicationDbContext _context;
        public MSMEsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(MSME MSME)
        {
            var existingEntity = _context.MSMEs.FirstOrDefault(e => e.koboEnterpriseId == MSME.koboEnterpriseId);

            if (existingEntity != null)
            {
                MSME.Id = existingEntity.Id;
                _context.Entry(existingEntity).CurrentValues.SetValues(MSME);
                _context.SaveChanges();
            }
            else
            {
                _context.MSMEs.Add(MSME);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MSME> GetAll()
        {
            _context.MSMEs.ToList();
            return _context.MSMEs;
        }

        public MSME GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MSME Update(int id, MSME MSME)
        {
            throw new NotImplementedException();
        }
    }
}
