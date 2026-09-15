using IMS.API.DTOs;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : AuthenticatedController
    {
        private readonly ICrudService<StoreInfo> _storeInfo;

        public StoreController(
            ICrudService<StoreInfo> storeInfo)
        {
            _storeInfo = storeInfo;
        }

        [HttpGet("stores")]
        public async Task<IActionResult> GetStore()
        {
            var stores = await _storeInfo.GetAllAsync();

            return Ok(new Response
            {
                Success = true,
                Message = "Stores retrieved successfully.",
                Data = stores
            });
        }

        [HttpPost("stores")]
        public async Task<IActionResult> CreateStore(StoreCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    Success = false,
                    Message = "Invalid store information."
                });
            }

            var store = new StoreInfo
            {
                StoreName = request.StoreName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                RegistrationNo = request.RegistrationNo,
                PanNo = request.PanNo,
                IsActive = request.IsActive,
                CreatedBy = "React",
                CreatedDate = DateTime.Now
            };

            try
            {


                await _storeInfo.InsertAsync(store);
            }
            catch (Exception)
            {
                return Ok(new Response
                {
                    Success = true,
                    Message = "Failed to created store.",
                });
            }
            return Ok(new Response
            {
                Success = true,
                Message = "Store created successfully.",
                Data = store
            });
        }
        [HttpPut("stores/{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreInfo store)
        {
            try
            {
                var existingStore = await _storeInfo.GetAsync(id);

                if (existingStore == null)
                {
                    return NotFound(new Response
                    {
                        Success = false,
                        Message = "Store not found."
                    });
                }

                existingStore.StoreName = store.StoreName;
                existingStore.Address = store.Address;
                existingStore.PhoneNumber = store.PhoneNumber;
                existingStore.RegistrationNo = store.RegistrationNo;
                existingStore.PanNo = store.PanNo;
                existingStore.IsActive = store.IsActive;
                existingStore.ModifiedDate = DateTime.Now;
                existingStore.ModifiedBy = "React";

                await _storeInfo.UpdateAsync(existingStore);

                return Ok(new Response
                {
                    Success = true,
                    Message = "Store updated successfully.",
                    Data = existingStore
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("stores/{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                var store = await _storeInfo.GetAsync(id);

                if (store == null)
                {
                    return NotFound(new Response
                    {
                        Success = false,
                        Message = "Store not found."
                    });
                }

                _storeInfo.Delete(store);

                return Ok(new Response
                {
                    Success = true,
                    Message = "Store Deleted successfully.",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
