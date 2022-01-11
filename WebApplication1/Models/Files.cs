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
        public long ID { get; set; }
        public Uri? Path { get; set; }
    }
}
