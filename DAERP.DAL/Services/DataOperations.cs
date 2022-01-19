using DAERP.BL;
using DAERP.BL.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.Services
{
    public static class DataOperations
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            try
            {
                foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
                {
                    if (prop is not null)
                    {
                        obj = prop.GetValue(obj, null);
                    }
                }
                return obj;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static List<string> GetParametersForSortingPurposes<T>(T model)
        {
            List<string> parameters = new List<string>();
            parameters.AddRange(GetChildProperties(model));
            return parameters;
        }
        private static List<string> GetChildProperties<T>(T model)
        {
            List<string> parameters = new List<string>();
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propValue = property.GetValue(model, null);
                if (propValue != null) // because of Lists
                {
                    if (property.PropertyType.ToString().EndsWith("Model"))
                    {
                        List<string> childProps = GetChildProperties(propValue);
                        List<string> childPropsWithModelName = new List<string>();
                        childProps.ForEach(cp =>
                        {
                            childPropsWithModelName.Add(property.PropertyType.Name.Replace("Model",String.Empty) + "." + cp);
                        });
                        parameters.AddRange(childPropsWithModelName);
                    }
                    else
                    {
                        parameters.Add(property.Name);
                    }
                }
                
            }
            return parameters;
        }
    }
}
