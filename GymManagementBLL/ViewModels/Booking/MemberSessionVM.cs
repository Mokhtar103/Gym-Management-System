namespace GymManagementBLL.ViewModels
{
    public class MemberSessionVM
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public int SessionId { get; set; }
        public string BookingDate { get; set; } = null!;
        public bool IsAttended { get; set; }
    }
}
