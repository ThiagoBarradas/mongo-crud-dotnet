using Mongo.CRUD.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Mongo.CRUD.Helpers
{
    /// <summary>
    /// Property Helper
    /// </summary>
    public static class PropertyHelper
    {
        private static readonly ConcurrentDictionary<(Type Type, string PropertyName), PropertyInfo> PropertyInfoByNameCache
            = new ConcurrentDictionary<(Type, string), PropertyInfo>();

        private static readonly ConcurrentDictionary<(Type Type, Type AttributeType), (PropertyInfo PropertyInfo, Attribute[] Attributes)> PropertyInfoByAttributeCache
            = new ConcurrentDictionary<(Type, Type), (PropertyInfo, Attribute[])>();

        /// <summary>
        /// Get single property details by property name
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyDetails GetIndividualPropertyDetailsByPropertyName<TObject>(this TObject obj, string propertyName)
            where TObject : class, new()
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (string.IsNullOrWhiteSpace(propertyName) == true)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var propertyInfo = PropertyInfoByNameCache.GetOrAdd((obj.GetType(), propertyName.Trim()), (key) =>
            {
                var properties = key.Type.GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name.Equals(key.PropertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return properties[i];
                    }
                }
                return null;
            });
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(obj);
                return new PropertyDetails
                {
                    PropertyInfo = propertyInfo,
                    Value = value
                };
            }
            return null;
        }

        /// <summary>
        /// Get single property details from attibute
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static PropertyDetails<TAttribute> GetIndividualPropertyDetailsByAttribute<TObject, TAttribute>(this TObject obj)
            where TObject : class, new()
            where TAttribute : Attribute
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var (propertyInfo, attributes) = PropertyInfoByAttributeCache.GetOrAdd((obj.GetType(), typeof(TAttribute)), (key) =>
            {
                var properties = key.Type.GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    var attrs = (Attribute[])properties[i].GetCustomAttributes(key.AttributeType, true);
                    if (attrs != null & attrs.Any())
                    {
                        return (properties[i], attrs);
                    }
                }
                return (null, null);
            });
            if (propertyInfo != null && attributes != null) 
            {
                var value = propertyInfo.GetValue(obj);
                return new PropertyDetails<TAttribute>
                {
                    Attributes = (TAttribute[])attributes,
                    PropertyInfo = propertyInfo,
                    Value = value
                };
            }

            return null;
        }
    }
}
