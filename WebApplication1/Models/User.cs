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
              $"{passwordEncrypted} TEXT"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }

    public class User {
        public long ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        private string? EncryptGUID { get; set; }
        private string? Password { get; set; }
    }
}
