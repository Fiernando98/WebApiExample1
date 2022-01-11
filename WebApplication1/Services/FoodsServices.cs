using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class FoodsServices {
        public static List<Food> GetAll() {
            try {
                List<Food> list = new List<Food>();
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {FoodSQLTable.tableName}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Food {
                                    ID = Convert.ToInt64(reader[$"{FoodSQLTable.id}"].ToString()),
                                    Restaurant = (reader[$"{FoodSQLTable.idRestaurant}"] != null) ? RestaurantsServices.GetSingle(Convert.ToInt64(reader[$"{FoodSQLTable.idRestaurant}"].ToString())) : null,
                                    Name = reader[$"{FoodSQLTable.name}"].ToString(),
                                    Description = reader[$"{FoodSQLTable.description}"].ToString(),
                                    Calories = Convert.ToDouble(reader[$"{FoodSQLTable.calories}"])
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

        public static Food? GetSingle(long id) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {FoodSQLTable.tableName} WHERE {FoodSQLTable.id} = {id}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new Food {
                                    ID = Convert.ToInt64(reader[$"{FoodSQLTable.id}"].ToString()),
                                    Restaurant = RestaurantsServices.GetSingle(Convert.ToInt64(reader[$"{FoodSQLTable.idRestaurant}"].ToString())),
                                    Name = reader[$"{FoodSQLTable.name}"].ToString(),
                                    Description = reader[$"{FoodSQLTable.description}"].ToString(),
                                    Calories = Convert.ToDouble(reader[$"{FoodSQLTable.description}"])
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

        public static Food Create(Food newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {FoodSQLTable.tableName} ({FoodSQLTable.idRestaurant}, {FoodSQLTable.name}, {FoodSQLTable.description}, {FoodSQLTable.calories}) VALUES (?, ?, ?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.idRestaurant}",newItem.Restaurant?.ID));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.name}",newItem.Name));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.description}",newItem.Description));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.calories}",newItem.Calories));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new Exception("Error en base de datos");
                        newItem.ID = dbContext.LastInsertRowId;
                        if (newItem.Restaurant?.ID != null) {
                            newItem.Restaurant = RestaurantsServices.GetSingle(newItem.Restaurant.ID);
                        }
                        return newItem;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static Food? Edit(long id,Food item) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"UPDATE {FoodSQLTable.tableName} SET {FoodSQLTable.name} = ?, {FoodSQLTable.description} = ?, {FoodSQLTable.calories} = ? WHERE {FoodSQLTable.id} = {id}",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.name}",item.Name));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.description}",item.Description));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.calories}",item.Calories));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) return null;
                        item.ID = id;
                        return item;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static int Delete(long id) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"DELETE FROM {FoodSQLTable.tableName} WHERE {FoodSQLTable.id} = {id}",dbContext)) {
                        return command.ExecuteNonQuery();
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
