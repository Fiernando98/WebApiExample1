using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class FoodsServices {
        public static List<Food> GetAll() {
            try {
                List<Food> list = new List<Food>();
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Foods", dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Food {
                                    ID = Convert.ToInt64(reader["id"].ToString()),
                                    Name = reader["name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Calories = Convert.ToDouble(reader["calories"])
                                });
                            }
                        }
                    }
                }
                return list;
            } catch (Exception) {
                throw;
            }
        }

        public static Food? GetSingle(int id) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Foods Where id = " + id, dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new Food {
                                    ID = Convert.ToInt64(reader["id"].ToString()),
                                    Name = reader["name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Calories = Convert.ToDouble(reader["calories"])
                                };
                            }
                        }
                    }
                }
            } catch (Exception) {
                throw;
            }
            return null;
        }
        public static Food? Create(Food newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    var query = "INSERT INTO Foods (name, description, calories) VALUES (?, ?, ?)";
                    using (SQLiteCommand command = new SQLiteCommand(query, dbContext)) {
                        command.Parameters.Add(new SQLiteParameter("name", newItem.Name));
                        command.Parameters.Add(new SQLiteParameter("description", newItem.Description));
                        command.Parameters.Add(new SQLiteParameter("calories", newItem.Calories));
                        command.ExecuteNonQuery();
                        newItem.ID = dbContext.LastInsertRowId;
                        return newItem;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
