using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class BeneficiariesService : IBeneficiaries
    {
        private readonly ApplicationDbContext _context;
        public BeneficiariesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Beneficiary Beneficiary)
        {
            var existingEntity = _context.Beneficiaries.FirstOrDefault(e => e.KoboBeneficiaryId == Beneficiary.KoboBeneficiaryId);

            if (existingEntity != null)
            {
                Beneficiary.Id = existingEntity.Id;

                // Update existing entity
                _context.Entry(existingEntity).CurrentValues.SetValues(Beneficiary);
                _context.SaveChanges();
            }
            else
            {
                // Add new entity
                _context.Beneficiaries.Add(Beneficiary);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Beneficiary> GetAll()
        {
            return _context.Beneficiaries.ToList();
        }

        public Beneficiary GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Beneficiary Update(int id, Beneficiary Beneficiary)
        {
            throw new NotImplementedException();
        }
    }


}
