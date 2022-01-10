namespace WebApplication1.Models {
    abstract class FoodSQLTable {
        public static string tableName = "Foods";

        public static string id = "id";
        public static string idRestaurant = "id_restaurant";
        public static string name = "name";
        public static string description = "description";
        public static string calories = "calories";

        private static string[] _table = {
              $"{id} INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL",
              $"{idRestaurant} INTEGER",
              $"{name} TEXT",
              $"{description} TEXT",
              $"{calories} REAL",
              $"FOREIGN KEY({idRestaurant}) REFERENCES {RestaurantSQLTable.tableName}({RestaurantSQLTable.id}) ON DELETE CASCADE ON UPDATE CASCADE"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }
    public class Food {
        public long ID { get; set; }
        public Restaurant? Restaurant { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Calories { get; set; }
    }
}
