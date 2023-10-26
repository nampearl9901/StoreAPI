using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StoreAPI.Model;

namespace StoreAPI.Services
{
    public class BrandServices : IBrandServices
    {
        private readonly IMongoCollection<Brands> _brandservices;
        private readonly IOptions<DatabaseSettings> _dbSetting;
        public BrandServices(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSetting = dbSettings;
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionStrings);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _brandservices = mongoDatabase.GetCollection<Brands>(dbSettings.Value.BrandsCollectionName);


        }
        public async Task<IEnumerable<Brands>> GetAllAsynsc() =>
       await _brandservices.Find(_ => true).ToListAsync();


        public async Task<Brands> GetById(string id) =>
            await _brandservices.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Brands brands) =>
            await _brandservices.InsertOneAsync(brands);

        public async Task UpdateAsync(string id, Brands brands) =>
          await _brandservices
          .ReplaceOneAsync(a => a.Id == id,  brands);

        public async Task DeleteAysnc(string id) =>
            await _brandservices.DeleteOneAsync(a => a.Id == id);





    }
}
