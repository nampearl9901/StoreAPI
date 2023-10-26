using StoreAPI.Model;


namespace StoreAPI.Services
{
    public interface IProductsServices
    {
       
        Task<IEnumerable<Products>> GetAllAsynsc();
       Task<Products> GetById(string id);
        Task CreateAsync(Products product);

        Task UpdateAsync(string id, Products products);
        Task DeleteAysnc(string id);
        Task<List<Products>> GetProductsByBrandId(string brandId);
        Task<List<Products>> GetProductsByCategory(string categoryId);
    }
}
