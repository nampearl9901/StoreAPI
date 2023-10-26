using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StoreAPI.Services;
using StoreAPI.Model;


namespace StoreAPI.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IMongoCollection<Categories> _categories;
        private readonly IOptions<DatabaseSettings> _dbSetting;
        public CategoryServices(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSetting = dbSettings;
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionStrings);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _categories = mongoDatabase.GetCollection<Categories>(dbSettings.Value.CategoriesCollectionName);

        }
        //

        public async Task<IEnumerable<Categories>> GetAllAsynsc() =>
        await _categories.Find(_ => true).ToListAsync();

        public async Task<Categories> GetById(string id) =>
            await _categories.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Categories category) =>
            await _categories.InsertOneAsync(category);

        public async Task UpdateAsync(string id, Categories categories) =>
          await _categories
          .ReplaceOneAsync(a => a.Id == id, categories);

        public async Task DeleteAysnc(string id) =>
            await _categories.DeleteOneAsync(a => a.Id == id);



    }

}


