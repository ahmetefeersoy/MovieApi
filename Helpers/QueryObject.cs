using System;
using System.Collections.Generic;

namespace api.Helpers
{
    public class QueryObject
    {
        public string? Name { get; set; } = null;
        public string? Genre { get; set; } = null;
        public SortByOptions? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2000;
        
        public int? MinYear { get; set; } = null;
        public int? MaxYear { get; set; } = null;
    }
}
