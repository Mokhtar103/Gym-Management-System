namespace GymManagementBLL.ViewModels
{
    public class GetAllMembersVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Photo { get; set; }
        public string Gender { get; set; } = null!;



    }
}
