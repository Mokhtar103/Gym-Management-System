using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOFWork _unitOFWork;

        public MemberService(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public bool CreateMember(CreatedMemberVM model)
        {
            try
            {
                if (IsEmailExists(model.Email))
                {
                    return false;
                }
                if (IsPhoneExists(model.Phone))
                {
                    return false;
                }
                var member = new Member
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,
                        City = model.City,
                        Street = model.Street
                    },
                    HealthRecord = new HealthRecord
                    {
                        Weight = model.HealthRecordVM.Weight,
                        Height = model.HealthRecordVM.Height,
                        BloodType = model.HealthRecordVM.BloodType,
                        Note = model.HealthRecordVM.Note
                    }
                };

                _unitOFWork.GetRepository<Member>().Add(member);
                _unitOFWork.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<GetAllMembersVM> GetAllMembers()
        {
            var member = _unitOFWork.GetRepository<Member>().GetAll() ?? [];

            if (member is null || !member.Any())
            {
                return [];
            }

            var membersVM = member.Select(m => new GetAllMembersVM
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()

            });

            return membersVM;


        }

        public GetMemberDetailsVM? GetMemberDetails(int memberId)
        {
            var member = _unitOFWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return null;
            var memberVM = new GetMemberDetailsVM
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                Address = FormatAddress(member.Address),
                DateOfBirth = member.DateOfBirth.ToShortDateString()
            };

            var activeMembership = _unitOFWork.GetRepository<Membership>()
                                    .GetAll(m => m.MemberId == member.Id && m.Status == "Active")
                                    .FirstOrDefault();

            if (activeMembership is not null)
            {
                var activeplan = _unitOFWork.GetRepository<Plan>().GetById(activeMembership.PlanId);
                memberVM.PlanName = activeplan?.Name ?? "N/A";
                memberVM.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                memberVM.MembershipEndDate = activeMembership.EndDate.ToShortDateString();

            }

            return memberVM;

        }

        public HealthRecordVM? GetMemberHealthDetails(int memberId)
        {
            var member = _unitOFWork.GetRepository<HealthRecord>().GetById(memberId);

            if (member is null)
                return null;

            var healthRecordVM = new HealthRecordVM
            {
                Weight = member.Weight,
                Height = member.Height,
                BloodType = member.BloodType,
                Note = member.Note

            };

            return healthRecordVM;
        }

        public bool UpdateMember(int memberId, UpdatedMemberVM model)
        {
            var member = _unitOFWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return false;

            var emailExist = _unitOFWork.GetRepository<Member>()
                                .GetAll(m => m.Email == model.Email && m.Id != memberId);

            var phoneExist = _unitOFWork.GetRepository<Member>()
                                .GetAll(m => m.Phone == model.Phone && m.Id != memberId);

            if(emailExist.Any() || phoneExist.Any())
                return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address = new Address
            {
                BuildingNumber = model.BuildingNumber,
                City = model.City,
                Street = model.Street
            };
            member.UpdatedAt = DateTime.Now;

            _unitOFWork.GetRepository<Member>().Update(member);
            _unitOFWork.SaveChanges();

            return true;
        }

        public UpdatedMemberVM? GetMemberForUpdate(int memberId)
        {
            var member = _unitOFWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return null;
            var memberToUpdateVM = new UpdatedMemberVM
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street

            };
            return memberToUpdateVM;
        }

        public bool RemoveMember(int memberId)
        {
            var member = _unitOFWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return false;

            var activeBookings = _unitOFWork.GetRepository<Booking>()
                                    .GetAll(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now);

            if (activeBookings is not null && activeBookings.Any())
                return false;

            var memberships = _unitOFWork.GetRepository<Membership>().GetAll(m => m.MemberId == memberId);
            try
            {
                if(memberships.Any())
                {
                    foreach (var membership in memberships)
                        _unitOFWork.GetRepository<Membership>().Delete(membership);
                }
                _unitOFWork.GetRepository<Member>().Delete(member);
                _unitOFWork.SaveChanges();

                return true;
                
            }
            catch (Exception)
            {

                throw;
            }
        }



        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            var member = _unitOFWork.GetRepository<Member>().GetAll(m => m.Email == email);
            return member is not null && member.Any();
        }
        private bool IsPhoneExists(string phone)
        {
            var member = _unitOFWork.GetRepository<Member>().GetAll(m => m.Phone == phone);
            return member is not null && member.Any();
        }

        private string FormatAddress(Address address)
        {
            if (address is null)
                return string.Empty;
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }


        #endregion
    }
}
