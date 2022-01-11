
using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class AuthServices {
        public static AuthToken Create(AuthToken newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {AuthTokenSQLTable.tableName} ({AuthTokenSQLTable.token}, {AuthTokenSQLTable.dateTimeCreated}, {AuthTokenSQLTable.idUser}) VALUES (?, ?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{AuthTokenSQLTable.token}",newItem.Token));
                        command.Parameters.Add(new SQLiteParameter($"{AuthTokenSQLTable.dateTimeCreated}",newItem.DateTimeCreated.ToFileTimeUtc()));
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
                try {
                    using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                        using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {AuthTokenSQLTable.tableName} WHERE {AuthTokenSQLTable.token} = '{token.Split(" ").LastOrDefault()}'",dbContext)) {
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
            }
            return false;
        }

        public static UserRegistrer? GetUserRegistrer(string email) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {UserSQLTable.tableName} WHERE {UserSQLTable.email} = '{email}'",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new UserRegistrer {
                                    ID = Convert.ToInt64(reader[$"{UserSQLTable.id}"].ToString()),
                                    FirstName = reader[$"{UserSQLTable.firstName}"].ToString(),
                                    LastName = reader[$"{UserSQLTable.lastName}"].ToString(),
                                    Email = reader[$"{UserSQLTable.email}"].ToString(),
                                    Phone = reader[$"{UserSQLTable.phone}"].ToString(),
                                    EncryptGUID = reader[$"{UserSQLTable.encryptGUID}"].ToString(),
                                    Password = reader[$"{UserSQLTable.passwordEncrypted}"].ToString()
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
    }
}
