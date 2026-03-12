using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public interface IBeneficiaries
    {
        IEnumerable<Beneficiary> GetAll();
        Beneficiary GetById(int id);
        void Add(Beneficiary Beneficiary);
        Beneficiary Update(int id, Beneficiary Beneficiary);
        void Delete(int id);
    }
}
