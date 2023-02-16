using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using store_be.DTOs;
using store_be.Models;
using store_be.Services;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace store_be.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceChangesController : ControllerBase
    {
        private readonly IPriceChangeService service;

        public PriceChangesController(IPriceChangeService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<PriceChange>>> GetPriceChanges([Optional]int productId)
        {
            if (productId == 0) //compiler considers 0 as the default value (no query param).
            {
                return await service.GetPriceChanges();
            }
            else
            {
                try
                {
                    var priceChanges = await service.GetPriceChangesById(productId);
                    return priceChanges;
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<PriceChange>> Schedule(PriceChangeDTO request)
        {
            try
            {
                var priceChange = await service.Schedule(request);
                return priceChange;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PriceChange>> UpdatePriceChange(int id, PriceChangeDTO request)
        {
            try
            {
                var priceChange = await service.UpdatePriceChange(id, request);
                return priceChange;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePriceChange(int id)
        {
            try
            {
                await service.DeletePriceChange(id);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
