namespace WebApplication1.Models {
    abstract class RestaurantSQLTable {
        public static string tableName = "Restaurants";

        public static string id = "id";
        public static string name = "name";

        private static string[] _table = {
              $"{id} INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL",
              $"{name} TEXT"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }
    public class Restaurant {
        public long ID { get; set; }
        public string? Name { get; set; }
    }
}
