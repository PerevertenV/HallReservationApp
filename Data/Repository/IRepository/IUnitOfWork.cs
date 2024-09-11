using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IHallRepository Hall { get; }
		IReservationRepository Reservation { get; }
	}
}
