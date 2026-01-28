using DataAccess.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUnitOFWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        ISessionRepository SessionRepository { get; set; }

        IMembershipRepository MembershipRepository { get; set; }

        IBookingRepository BookingRepository { get; set; }

        int SaveChanges();
    }
}
