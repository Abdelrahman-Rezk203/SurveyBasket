using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SurveyBasket.API.Abstractions
{
    public class PaginationList<T>
    {
        public PaginationList(List<T> items , int count , int pageSize , int pageNumber)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling((count / (double)pageSize));
        }

        public List<T> Items { get; private set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginationList<T>(items , count , pageSize , pageNumber );
        }

    }
}
