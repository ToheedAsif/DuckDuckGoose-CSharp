using DuckDuckGoose.Areas.Identity.Data;
using DuckDuckGoose.Models;
using DuckDuckGoose.Models.Database;
using DuckDuckGoose.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace DuckDuckGoose.Repositories;

public interface IHonkRepo
{
    public Pagination<Honk> GetHonks(GetHonksRequest request);
}

public class HonkRepo : IHonkRepo
{
    private DuckDuckGooseIdentityDbContext _context;

    public HonkRepo(DuckDuckGooseIdentityDbContext context)
    {
        _context = context;
    }

    public Pagination<Honk> GetHonks(GetHonksRequest request)
    {
        IQueryable<Honk> filteredHonks = _context.Honks
            .Include(h => h.User)
            .OrderByDescending(h => h.Timestamp);
        if (request.Search is not null)
        {
            filteredHonks = filteredHonks
                .Where(honk => honk.Content.Contains(request.Search));
        }
        
        return Pagination<Honk>.Paginate(filteredHonks, request.PageNumber ?? 1, 5);
    }
}
