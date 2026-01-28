namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public ICollection<Booking> MemberSessions { get; set; } = new List<Booking>();
    }
}
