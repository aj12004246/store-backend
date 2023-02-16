using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using store_be.DTOs;
using store_be.Models;
using store_be.Services.SaleService;

namespace store_be.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController:ControllerBase
    {
        private readonly ISaleService _service;

        public SaleController(ISaleService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<List<Sale>> GetAllSales()
        {
            return await _service.GetAllSalesAsync();
        }
        [HttpPost]
        public async Task<ActionResult> AddSale(SaleDTO sale)
        {
            try
            {
                await _service.AddSaleAsync(sale);
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public async Task DeleteSale(int id)
        {
            await _service.RemoveSale(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Sale>> UpdateSale(int id, SaleDTO sale)
        {
            try
            {
                if (sale == null)
                {
                    return BadRequest("Sale is null");
                }
                var saleToChange = await _service.UpdateSale(id, sale);
                return Ok(saleToChange);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
