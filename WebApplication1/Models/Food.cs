using System.ComponentModel.DataAnnotations;

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
              $"{idRestaurant} INTEGER NOT NULL",
              $"{name} TEXT NOT NULL",
              $"{description} TEXT",
              $"{calories} REAL NOT NULL",
              $"FOREIGN KEY({idRestaurant}) REFERENCES {RestaurantSQLTable.tableName}({RestaurantSQLTable.id}) ON DELETE CASCADE ON UPDATE CASCADE"
      };

        public static string toCreateQuery => $"CREATE TABLE IF NOT EXISTS {tableName} ({String.Join(", ",_table)});";
    }
    public class Food {
        public Food(long id,Restaurant restaurant,string name,double calories) => (ID, Restaurant, Name, Calories) = (id, restaurant, name, calories);

        [Required(ErrorMessage = "ID is required")]
        public long ID { get; set; }

        [Required(ErrorMessage = "Restaurant is required")]
        public Restaurant? Restaurant { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string Name { get; set; }

        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 0)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Calories is required")]
        public double Calories { get; set; }
    }
}
