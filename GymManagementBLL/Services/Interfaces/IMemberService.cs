using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<GetAllMembersVM> GetAllMembers();
        bool CreateMember(CreatedMemberVM model);
        GetMemberDetailsVM? GetMemberDetails(int memberId);

        HealthRecordVM? GetMemberHealthDetails(int memberId);

        bool UpdateMember(int memberId, UpdatedMemberVM model);

        bool RemoveMember(int memberId);

        UpdatedMemberVM? GetMemberForUpdate(int memberId);
    }
}
