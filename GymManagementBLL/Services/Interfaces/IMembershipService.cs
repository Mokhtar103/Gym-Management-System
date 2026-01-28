using GymManagementBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipVM> GetAllMemberships();
        IEnumerable<MemberForSelectListVM> GetMembersForDropdown();
        IEnumerable<PlanForSelectListVM> GetPlansForDropdown();
        bool CreateMembership(CreateMembershipVM model);
        bool DeleteMembership(int memberId);
    }
}
