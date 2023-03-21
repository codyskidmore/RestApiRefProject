namespace Movies.Api.Contracts.Requests;

public class GetAllMoviesRequest //: PagedRequest
{
    public string? Title { get; init; }
    public int? Year { get; init; }
    public string? SortBy { get; init; }

    // Workaround for problems with API Endpoints & default values
    private int _page = 1;
    public int? Page
    {
        get => _page;
        set
        {
            if (value > 0)
            {
                _page = value.Value;
            }
        }
    }

    // Workaround for problems with API Endpoints & default values
    private int _pageSize = 10;
    public int? PageSize
    {
        get => _pageSize;
        init
        {
            if (value > 0)
            {
                _pageSize = value.Value;
            }
        }
    }
}
