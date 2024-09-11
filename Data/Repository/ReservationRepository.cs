using Data.Data;
using Data.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Data.Repository
{
	public class ReservationRepository: Repository<Reservation>, IReservationRepository
	{
		private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context) : base(context) 
        {
            _context = context; 
        }

		public int CountFullPrice(int BPPH, string? additionalOptions, 
			DateTime reservTime, TimeOnly duration)
		{
			//інціалізуємо список із додатковими опціями
			List<string> options = new List<string>();
			//інціалізуємо змінну із кінцевою ціною куди будемо додавати інші суми
			//та одразу рахуємо базову ціну із к-стю годин
			int FinalPrice = (int)(((duration.Hour*60 + duration.Minute)/60) * BPPH); 
			//перевіряємо чи в нас є якісь додаткові функції
			if (!additionalOptions.IsNullOrEmpty()) 
			{
				//якщо є додаткові функції ми їх отримуємо
				 options = additionalOptions.Split('/').ToList();
			}
			//перевіряємо чи в нас є якісь додаткові функції
			if (options.Any()) 
			{
				//порінюємо значення, отримуємо суму та додаємо опції 
				foreach (string option in options) 
				{
					if (SD.AddOptions.ContainsKey(option)) 
					{
						SD.AddOptions.TryGetValue(option, out int price);
						FinalPrice += price;
					}	
				}
			}
			//часові перетворення та визначення часу кінця броні 
			TimeOnly reservStartInHours = TimeOnly.FromDateTime(reservTime);
			TimeOnly reservEndInHours = TimeOnly.FromTimeSpan(reservStartInHours.ToTimeSpan() 
				+ duration.ToTimeSpan());

			//розрахунок вартості залежно від  часу бронювання
			if(reservStartInHours >= new TimeOnly(6, 0) && reservEndInHours < new TimeOnly(9, 0)) 
			{
				FinalPrice -= (int)((10 * FinalPrice) / 100);
			}
			if(reservStartInHours >= new TimeOnly(18, 0) && reservEndInHours < new TimeOnly(23, 0)) 
			{
				FinalPrice -= (int)((20 * FinalPrice) / 100);
			}
			if(reservStartInHours >= new TimeOnly(12, 0) && reservEndInHours < new TimeOnly(14, 0)) 
			{
				FinalPrice += (int)((15 * FinalPrice) / 100);
			}

			return FinalPrice;
		}
	}
}
