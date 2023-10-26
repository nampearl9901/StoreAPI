using StoreAPI.Model;


namespace StoreAPI.Services
{
    public interface IUservices
    {
        
        Task<IEnumerable<User>> GetAllAsynsc();
        Task CreateAsync(User user);
       
        Task<bool> IsEmailExistsAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetByEmailAsync(string email);

    }
}
