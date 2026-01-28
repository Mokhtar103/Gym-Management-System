using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels
{
    public class HealthRecordVM
    {
        [Range(0, 500, ErrorMessage = "Weight must be between 0 and 500 kg.")]
        public decimal Weight { get; set; }
        [Range(0, 300, ErrorMessage = "Height must be between 0 and 300 cm.")]
        public decimal Height { get; set; }
        [Required(ErrorMessage = "Blood Type is Required")]
        [StringLength(3, ErrorMessage = "Blood Type must be at most 3 characters.")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
