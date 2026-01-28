using GymManagementBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionVM> GetAllSessionsWithTrainerAndCategory();
        IEnumerable<MemberSessionVM> GetAllMembersForSession(int id);
        bool CreateBooking(CreateBookingVM model);
        IEnumerable<MemberForSelectListVM> GetMemberForDropdown(int id);
        bool MemberAttended(MemberAttendanceVM model);
        bool CancelBooking(MemberAttendanceVM model);
    }
}
