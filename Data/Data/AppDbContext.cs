using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			

		}
		public DbSet<Hall> Halls { get; set; }
		public DbSet<Reservation> Reservations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Hall>().HasData(
				new Hall
				{
					Id = 1,
					Name = "Hall A",
					Capacity = 50,
					AdditionalOptions = "projector/wifi/sound",
					PricePerHour = 2000,
					reserved = false,
				}

			);
		}
	}
}
