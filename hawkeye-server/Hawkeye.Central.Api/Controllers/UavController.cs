using Hawkeye.Central.Services;
using Hawkeye.Central.Services.Models.Request;
using Microsoft.AspNetCore.Mvc;
using TDeboutte.Common.ServiceResult.AspNetCore;

namespace Hawkeye.Central.Api.Controllers
{
    [ApiController]
    public class UavController(UavService uavService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken ct)
        {
            var result = await uavService.GetAsync(ct);
            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await uavService.GetByIdAsync(id, ct);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UavRequest request, CancellationToken ct)
        {
            var result = await uavService.CreateAsync(request, ct);
            return result.ToActionResult();
        }
    }
}
