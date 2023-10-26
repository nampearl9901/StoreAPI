using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StoreAPI.Model;


namespace StoreAPI.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly IMongoCollection<Products> _products;
        private readonly IOptions<DatabaseSettings> _dbSetting;
        public ProductsServices(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSetting = dbSettings;
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionStrings);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _products = mongoDatabase.GetCollection<Products>(dbSettings.Value.ProductsCollectionName);

        }
        //
        public async Task<IEnumerable<Products>> GetAllAsynsc() =>
            await _products.Find(_ => true).ToListAsync();


        public async Task<Products> GetById(string id) =>
            await _products.Find(a => a.Id == id).FirstOrDefaultAsync(); 

        public async Task CreateAsync(Products product) =>
            await _products.InsertOneAsync(product);

        public async Task UpdateAsync(string id, Products products) =>
          await _products
          .ReplaceOneAsync(a => a.Id == id, products);

        public async Task DeleteAysnc(string id) =>
            await _products.DeleteOneAsync(a => a.Id == id);

        //bybrnand
        public async Task<List<Products>> GetProductsByBrandId(string brandId)
        {
            var filter = Builders<Products>.Filter.Eq(p => p.BrandId, brandId);
            return await _products.Find(filter).ToListAsync();
        }
        //bycategory
        public async Task<List<Products>> GetProductsByCategory(string categoryId)
        {
            var filter = Builders<Products>.Filter.Eq(p => p.CategoryId, categoryId);
            return await _products.Find(filter).ToListAsync();
        }
    }
}
