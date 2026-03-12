using sacpapi.Data;
using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class FieldRegisterBeneficiariesService : IFieldRegisterBeneficiaries
    {
        private readonly ApplicationDbContext _context;
        public FieldRegisterBeneficiariesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(FieldRegisterBeneficiary FieldRegisterBeneficiary)
        {
            var existingEntity = _context.FieldRegister.FirstOrDefault(e => e.KoboBeneficiaryId == FieldRegisterBeneficiary.KoboBeneficiaryId);

            if (existingEntity != null)
            {
                FieldRegisterBeneficiary.Id = existingEntity.Id;

                // Update existing entity
                _context.Entry(existingEntity).CurrentValues.SetValues(FieldRegisterBeneficiary);
                _context.SaveChanges();
            }
            else
            {
                // Add new entity
                _context.FieldRegister.Add(FieldRegisterBeneficiary);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FieldRegisterBeneficiary> GetAll()
        {
            var result = _context.FieldRegister.ToList();
            return result;
        }

        public FieldRegisterBeneficiary GetById(int id)
        {
            throw new NotImplementedException();
        }

        public FieldRegisterBeneficiary Update(int id, FieldRegisterBeneficiary newFieldRegisterBeneficiary)
        {
            throw new NotImplementedException();
        }
    }
}
