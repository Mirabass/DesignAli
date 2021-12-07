using System.Collections.Generic;

namespace DADataManager.Library.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        object SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}