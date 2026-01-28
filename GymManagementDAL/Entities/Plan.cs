namespace GymManagementDAL.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Membership> MembersPlans { get; set; } = new List<Membership>();

    }
}
