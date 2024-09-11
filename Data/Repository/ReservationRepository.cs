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
	}
}
