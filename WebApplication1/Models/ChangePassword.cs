using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models {
    public class ChangePassword {
        public ChangePassword(string currentPassword,string newPassword) =>
        (CurrentPassword, NewPassword) = (currentPassword, newPassword);

        [Required(ErrorMessage = "Current password is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New passsword is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string NewPassword { get; set; }
    }
}
