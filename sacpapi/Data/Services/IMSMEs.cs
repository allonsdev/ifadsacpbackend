using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public interface IMSMEs
    {
        IEnumerable<MSME> GetAll();
        MSME GetById(int id);
        void Add(MSME MSME);
        MSME Update(int id, MSME MSME);
        void Delete(int id);
    }
}
