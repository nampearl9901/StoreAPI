namespace StoreAPI.Model
{
    public class DatabaseSettings
    {
        public string? ConnectionStrings { get; set; }
        public string? DatabaseName { get; set; }
        public string? CategoriesCollectionName { get; set; }
        public string? ProductsCollectionName { get; set; }
        public string? BrandsCollectionName { get; set; }
        public string? UserCollectionName { get; set; }
        public string? UsersCollectionName { get;set; }
    }
}
