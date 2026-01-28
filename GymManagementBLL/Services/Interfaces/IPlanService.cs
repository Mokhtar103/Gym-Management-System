using GymManagementBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        bool UpdatePlan(int planId, UpdatedPlanVM input);
        UpdatedPlanVM? GetPlanToUpdate(int planId);
        IEnumerable<GetPlanVM> GetAllPlans();
        GetPlanVM? GetPlanById(int planId);
        bool ActivatePlan(int planId);
    }
}
