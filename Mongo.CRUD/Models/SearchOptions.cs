using Mongo.CRUD.Enums;

namespace Mongo.CRUD.Models
{
    /// <summary>
    /// Search options - paging and sorting
    /// </summary>
    public class SearchOptions
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public SearchOptions(int pageNumber = 1, int pageSize = 10, bool enablePagination = true)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.EnablePagination = enablePagination;
        }

        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page
        {
            get { return this.PageNumber; }
            set { this.PageNumber = value; }
        }

        /// <summary>
        /// Page size
        /// </summary>
        public int Size
        {
            get { return this.PageSize; }
            set { this.PageSize = value; }
        }

        /// <summary>
        /// Sort mode
        /// </summary>
        public SortMode SortMode { get; set; }

        /// <summary>
        /// Sort field
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// Enable pagination. Default is true.
        /// </summary>
        public bool EnablePagination { get; set; }
    }
}
