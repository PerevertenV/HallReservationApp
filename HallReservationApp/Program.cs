using Data.Data;
using Data.Repository;
using Data.Repository.IRepository;
using Data.Services;
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

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IBLServices, BLServices>();

			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
					c.RoutePrefix = string.Empty;
				});
			}

			app.MapControllers();

			app.Run();
		}
	}
}
