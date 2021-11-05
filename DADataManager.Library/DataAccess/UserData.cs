using DADataManager.Library.Internal.DataAccess;
using DADataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DADataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "DADataConnection");

            return output;
        }
    }
}