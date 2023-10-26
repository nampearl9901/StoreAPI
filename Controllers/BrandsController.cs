using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Model;
using StoreAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAPI.Controllers
{
   
    [Route("api/brands")]
    [ApiController]

    public class BrandsController : ControllerBase
    {
        private readonly IBrandServices _brands;
        public BrandsController(IBrandServices brandServices)
        {
            _brands = brandServices;
        }
        // GET: api/<BrandsController>
        [HttpGet("getbrands")]
        public async Task<IActionResult> Get()
        {
             Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Cho phép tất cả nguồn gốc, hãy cân nhắc chỉ định nguồn gốc cụ thể
            var brands = await _brands.GetAllAsynsc();
            return Ok(brands);
        }

        // GET api/<BrandsController>/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Cho phép tất cả nguồn gốc, hãy cân nhắc chỉ định nguồn gốc cụ thể
            var brands = await _brands.GetById(id);
            if (brands == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        // POST api/<BrandsController>
        [HttpPost(Name = "postbrand"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(Brands brands)
        {
            await _brands.CreateAsync(brands);
            return Ok("created successfully");
        }

        // PUT api/<BrandsController>/5
        [HttpPut("put/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Brands newbrands)
        {
            
            var existingbrands = await _brands.GetById(id);
            if (existingbrands == null)
            {
                return NotFound();
            }

            // Kiểm tra xem id trong newcategory có giống với id truyền vào không
            if (newbrands.Id != id)
            {
                // Trường Id không thể chỉnh sửa
                return BadRequest("Cannot update Id");
            }

            await _brands.UpdateAsync(id, newbrands);
            return Ok("Updated successfully");
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("Delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var brands = await _brands.GetById(id);
            if (brands == null)

                return NotFound();
            await _brands.DeleteAysnc(id);
            return Ok("Delete successfully");
        }
    }
}
