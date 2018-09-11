using Mongo.CRUD.Enums;
using Mongo.CRUD.Models;
using MongoDB.Driver;

namespace Mongo.CRUD.Builders
{
    /// <summary>
    /// Filter builder
    /// </summary>
    public static class FilterBuilder
    {
        /// <summary>
        /// Set sorting for search
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="queryResults"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IFindFluent<TDocument, TDocument> WithSorting<TDocument>(this IFindFluent<TDocument, TDocument> queryResults, SearchOptions options)
            where TDocument : class, new()
        {
            if (string.IsNullOrWhiteSpace(options.SortField) == false)
            {
                var sortBuilder = Builders<TDocument>.Sort;
                SortDefinition<TDocument> sortFilter;

                if (options.SortMode == SortMode.Asc)
                {
                    sortFilter = sortBuilder.Ascending(options.SortField);
                }
                else
                {
                    sortFilter = sortBuilder.Descending(options.SortField);
                }

                queryResults.Sort(sortFilter);
            }

            return queryResults;
        }

        /// <summary>
        /// Set paging for search
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="queryResults"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IFindFluent<TDocument, TDocument> WithPaging<TDocument>(this IFindFluent<TDocument, TDocument> queryResults, SearchOptions options)
            where TDocument : class, new()
        {
            var offset = (options.PageNumber - 1) * options.PageSize;
            queryResults.Skip(offset).Limit(options.PageSize);
            return queryResults;
        }

        /// <summary>
        /// Join filters using "AND" or "OR" operator. Default "AND"
        /// Dont worry about null or others issues about filter join
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="filter"></param>
        /// <param name="filterToAdd"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static FilterDefinition<TDocument> Join<TDocument>(this FilterDefinition<TDocument> filter, FilterDefinition<TDocument> filterToAdd)
            where TDocument : class, new()
        {
            return Join(filter, filterToAdd, Operator.And);
        }

        /// <summary>
        /// Join filters using "AND" or "OR" operator
        /// Dont worry about null or others issues about filter join
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="filter"></param>
        /// <param name="filterToAdd"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static FilterDefinition<TDocument> Join<TDocument>(this FilterDefinition<TDocument> filter, FilterDefinition<TDocument> filterToAdd, Operator operation)
            where TDocument : class, new()
        {
            if (filter == null)
            {
                return filterToAdd;
            }

            if (filterToAdd == null)
            {
                return filter;
            }

            if (operation == Operator.And)
            {
                return filter & filterToAdd;
            }
            else
            {
                return filter | filterToAdd;
            }
        }

        /// <summary>
        /// Get filter builder
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        public static FilterDefinitionBuilder<TDocument> GetFilterBuilder<TDocument>()
            where TDocument : class, new()
        {
            return Builders<TDocument>.Filter;
        }
    }
}
