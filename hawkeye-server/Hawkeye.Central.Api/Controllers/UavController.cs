using Hawkeye.Central.Common.ServiceResult;
using Hawkeye.Central.Common.ServiceResult.AspNetCore;
using Hawkeye.Central.Common.ServiceResult.Paging;
using Hawkeye.Central.Data;
using Hawkeye.Central.Data.Models;
using Hawkeye.Central.Services.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawkeye.Central.Api.Controllers
{
    [ApiController]
    public class UavController(HawkeyeDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken ct)
        {
            var entities = await db.Uavs
                .AsNoTracking()
                .ToListAsync(ct);

            var result = new ServicePagingResult<Uav>(entities, new PagingResult { Page = 1, PageSize = 1000, TotalCount = entities.Count });
            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var entity = await db.Uavs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id, ct);

            var result = new ServiceResult<Uav>(entity == null ? ServiceResultType.NotFound : ServiceResultType.Ok, entity);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UavRequest request, CancellationToken ct)
        {
            var entity = new Uav { Name = request.Name, Description = request.Description };
            db.Add(entity);

            await db.SaveChangesAsync(ct);

            var result = new ServiceResult<Uav>(ServiceResultType.Ok, entity);
            return result.ToActionResult();
        }
    }
}
