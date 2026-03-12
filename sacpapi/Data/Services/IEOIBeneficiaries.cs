using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public interface IEOIBeneficiaries
    {
        IEnumerable<EOIBeneficiary> GetAll();
        EOIBeneficiary GetById(int id);
        void Add(EOIBeneficiary EOIBeneficiary);
        EOIBeneficiary Update(int id, EOIBeneficiary newEOIBeneficiary);
        void Delete(int id);
    }
}
