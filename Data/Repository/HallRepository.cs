using Data.Data;
using Data.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
	public class HallRepository: Repository<Hall>, IHallRepository
	{
		private readonly AppDbContext _context;
        public HallRepository(AppDbContext  context) :  base(context)  
        {
            _context = context;
        }
    }
}
