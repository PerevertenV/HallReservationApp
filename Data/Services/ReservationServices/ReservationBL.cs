using Data.Services.ReservationServices.Interfces;
using Microsoft.IdentityModel.Tokens;
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
	}
}
