using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Model;
using StoreAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices) {
        _categoryServices = categoryServices;
        }
        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> Get()
        {
             Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Cho phép tất cả nguồn gốc, hãy cân nhắc chỉ định nguồn gốc cụ thể
            var categories = await _categoryServices.GetAllAsynsc();
            return Ok(categories);
        }


        // GET api/Category/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var categories = await _categoryServices.GetById(id);
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        // POST api/Category
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(Categories categories)
        {
            await _categoryServices.CreateAsync(categories);
            return Ok("created successfully");

        }

        // PUT api/Category/5
        [HttpPut("put/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Categories newcategory)
        {
            //var category = await _categoryServices.GetById(id);
            //if (category == null)
            
            //    return NotFound();
            
            //await _categoryServices.UpdateAsync(id, newcategory);
            //return Ok("updates successfully");
            var existingCategory = await _categoryServices.GetById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Kiểm tra xem id trong newcategory có giống với id truyền vào không
            if (newcategory.Id != id)
            {
                // Trường Id không thể chỉnh sửa
                return BadRequest("Cannot update Id");
            }

            await _categoryServices.UpdateAsync(id, newcategory);
            return Ok("Updated successfully");
        }

        // DELETE api/Category/5
        [HttpDelete("delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (string id)
        {
            var category = await _categoryServices.GetById(id);
            if (category == null)

                return NotFound();
            await _categoryServices.DeleteAysnc(id);
            return Ok("Delete successfully");
        }
    }
}
