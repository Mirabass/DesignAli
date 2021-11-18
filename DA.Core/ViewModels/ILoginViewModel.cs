using System.Threading.Tasks;

namespace DA.Core.ViewModels
{
    public interface ILoginViewModel
    {
        bool CanLogIn { get; }
        string? ErrorMessage { get; set; }
        bool IsErrorVisible { get; }
        string? Password { get; set; }
        string? UserName { get; set; }

        Task Initialize();
        Task LogIn();
    }
}