using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.HallServices.Interfaces
{
	
	public interface IHallBL//Hall business logic
	{
		//шукає вільні холи за запитом 
		public Task<IEnumerable<Hall>> FindHallsAsync(DateTime ReservationDateTime,
			TimeOnly duration, int capacity);
	}
}
