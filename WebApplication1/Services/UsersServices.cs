using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class UsersServices {
        public static User? Create(User newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {UserSQLTable.tableName} (name) VALUES (?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter("name",newItem.FirstName));
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
    }
}
