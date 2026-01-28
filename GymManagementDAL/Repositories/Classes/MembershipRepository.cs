using DataAccess.Repositories.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Classes
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Membership> GetAllMembershipsWithMembersAndPlans(Func<Membership, bool>? filter = null)
        {
            var Memberships = _dbContext.Memberships.Include(m => m.Member)
                                                    .Include(m => m.Plan)
                                                    .Where(filter ?? (_ => true));

            return Memberships;
        }

        public Membership? GetFirstOrDefault(Func<Membership, bool>? filter = null)
        {
            var membership = _dbContext.Memberships.FirstOrDefault(filter ?? (_ => true));
            return membership;
        }
    }
}
