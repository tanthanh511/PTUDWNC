using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.Collections;

namespace TatBlog.Services.Extensions
{
    public static class PagedListExtensions
    {
        //  tạo biểu thức để sắp xếp dữ liệu 
        // sử dụng mệnh đề order by 
        public static string GetOrderExpression(
            this IPagingParams pagingParams,
            string defaultColumn = "Id")
        {
            var column = string.IsNullOrWhiteSpace(pagingParams.SortColumn)
                ? defaultColumn
                : pagingParams.SortColumn;
            var oder = "ASC".Equals(
                pagingParams.SortOrder, StringComparison.OrdinalIgnoreCase)
                ? pagingParams.SortOrder : "DESC";
            return $"{column} {oder}";
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(
            this IQueryable<T> source,
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);
            var items = await source
                .OrderBy(pagingParams.GetOrderExpression())
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync(cancellationToken);
            return new PagedList<T>(
                items,
                pagingParams.PageNumber,
                pagingParams.PageSize,
                totalCount);
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(
            this IQueryable<T> source,
            int pageNumber = 1,
            int pageSize = 10,
            string sortColumn = "Id",
            string sortOrder = "DESC",
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);
            var items = await source
                .OrderBy($"{sortColumn} {sortOrder}")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return new PagedList<T>(
                items, pageNumber, pageSize, totalCount);
        }


    }
}
