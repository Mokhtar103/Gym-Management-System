using DataAccess.Repositories.Interfaces;
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
    public class UnitOfWork : IUnitOFWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> repositories = [];
        public ISessionRepository SessionRepository { get; set; }
        public IMembershipRepository MembershipRepository { get; set; }

        public IBookingRepository BookingRepository { get; set; }


        public UnitOfWork(GymDbContext dbContext,
            ISessionRepository sessionRepository,
            IMembershipRepository membershipRepository,
            IBookingRepository bookingRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
            BookingRepository = bookingRepository;
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var entityName = typeof(TEntity).Name;

            if(repositories.TryGetValue(entityName, out object? repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }

            var newRepository = new GenericRepository<TEntity>(_dbContext);
            repositories.Add(entityName, newRepository);
            return newRepository;

        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
