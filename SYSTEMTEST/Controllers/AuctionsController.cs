using Microsoft.AspNetCore.Mvc;
using SYSTEMTEST.DTOs;
using SYSTEMTEST.Services.Interfaces;

namespace SYSTEMTEST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _service;

        public AuctionsController(
            IAuctionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var auction =
                await _service.GetByIdAsync(id);

            if (auction == null)
                return NotFound();

            return Ok(auction);
        }

        [HttpPost("{id}/bid")]
        public async Task<IActionResult> PlaceBid(int id, CreateBidDto dto)
        {
            await _service.PlaceBidAsync(id, dto.Amount, dto.RowVersion);
            return Ok();
        }

    }
}

