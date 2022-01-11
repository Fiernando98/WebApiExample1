
using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class AuthServices {
        public static AuthToken Create(AuthToken newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {AuthTokenSQLTable.tableName} ({AuthTokenSQLTable.token}, {AuthTokenSQLTable.idUser}) VALUES (?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{AuthTokenSQLTable.token}",newItem.Token));
                        command.Parameters.Add(new SQLiteParameter($"{AuthTokenSQLTable.idUser}",newItem.User?.ID));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new Exception("Error en base de datos");
                        return newItem;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
        public static bool ValidateToken(string? token) {
            if (token != null) {
                return _CheckToken(token.Split("bearer ").LastOrDefault());
            }
            return false;
        }

        private static bool _CheckToken(string? token) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {AuthTokenSQLTable.tableName} WHERE {AuthTokenSQLTable.token} = {token}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return true;
                            }
                        }
                    }
                }
            } catch (Exception) {
                throw;
            }
            return false;
        }
    }
}
