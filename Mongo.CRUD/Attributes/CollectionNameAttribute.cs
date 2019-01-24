using System;

namespace Mongo.CRUD.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        public string CollectionName { get; set; }

        public CollectionNameAttribute(string collectionName)
        {
            this.CollectionName = collectionName;
        }
    }
}
