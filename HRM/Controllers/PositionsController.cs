using HRM.Repositories.Dtos.Models;
using HRM.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Apis.Controllers
{
    [Route("api/v1/positions")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionsService _positionService;
        public PositionsController(IPositionsService positionService)
        {
            _positionService = positionService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _positionService.GetAllPosition());
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNew([FromBody] PositionAdd positionAdd)
        {
            return Ok(await _positionService.AddNewPosition(positionAdd));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody]PositionUpdate positionUpdate)
        {
            return Ok(await _positionService.UpdatePosition(id, positionUpdate));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemovePosition(int id)
        {
            return Ok(await _positionService.RemovePosition(id));
        }
    }
}
