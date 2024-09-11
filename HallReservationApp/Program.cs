using Data.Data;
using Data.Repository;
using Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HallReservationApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddControllers();

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			var app = builder.Build();

			app.MapGet("/", () => "Hello World!");

			app.MapControllers();

			app.Run();
		}
	}
}
