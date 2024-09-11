using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Data;
using TaskManager.Repository.IRepository;
using Data.Data;

namespace Data.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _context;
		internal DbSet<T> dbSet;

        public Repository(AppDbContext context)
        {
			_context = context;	
			this.dbSet = _context.Set<T>();
        }
		//CRUD асинхронна реалізація методів
		// асинхронний метод для додавання нової сутності
        public async Task AddAsync(T item)
		{
			await _context.AddAsync(item);
			await _context.SaveChangesAsync();
		}
		// асинхронний метод для видалення певної сутності
		public async Task DeleteAsync(T item)
		{
			 _context.Remove(item);
			await _context.SaveChangesAsync();
		}
		// асинхронний метод для отримання всіх сутностей/використання фільтрів
		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = dbSet;
			return filter !=null ? query.Where(filter).ToList() : query.ToList();
		}
		// асинхронний метод для отримання певної сутності за допомоги лямбда виразу
		public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = dbSet;
			return filter != null ? query.Where(filter).FirstOrDefault() : query.FirstOrDefault();
		}
		// асинхронний метод для оновлення
		public async Task UpdateAsync(T item)
		{
			_context.Update(item);
			await _context.SaveChangesAsync();
		}
	}
}
