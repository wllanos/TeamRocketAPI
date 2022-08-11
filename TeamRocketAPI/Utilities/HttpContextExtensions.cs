using Microsoft.EntityFrameworkCore;

namespace TeamRocketAPI.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInHeader<T>
            (this HttpContext httpContext, IQueryable<T> query)
        {
            if(httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double total = await query.CountAsync();
            httpContext.Response.Headers.Add("TotalOfRecords", total.ToString());

        }
    }
}
