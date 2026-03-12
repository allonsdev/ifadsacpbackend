using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class MSMERegisterEnterprisesService : IMSMERegisterEnterprises
    {
        private readonly ApplicationDbContext _context;
        public MSMERegisterEnterprisesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(MSMERegisterEnterprise MSMERegisterEnterprise)
        {
            var existingEntity = _context.MSMERegister.FirstOrDefault(e => e.koboEnterpriseId == MSMERegisterEnterprise.koboEnterpriseId);

            if (existingEntity != null)
            {
                MSMERegisterEnterprise.Id = existingEntity.Id;

                // Update existing entity
                _context.Entry(existingEntity).CurrentValues.SetValues(MSMERegisterEnterprise);
                _context.SaveChanges();
            }
            else
            {
                // Add new entity
                _context.MSMERegister.Add(MSMERegisterEnterprise);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MSMERegisterEnterprise> GetAll()
        {
            var result = _context.MSMERegister.ToList();
            return result;
        }

        public MSMERegisterEnterprise GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MSMERegisterEnterprise Update(int id, MSMERegisterEnterprise MSMERegisterEnterprise)
        {
            throw new NotImplementedException();
        }
    }
}
