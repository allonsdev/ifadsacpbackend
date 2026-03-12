using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class EOIBeneficiariesService : IEOIBeneficiaries
    {
        private readonly ApplicationDbContext _context;
        public EOIBeneficiariesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(EOIBeneficiary EOIBeneficiary)
        {
            var existingEntity = _context.EOIBeneficiaries.FirstOrDefault(e => e.KoboBeneficiaryId == EOIBeneficiary.KoboBeneficiaryId);

            if (existingEntity != null)
            {
                EOIBeneficiary.Id = existingEntity.Id;
                _context.Entry(existingEntity).CurrentValues.SetValues(EOIBeneficiary);
                _context.SaveChanges();
            }
            else
            {
                _context.EOIBeneficiaries.Add(EOIBeneficiary);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EOIBeneficiary> GetAll()
        {
            var result = _context.EOIBeneficiaries.ToList();
            return result;
        }

        public EOIBeneficiary GetById(int id)
        {
            throw new NotImplementedException();
        }

        public EOIBeneficiary Update(int id, EOIBeneficiary newEOIBeneficiary)
        {
            throw new NotImplementedException();
        }
    }
}
