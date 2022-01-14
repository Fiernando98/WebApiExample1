using System.ComponentModel.DataAnnotations;

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
        public Restaurant(long id,string name) => (ID, Name) = (id, name);

        [Required(ErrorMessage = "ID is required")]
        public long ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100,ErrorMessage = "{0} length must be between {2} and {1}.",MinimumLength = 5)]
        public string Name { get; set; }
    }
}
