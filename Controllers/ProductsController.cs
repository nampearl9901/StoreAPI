using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Model;
using StoreAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAPI.Controllers
{
    
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsServices _productsServices;
        public ProductsController(IProductsServices productsServices)
        {
           _productsServices = productsServices;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var products = await _productsServices.GetAllAsynsc();
            return Ok(products);
        }

        // GET api/<ProductsController>/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Cho phép tất cả nguồn gốc, hãy cân nhắc chỉ định nguồn gốc cụ thể
            var products = await _productsServices.GetById(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // POST api/<ProductsController>
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(Products products)
        {
            await _productsServices.CreateAsync(products);
            return Ok("created successfully");
        }

        // PUT api/<ProductsController>/5
        [HttpPut("put/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Products newproducts)
        {
           

            var existingproduct = await _productsServices.GetById(id);
            if (existingproduct == null)
            {
                return NotFound();
            }

            // Kiểm tra xem id trong newcategory có giống với id truyền vào không
            if (newproducts.Id != id)
            {
                // Trường Id không thể chỉnh sửa
                return BadRequest("Cannot update Id");
            }

            await _productsServices.UpdateAsync(id,  newproducts);
            return Ok("Updated successfully");
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (string id)
        {
            var products = await _productsServices.GetById(id);
            if (products == null)

                return NotFound();
            await _productsServices.DeleteAysnc(id);
            return Ok("Delete successfully");
        }

        [HttpGet("byBrand/{brandId}")]
        public async Task<IActionResult> GetProductsByBrandId(string brandId)
        {
            var products = await _productsServices.GetProductsByBrandId(brandId);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }
      
        [HttpGet("byCategory/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryId)
        {
            var products = await _productsServices.GetProductsByCategory(categoryId);
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }



    }
}
