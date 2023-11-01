using DuckDuckGoose.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckDuckGoose.Repositories;

public interface IUserRepo
{
    public DuckDuckGooseUser GetUserById(string id);
}

public class UserRepo : IUserRepo
{
    private DuckDuckGooseIdentityDbContext _context;

    public UserRepo(DuckDuckGooseIdentityDbContext context)
    {
        _context = context;
    }

    public DuckDuckGooseUser GetUserById(string id)
    {
        try
        {
            return _context.Users
                .Include(u => u.Honks)
                .Include(u => u.Followers)
                .Single(u => u.Id == id);
        }
        catch (InvalidOperationException e)
        {
            throw new ArgumentOutOfRangeException($"A user was not found with id {id}", e);
        }
    }
}
