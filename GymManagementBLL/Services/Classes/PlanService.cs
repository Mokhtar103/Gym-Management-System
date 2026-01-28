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
    public class PlanService : IPlanService
    {
        private readonly IUnitOFWork _unitOFWork;

        public PlanService(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }
        public bool ActivatePlan(int planId)
        {
            try
            {
                var plan = _unitOFWork.GetRepository<Plan>().GetById(planId);
                if (plan is null || HasActiveMemberShips(planId))
                    return false;

                plan.IsActive = !plan.IsActive;
                plan.UpdatedAt = DateTime.UtcNow;

                _unitOFWork.GetRepository<Plan>().Update(plan);
                _unitOFWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<GetPlanVM> GetAllPlans()
        {
            var plans = _unitOFWork.GetRepository<Plan>().GetAll();

            if (plans == null || !plans.Any())
            {
                return [];
            }

            var planVMs = plans.Select(plan => new GetPlanVM
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationInDays = plan.DurationInDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            });
            return planVMs;
        }

        public GetPlanVM? GetPlanById(int planId)
        {
           var plan = _unitOFWork.GetRepository<Plan>().GetById(planId);
           if (plan is null)
               return null;
            
            var planVM = new GetPlanVM
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationInDays = plan.DurationInDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
            return planVM;
        }

        public UpdatedPlanVM? GetPlanToUpdate(int planId)
        {
            var plan = _unitOFWork.GetRepository<Plan>().GetById(planId);

            if (plan is null || plan.IsActive == false)
                return null;

            var updatedPlanVM = new UpdatedPlanVM
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationInDays = plan.DurationInDays,
                Price = plan.Price
            };
            return updatedPlanVM;

        }

        public bool UpdatePlan(int planId, UpdatedPlanVM input)
        {
            try
            {
                var plan = _unitOFWork.GetRepository<Plan>().GetById(planId);

                if (plan is null || HasActiveMemberShips(planId))
                    return false;

                plan.Name = input.PlanName;
                plan.Description = input.Description;
                plan.DurationInDays = input.DurationInDays;
                plan.Price = input.Price;

                _unitOFWork.GetRepository<Plan>().Update(plan);
                _unitOFWork.SaveChanges();

                return true;

            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Helper Methods
        private bool HasActiveMemberShips(int planId)
        {
            var memberships = _unitOFWork.GetRepository<Membership>().GetAll(m => m.PlanId == planId && m.Status == "Active");
            return memberships.Any();
        }
        #endregion
    }
}
