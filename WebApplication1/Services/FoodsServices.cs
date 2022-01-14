using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class FoodsServices {
        public static List<Food> GetAll(WhereSQL whereSQL) {
            try {
                List<Food> list = new List<Food>();
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {FoodSQLTable.tableName} {whereSQL.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Food(id: Convert.ToInt64(reader[$"{FoodSQLTable.id}"].ToString()),name: reader[$"{FoodSQLTable.name}"].ToString()!,calories: Convert.ToDouble(reader[$"{FoodSQLTable.calories}"])) {
                                    Restaurant = (reader[$"{FoodSQLTable.idRestaurant}"] != null) ? RestaurantsServices.GetSingle(new WhereSQL {
                                        SQLClauses = new string[] { $"{RestaurantSQLTable.id} = {Convert.ToInt64(reader[$"{FoodSQLTable.idRestaurant}"].ToString())}" }
                                    }) : null,
                                    Description = reader[$"{FoodSQLTable.description}"].ToString()
                                });
                            }
                        }
                    }
                }
                return list;
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
            }
        }

        public static Food? GetSingle(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {FoodSQLTable.tableName} {whereSQL?.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new Food(id: Convert.ToInt64(reader[$"{FoodSQLTable.id}"].ToString()),name: reader[$"{FoodSQLTable.name}"].ToString()!,calories: Convert.ToDouble(reader[$"{FoodSQLTable.calories}"])) {
                                    Restaurant = (reader[$"{FoodSQLTable.idRestaurant}"] != null) ? RestaurantsServices.GetSingle(new WhereSQL {
                                        SQLClauses = new string[] { $"{RestaurantSQLTable.id} = {Convert.ToInt64(reader[$"{FoodSQLTable.idRestaurant}"].ToString())}" }
                                    }) : null,
                                    Description = reader[$"{FoodSQLTable.description}"].ToString()
                                };
                            }
                        }
                    }
                }
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
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
                            newItem.Restaurant = RestaurantsServices.GetSingle(new WhereSQL {
                                SQLClauses = new string[] { $"{RestaurantSQLTable.id} = {newItem.Restaurant.ID}" }
                            });
                        }
                        return newItem;
                    }
                }
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
            }
        }

        public static Food Edit(WhereSQL whereSQL,Food item) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"UPDATE {FoodSQLTable.tableName} SET {FoodSQLTable.name} = ?, {FoodSQLTable.description} = ?, {FoodSQLTable.calories} = ? {whereSQL?.GetClausule()}",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.name}",item.Name));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.description}",item.Description));
                        command.Parameters.Add(new SQLiteParameter($"{FoodSQLTable.calories}",item.Calories));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new HttpResponseException(StatusCodes.Status404NotFound);
                        return item;
                    }
                }
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
            }
        }

        public static bool Delete(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"DELETE FROM {FoodSQLTable.tableName} {whereSQL?.GetClausule()}",dbContext)) {
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
            }
        }
    }
}
