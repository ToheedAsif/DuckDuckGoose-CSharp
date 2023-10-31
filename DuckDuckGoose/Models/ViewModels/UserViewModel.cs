using DuckDuckGoose.Areas.Identity.Data;

namespace DuckDuckGoose.Models.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public IEnumerable<UserViewModelUser> Followers { get; set; }
    public Pagination<HonkViewModel> Honks { get; set; }

    public UserViewModel(DuckDuckGooseUser user, int page, int perPage)
    {
        Id = user.Id;
        UserName = user.UserName;
        Followers = user.Followers != null ? user.Followers.Select(u => new UserViewModelUser(u)) : Enumerable.Empty<UserViewModelUser>();
        Honks = Pagination<HonkViewModel>.Paginate(user.Honks.Select(h => new HonkViewModel(h)), page, perPage);
    }
    
    public string? Filter { get; set; }
    public string? Search { get; set; }
}

public class UserViewModelUser
{
    public string Id { get; set; }
    public string UserName { get; set; }

    public UserViewModelUser(DuckDuckGooseUser user)
    {
        Id = user.Id;
        UserName = user.UserName;
    }
}
