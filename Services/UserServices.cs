//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StoreAPI.Model;


namespace StoreAPI.Services
{
    public class UserServices : IUservices
    {
       
        private readonly IMongoCollection<User> _userServices;
        private readonly IOptions<DatabaseSettings> _dbSetting;
        public UserServices(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSetting = dbSettings;
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionStrings);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _userServices = mongoDatabase.GetCollection<User>(dbSettings.Value.UsersCollectionName);

        }
        public async Task<IEnumerable<User>> GetAllAsynsc() =>
           await _userServices.Find(_ => true).ToListAsync();

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var user = await _userServices.Find(u => u.Email == email).FirstOrDefaultAsync();
            return user != null; // Trả về true nếu tìm thấy người dùng, ngược lại trả về false.
        }
        public async Task<User> GetUserByEmailAsync(string email) =>
            // Sử dụng Entity Framework Core để truy vấn người dùng dựa trên email
            await _userServices.Find(u => u.Email == email).FirstOrDefaultAsync();
        public async Task<User> GetByEmailAsync(string email) =>
          await _userServices.Find(u => u.Email == email).SingleOrDefaultAsync();
        public async Task CreateAsync(User user) =>
             await _userServices.InsertOneAsync(user);
       

         

    }
}
