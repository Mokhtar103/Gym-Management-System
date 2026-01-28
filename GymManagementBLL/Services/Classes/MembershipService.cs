using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }
        public bool CreateMembership(CreateMembershipVM model)
        {
            if (!IsMemberExists(model.MemberId) || !IsPlanExists(model.PlanId) || HasActiveMemberships(model.MemberId))
                return false;

            var membershipRepo = _unitOFWork.MembershipRepository;
            var membershipToCreate = _mapper.Map<Membership>(model);

            var plan = _unitOFWork.GetRepository<Plan>().GetById(model.PlanId);
            membershipToCreate.EndDate = DateTime.UtcNow.AddDays(plan!.DurationInDays);

            membershipRepo.Add(membershipToCreate);

            return _unitOFWork.SaveChanges() > 0;
        }

        public bool DeleteMembership(int memberId)
        {
            var membershipRepo = _unitOFWork.MembershipRepository;

            var membershipToDelete = membershipRepo.GetFirstOrDefault(m => m.MemberId == memberId && m.Status == "Active");
            if (membershipToDelete is null)
                return false;

            membershipRepo.Delete(membershipToDelete);
            return _unitOFWork.SaveChanges() > 0;
        }

        public IEnumerable<MembershipVM> GetAllMemberships()
        {
            var memberships = _unitOFWork.MembershipRepository.GetAllMembershipsWithMembersAndPlans(m => m.Status == "Active");
            var membershipsVM = _mapper.Map<IEnumerable<MembershipVM>>(memberships);
            return membershipsVM;

        }

        public IEnumerable<MemberForSelectListVM> GetMembersForDropdown()
        {
            var members = _unitOFWork.GetRepository<Member>().GetAll();
            var memberSelectList = _mapper.Map<IEnumerable<MemberForSelectListVM>>(members);
            return memberSelectList;
        }

        public IEnumerable<PlanForSelectListVM> GetPlansForDropdown()
        {
            var plans = _unitOFWork.GetRepository<Plan>().GetAll(p => p.IsActive);
            var planSelectList = _mapper.Map<IEnumerable<PlanForSelectListVM>>(plans);
            return planSelectList;
        }

        #region Helper Methods
        private bool IsMemberExists(int memberId)
            => _unitOFWork.GetRepository<Member>().GetById(memberId) is not null;
        private bool IsPlanExists(int planId)
            => _unitOFWork.GetRepository<Plan>().GetById(planId) is not null;

        private bool HasActiveMemberships(int memberId)
            => _unitOFWork.MembershipRepository
            .GetAllMembershipsWithMembersAndPlans(m => m.MemberId == memberId && m.Status == "Active").Any();

       

        #endregion
    }
}
