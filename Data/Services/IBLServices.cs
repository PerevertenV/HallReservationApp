using Data.Services.HallServices.Interfaces;
using Data.Services.ReservationServices.Interfces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
	public interface IBLServices
	{
		IHallBL Hall { get; }
		IReservationBL Reservation { get; }
	}
}
