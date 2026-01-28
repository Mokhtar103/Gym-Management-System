using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _context;

        public HealthRecordRepository(GymDbContext context)
        {
            _context = context;
        }
        public int Add(HealthRecord record)
        {
            _context.Add(record);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var record = GetById(id);
            if (record == null)
                return 0;

            _context.Remove(record);
            return _context.SaveChanges();

        }

        public IEnumerable<HealthRecord> GetAll()
        {
            return _context.HealthRecords.ToList();

        }

        public HealthRecord? GetById(int id)
        {
            return _context.HealthRecords.Find(id);

        }

        public int Update(HealthRecord record)
        {
            _context.Update(record);
            return _context.SaveChanges();
        }
    }
}
