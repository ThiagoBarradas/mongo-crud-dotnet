using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mongo.CRUD.Models
{
    public class ProjectionOptions
    {
        private List<string> _fieldsToInclude = new List<string>();
        public List<string> FieldsToInclude
        {
            get => _fieldsToInclude;
            set
            {
                if(FieldsToExclude?.Any() == true)
                {
                    throw new InvalidOperationException("Cannot mix include and exclude projection fields.");
                }

                _fieldsToInclude = value;
            }
        }

        private List<string> _fieldsToExclude = new List<string>();
        public List<string> FieldsToExclude
        {
            get => _fieldsToExclude;
            set
            {
                if (FieldsToInclude?.Any() == true)
                {
                    throw new InvalidOperationException("Cannot mix include and exclude projection fields.");
                }

                _fieldsToExclude = value;
            }
        }

        public ProjectionOptions WithFields<TDocument>(params Expression<Func<TDocument, object>>[] fieldsToInclude)
        {
            if (FieldsToExclude?.Any() == true) return this;

            FieldsToInclude.AddRange(fieldsToInclude.Select(x => (x.Body as MemberExpression).Member.Name));
            return this;
        }

        public ProjectionOptions WithFields(params string[] fieldsToInclude)
        {
            if (FieldsToExclude?.Any() == true) return this;

            FieldsToInclude.AddRange(fieldsToInclude);
            return this;
        }

        public ProjectionOptions WithoutFields<TDocument>(params Expression<Func<TDocument, object>>[] fieldsToExclude)
        {
            if (FieldsToInclude?.Any() == true) return this;

            FieldsToExclude.AddRange(fieldsToExclude.Select(x => (x.Body as MemberExpression).Member.Name));
            return this;
        }

        public ProjectionOptions WithoutFields(params string[] fieldsToExclude)
        {
            if (FieldsToInclude?.Any() == true) return this;

            FieldsToExclude.AddRange(fieldsToExclude);
            return this;
        }

        internal ProjectionDefinition<TDocument, TDocument> BuildProjectionByFields<TDocument>()
        {
            var projectionBuilder = Builders<TDocument>.Projection;

            if (FieldsToInclude?.Any() == true)
            {
                var firstField = FieldsToInclude.FirstOrDefault();
                var projection = projectionBuilder.Include(firstField);
                foreach (var field in FieldsToInclude.Skip(1))
                {
                    projection = projection.Include(field);
                }

                return projection;
            }

            if (FieldsToExclude?.Any() == true)
            {
                var firstField = FieldsToExclude.FirstOrDefault();
                var projection = projectionBuilder.Exclude(firstField);

                foreach (var field in FieldsToExclude.Skip(1))
                {
                    projection = projection.Exclude(field);
                }

                return projection;
            }

            return null;
        }
    }
}
