using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class RestaurantsServices {
        public static List<Restaurant> GetAll() {
            try {
                List<Restaurant> list = new List<Restaurant>();
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Restaurants",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Restaurant {
                                    ID = Convert.ToInt64(reader["id"].ToString()),
                                    Name = reader["name"].ToString()
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

        public static Restaurant? GetSingle(long id) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Restaurants WHERE id = " + id,dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new Restaurant {
                                    ID = Convert.ToInt64(reader["id"].ToString()),
                                    Name = reader["name"].ToString()
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
        public static Restaurant? Create(Restaurant newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO Restaurants (name) VALUES (?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter("name",newItem.Name));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) return null;
                        newItem.ID = dbContext.LastInsertRowId;
                        return newItem;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static Restaurant? Edit(long id,Restaurant item) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("UPDATE Restaurants SET name = ? WHERE ID = " + id,dbContext)) {
                        command.Parameters.Add(new SQLiteParameter("name",item.Name));
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
                    using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Restaurants WHERE id = " + id,dbContext)) {
                        return command.ExecuteNonQuery();
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
