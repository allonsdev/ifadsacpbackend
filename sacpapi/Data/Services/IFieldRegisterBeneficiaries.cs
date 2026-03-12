using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public interface IFieldRegisterBeneficiaries
    {
        IEnumerable<FieldRegisterBeneficiary> GetAll();
        FieldRegisterBeneficiary GetById(int id);
        void Add(FieldRegisterBeneficiary FieldRegisterBeneficiary);
        FieldRegisterBeneficiary Update(int id, FieldRegisterBeneficiary newFieldRegisterBeneficiary);
        void Delete(int id);
    }
}
