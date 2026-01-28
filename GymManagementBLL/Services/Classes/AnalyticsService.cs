using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOFWork _unitOFWork;

        public AnalyticsService(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }
        public AnalyticsVM GetAnalyticsData()
        {
            var sessionRepository = _unitOFWork.GetRepository<Session>();
            return new AnalyticsVM
            {
                TotalMembers = _unitOFWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = _unitOFWork.GetRepository<Membership>().GetAll(x => x.Status == "Active").Count(),
                TotalTrainers = _unitOFWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = sessionRepository.GetAll(s => s.StartDate > DateTime.Now).Count(),
                OngoingSessions = sessionRepository.GetAll(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now).Count(),
                CompletedSessions = sessionRepository.GetAll(s => s.EndDate < DateTime.Now).Count()
            };
        }
    }
}
