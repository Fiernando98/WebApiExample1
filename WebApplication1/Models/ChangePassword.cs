namespace WebApplication1.Models {
    public class ChangePassword {
        public ChangePassword(string currentPassword,string newPassword) =>
        (CurrentPassword, NewPassword) = (currentPassword, newPassword);
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
