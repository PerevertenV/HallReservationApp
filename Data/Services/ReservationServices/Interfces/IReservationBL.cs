using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ReservationServices.Interfces
{
	public interface IReservationBL
	{
		public int CountFullPrice(int BPPH, string? additionalOptions,
			DateTime reservTime, TimeOnly duration);
	}
}
