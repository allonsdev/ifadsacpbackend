using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public interface IMSMERegisterEnterprises
    {
        IEnumerable<MSMERegisterEnterprise> GetAll();
        MSMERegisterEnterprise GetById(int id);
        void Add(MSMERegisterEnterprise MSMERegisterEnterprise);
        MSMERegisterEnterprise Update(int id, MSMERegisterEnterprise MSMERegisterEnterprise);
        void Delete(int id);
    }
}
