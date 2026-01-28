namespace GymManagementDAL.Entities
{
    public class Membership : BaseEntity
    {
        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                return EndDate >= DateTime.Now ? "Active" : "Expired";
            }
        }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }
        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; }
    }
}
