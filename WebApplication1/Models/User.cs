using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models {
    abstract class UserSQLTable {
        public static string tableName = "Users";

        public static string id = "id";
        public static string firstName = "first_name";
        public static string lastName = "last_name";
        public static string email = "email";
        public static string phone = "phone";
        public static string encryptGUID = "encrypt_guid";
        public static string passwordEncrypted = "password_encrypted";

        private static string[] _table = {
              $"{id} INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL",
              $"{firstName} TEXT",
              $"{lastName} TEXT",
              $"{email} TEXT NOT NULL UNIQUE",
              $"{phone} TEXT",
              $"{encryptGUID} TEXT NOT NULL",
              $"{passwordEncrypted} TEXT NOT NULL"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }

    public class User {
        public User(string firstname,string lastname,string email,string phone) =>
        (FirstName, LastName, Email, Phone) = (firstname, lastname, email, phone);

        [Required(ErrorMessage = "ID is required")]
        public long ID { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string Phone { get; set; }
    }

    public class UserLogin : User {
        public UserLogin(string firstname,string lastname,string email,string phone,string password)
            : base(firstname: firstname,lastname: lastname,email: email,phone: phone) =>
        (Password) = (password);

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string Password { get; set; }
        public User getUser() => new User(
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone
        ) {
            ID = ID
        };
    }

    public class UserRegistrer : UserLogin {
        public UserRegistrer(string firstname,string lastname,string email,string phone,string password,string encryptGUID)
            : base(firstname: firstname,lastname: lastname,email: email,phone: phone,password: password) =>
        (EncryptGUID) = (encryptGUID);

        [Required(ErrorMessage = "EncryptGUID is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 1)]
        public string EncryptGUID { get; set; }
        public UserLogin toUser() => (UserLogin)this.getUser();
    }
}
