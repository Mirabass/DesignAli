using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Mapper<T>
    {
        public static T Map(Dictionary<(int, int), string> data, Dictionary<string, int> mapSettings)
        {
            T result = default(T);
            return result;
        }
    }
}
