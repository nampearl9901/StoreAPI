using StoreAPI.Model;


namespace StoreAPI.Services
{
    public interface IBrandServices
    {
        Task<IEnumerable<Brands>> GetAllAsynsc();
        Task<Brands> GetById(string id);
        Task CreateAsync(Brands brand);

        Task UpdateAsync(string id, Brands brand);
        Task DeleteAysnc(string id);
    }
}
