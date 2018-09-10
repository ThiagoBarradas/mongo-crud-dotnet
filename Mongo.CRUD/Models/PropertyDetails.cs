using System;
using System.Reflection;

namespace Mongo.CRUD.Models
{
    /// <summary>
    /// Property details from attribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class PropertyDetails<TAttribute> : PropertyDetails 
        where TAttribute : Attribute
    {
        /// <summary>
        /// Attibutes
        /// </summary>
        public TAttribute[] Attributes { get; set; }
    }

    /// <summary>
    /// Property details
    /// </summary>
    public class PropertyDetails
    {
        /// <summary>
        /// Property info
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Property value
        /// </summary>
        public object Value { get; set; }
    }
}
