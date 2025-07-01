using static System.Math;

namespace BlogSystem.Service.ResponseBase.Paginations
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResponse<T>> ToPaginatedListAsync<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
            where T : class
        {
            if (source == null)
            {
                throw new Exception("Empty");
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = Min(Max(3, pageSize), 20);

            int count = source.Count();
            //if (count == 0) return PaginatedResponse<T>.Create(new List<T>(), count, pageNumber, pageSize);

            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return PaginatedResponse<T>.Create(items, count, pageNumber, pageSize);
        }
    }
}
