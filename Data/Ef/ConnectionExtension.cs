using Data.Ef;
using Microsoft.EntityFrameworkCore;

namespace Data;

public static class ConnectionExtension
    {
        public static  async Task<EntityConnection<E>> GetConnection<E>(this IQueryable<E> source, PaginationParameter parameters, Dictionary<string,object>[]? orderBy) where E : Entity
        {
            PageInfo pageInfo = new PageInfo { CurrentPage = parameters.Page, PageSize = parameters.PageSize };
            var rowCount = await source.CountAsync();
            pageInfo.RowCount = rowCount;
            pageInfo.PageCount = rowCount / pageInfo.PageSize;
            if (rowCount % pageInfo.PageSize > 0)
            {
                pageInfo.PageCount += 1;
            }
            pageInfo.HasPreviousPage = pageInfo.CurrentPage > 1;
            pageInfo.HasNextPage = pageInfo.CurrentPage < pageInfo.PageCount;
            return new EntityConnection<E>(pageInfo)
            {
                Edges = await (orderBy != null?  source.OrderBy(orderBy): source).Skip(parameters.Skip).Take(parameters.Take).ToListAsync()
            };
        }
    }
