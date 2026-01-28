namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Booking> SessionMembers { get; set; } = new List<Booking>();
        public Trainer Trainer { get; set; } = null!;
        public int TrainerId { get; set; }
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }


    }
}
