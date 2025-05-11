using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hawkeye.Central.Data;
using Hawkeye.Central.Data.Models;
using Hawkeye.Central.Services.Models.Request;
using Hawkeye.Central.Services.Models.Result;
using Microsoft.EntityFrameworkCore;
using TDeboutte.Common.ServiceResult;
using TDeboutte.Common.ServiceResult.Paging;

namespace Hawkeye.Central.Services;

public class UavService(HawkeyeDbContext db)
{
    public async Task<ServicePagingResult<UavResult>> GetAsync(CancellationToken ct)
    {
        var entities = await db.Uavs
            .AsNoTracking()
            .Select(x => ProjectToUavResult(x))
            .ToListAsync(ct);

        return new ServicePagingResult<UavResult>(entities, new PagingResult { Page = 1, PageSize = 1000, TotalCount = entities.Count });
    }

    public async Task<ServiceResult<UavResult>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await db.Uavs
            .AsNoTracking()
            .Select(x => ProjectToUavResult(x))
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        return new ServiceResult<UavResult>(entity == null ? ServiceResultType.NotFound : ServiceResultType.Ok, entity);
    }

    public async Task<ServiceResult<UavResult>> CreateAsync(UavRequest request, CancellationToken ct)
    {
        var entity = new Uav { Name = request.Name, Description = request.Description };
        db.Add(entity);

        await db.SaveChangesAsync(ct);
        
        return await GetByIdAsync(entity.Id, ct);
    }

    private static UavResult ProjectToUavResult(Uav x) => new UavResult
    {
        Id = x.Id,
        Name = x.Name,
        Description = x.Description,
    };
}