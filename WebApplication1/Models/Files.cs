using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models {
    abstract class FilesSQLTable {
        public static string tableName = "Files";

        public static string id = "id";
        public static string path = "path";

        private static string[] _table = {
              $"{id} INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL",
              $"{path} TEXT"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }

    public class Files {
        public Files(long id) => (ID) = (id);

        [Required(ErrorMessage = "ID is required")]
        public long ID { get; set; }
        public string? Path { get; set; }
    }
}
