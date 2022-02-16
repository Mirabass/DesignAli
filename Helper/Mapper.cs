using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Mapper<T> where T : new()
    {
        public static T Map(Dictionary<int, string> data, Dictionary<string, int> mapSettings)
        {
            T obj = new T();
            foreach (var setting in mapSettings)
            {
                PropertyInfo piInstance = obj.GetType().GetProperty(setting.Key);
                Type propertyType = piInstance.PropertyType;
                object propertyValue = data[setting.Value].ToString();
                if (!string.IsNullOrWhiteSpace((string)propertyValue))
                {
                    propertyValue = Convert.ChangeType(propertyValue, propertyType, CultureInfo.InvariantCulture);
                    piInstance.SetValue(obj, propertyValue);
                }
            }
            return obj;
        }
    }
}
