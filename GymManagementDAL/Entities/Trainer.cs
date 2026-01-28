using GymManagementDAL.Entities.Enums;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialities Specialities { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
