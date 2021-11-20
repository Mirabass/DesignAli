using DA.Core.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace DA.Core.Services
{
    public interface IUserService
    {
        HttpClient? ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string? token);
    }
}