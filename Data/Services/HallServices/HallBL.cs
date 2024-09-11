using Data.Repository.IRepository;
using Data.Services.HallServices.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.HallServices
{
	public class HallBL : IHallBL
	{
		private readonly IUnitOfWork _unitOfWork;
        public HallBL(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;	
        }
        public async Task<IEnumerable<Hall>> FindHallsAsync(DateTime ReservationDateTime, 
			TimeOnly duration, int capacity)
		{
			//отримуємо всі зали які не є заброньованими
			List<Hall> hallList = (await _unitOfWork.Hall.GetAllAsync(u =>
				u.reserved == false && u.Capacity == capacity)).ToList();

			//отримуємо резервації для того щоб отримати зали які є зарезервовані
			//але підходять під умову резерваці(в той самий день але на інші години резервація)
			List<Reservation> reservationList = (await _unitOfWork.Reservation.GetAllAsync()).ToList();
			if (reservationList != null)
			{
				//вибираємо лише ті резервації котрі збігаються із потрібною датою резервації
				reservationList = reservationList.Where(u =>
					u.dateTimeOfReserv.Date == ReservationDateTime.Date).ToList();
				//перевіряємо чи список не пустий після фільтрації
				if (reservationList != null)
				{

					//фільтрація за часом, представлені перетворення часу для коректної
					//відфільтрації резервацій котрі не потрапляють у заданий чаосвий проміжок
					reservationList = reservationList.Where(u =>
						(u.dateTimeOfReserv.Add(u.reservTime.ToTimeSpan()) < ReservationDateTime)
						|| (u.dateTimeOfReserv < ReservationDateTime.Add(duration.ToTimeSpan())))
						.ToList();

					Hall singleHall = new Hall();
					//перевіряємо чи є елементи після останьої фільтрації
					if (reservationList.Any())
					{
						//якщо ще залишились елементи то отримуємо ці зали та додаємо до списку
						foreach (Reservation reserv in reservationList)
						{
							singleHall = await _unitOfWork.Hall.GetFirstOrDefaultAsync(u =>
								u.Id == reserv.hallId && u.Capacity == capacity);

							if (singleHall != null) { hallList.Add(singleHall); }
						}
					}
				}
			}
			
			//після всіх пошуків преедаємо список елементів
			return (hallList);
		}
	}
}
