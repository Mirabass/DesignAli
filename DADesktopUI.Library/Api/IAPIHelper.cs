using System.Net.Http;
using System.Threading.Tasks;
using DADesktopUI.Models;

namespace DADesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        public HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}