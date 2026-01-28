using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels
{
    public class UpdatedMemberVM
    {
       

        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone is Required")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must start with '01' and be 11 digits long.")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; } = null!;

     

       

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than zero")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain only letters and spaces.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "City must be between 2 and 150 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street must contain letters, numbers and spaces.")]
        public string Street { get; set; } = null!;
    }
}
