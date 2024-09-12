using Data.Services.ReservationServices.Interfces;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Data.Services.ReservationServices
{
	public class ReservationBL: IReservationBL
	{
		public int CountFullPrice(int BPPH, string? additionalOptions,
			DateTime reservTime, TimeOnly duration)
		{
			//інціалізуємо список із додатковими опціями
			List<string> options = new List<string>();

			//інціалізуємо змінну із кінцевою ціною куди будемо додавати інші суми
			//та одразу рахуємо базову ціну із к-стю годин
			int FinalPrice = (int)(((duration.Hour * 60 + duration.Minute) / 60) * BPPH);
			//перевіряємо чи в нас є якісь додаткові функції
			if (!additionalOptions.IsNullOrEmpty())
			{
				//перевіряємо із  усіма можливими функціями чи вони вибрані
				foreach (var option in SD.AddOptions)
				{
					//перевіряємо кожну опцію
					if (additionalOptions.Contains(option.Key))
					{
						//у разі збігу додаємо суму
						FinalPrice += option.Value;
					}
				}
			}

			//часові перетворення та визначення часу кінця броні 
			TimeOnly reservStartInHours = TimeOnly.FromDateTime(reservTime);
			TimeOnly reservEndInHours = TimeOnly.FromTimeSpan(reservStartInHours.ToTimeSpan()
				+ duration.ToTimeSpan());

			//розрахунок вартості залежно від  часу бронювання
			if (reservStartInHours >= new TimeOnly(6, 0) && reservEndInHours < new TimeOnly(9, 0))
			{
				FinalPrice -= (int)((10 * FinalPrice) / 100);
			}
			if (reservStartInHours >= new TimeOnly(18, 0) && reservEndInHours < new TimeOnly(23, 0))
			{
				FinalPrice -= (int)((20 * FinalPrice) / 100);
			}
			if (reservStartInHours >= new TimeOnly(12, 0) && reservEndInHours < new TimeOnly(14, 0))
			{
				FinalPrice += (int)((15 * FinalPrice) / 100);
			}

			return FinalPrice;
		}

		public double AvrgPriceForReservations(List<Reservation> reservList)
		{
			//одразу прередаємо значення яке отримуємо через LINQ
			return reservList.Average(u => u.FinalSum);
		}

		public string TheMostPopularAddOption(List<Reservation> reservList)
		{
			//створюємо словник в якому буде ключ це опція 
			//а значенням скільки раз додана ця опція у резерваціях
			Dictionary<string, int> AddOptionDctnr = new Dictionary<string, int>();
			//предаємо всі доступні опції
			foreach(var key in SD.AddOptions.Keys) 
			{
				AddOptionDctnr[key] = 0;
			}
			//преребираємо та рахуємо яка опція є найпопулярнішою
			foreach(Reservation reservation in reservList) 
			{ 
				// перевіряємо чи містить резервація додаткові опції
				foreach (var key in AddOptionDctnr.Keys.ToList())
				{
					if (reservation.SelectedAddOpt.Contains(key))
					{
						//збільшуємо значення для відповідної опції
						AddOptionDctnr[key]++;
					}
				}
			}
			//шукаємо ключ із найбільшим значенням та передаємо його
			return AddOptionDctnr.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
		}

		public TimeOnly TheMostPopularDuration(List<Reservation> reservList)
		{
			//за допомоги LINQ групуємо по значеню 
			//потім сортуємо ці групи по к-сті елементів
			//Вибираємо значення із груп по одному(вибираємо ключ)
			//Обираємо перший яки має найбільше значення
			TimeOnly resultTime = reservList
				.GroupBy(n => n.reservTime)
				.OrderByDescending(g => g.Count())
				.Select(g => g.Key)
				.FirstOrDefault();

			return resultTime;
		}
		public int TheMostPopularHall(List<Reservation> reservList)
		{
			//за допомоги LINQ групуємо по значеню 
			//потім сортуємо ці групи по к-сті елементів
			//Вибираємо значення із груп по одному(вибираємо ключ)
			//Обираємо перший яки має найбільше значення
			int  resultId = reservList
				.GroupBy(n => n.hallId)
				.OrderByDescending(g => g.Count())
				.Select(g => g.Key)
				.FirstOrDefault();
			//прередаємо значення
			return (resultId);
		}
	}
}
