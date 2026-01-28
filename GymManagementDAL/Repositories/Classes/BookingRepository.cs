using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _context;

        public BookingRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetSessionById(int sessionId)
        {
            var sessionBookings = _context.Bookings.Where(b => b.SessionId == sessionId)
                                                   .Include(b => b.Member)
                                                   .ToList();
            return sessionBookings;
        }
    }
}
