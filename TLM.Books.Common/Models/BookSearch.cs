using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace TLM.Books.Common.Models;

public class BookSearch : IQueryPaging, IQuerySort
{
    [QueryOperator(Operator = WhereOperator.Contains, HasName = "Name", CaseSensitive = false)]
    public string Name { get; set; }

    
    public int? Limit { get; set; } = 10;
    public int? Offset { get; set; }
    public string Sort { get; set; }
}