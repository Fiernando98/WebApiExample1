using WebApplication1.Settings;
using System.Data.SQLite;
namespace WebApplication1.Services {
    public class TokenServices {
        public static bool ValidateToken(string? token) {
            if (token != null) {
                return _CheckToken(token.Split("bearer ").LastOrDefault());
            }
            return false;
        }

        private static bool _CheckToken(string? token) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Tokens WHERE token = " + token,dbContext)) {
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
