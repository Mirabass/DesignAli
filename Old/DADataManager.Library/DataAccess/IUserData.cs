using DADataManager.Library.Models;
using System.Collections.Generic;

namespace DADataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}