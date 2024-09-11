using System.Linq.Expressions;

namespace TaskManager.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null);

        Task AddAsync(T item);

        Task UpdateAsync(T item);

        Task DeleteAsync(T item);

    }
}
