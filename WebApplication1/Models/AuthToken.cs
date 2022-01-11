using WebApplication1.Models;
namespace WebApplication1.Models {
    abstract class AuthTokenSQLTable {
        public static string tableName = "Auth_token";

        public static string token = "token";
        public static string dateTimeCreated = "date_time_created";
        public static string idUser = "id_user";

        private static string[] _table = {
              $"{token} TEXT PRIMARY KEY NOT NULL",
              $"{dateTimeCreated} INTEGER",
              $"{idUser} INTEGER",
              $"FOREIGN KEY({idUser}) REFERENCES {UserSQLTable.tableName}({UserSQLTable.id}) ON DELETE CASCADE ON UPDATE CASCADE"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }
    public class AuthToken {
        public User? User { get; set; }
        public string? Token { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
    }
}
