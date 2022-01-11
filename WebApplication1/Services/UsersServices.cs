using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class UsersServices {
        public static User Create(UserLogin newItem) {
            try {
                string encryptGUID = Guid.NewGuid().ToString();
                newItem.Password = EncryptionServices.Encrypt(newItem.Password!,encryptGUID);
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {UserSQLTable.tableName} ({UserSQLTable.firstName}, {UserSQLTable.lastName}, {UserSQLTable.email}, {UserSQLTable.phone}, {UserSQLTable.encryptGUID}, {UserSQLTable.passwordEncrypted}) VALUES (?, ?, ?, ?, ?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.firstName}",newItem.FirstName));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.lastName}",newItem.LastName));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.email}",newItem.Email));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.phone}",newItem.Phone));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.encryptGUID}",encryptGUID));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.passwordEncrypted}",newItem.Password));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new Exception("Error en base de datos");
                        newItem.ID = dbContext.LastInsertRowId;
                        return newItem.getUser();
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static User? GetSingle(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {UserSQLTable.tableName} {whereSQL.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new User {
                                    ID = Convert.ToInt64(reader[$"{UserSQLTable.id}"].ToString()),
                                    FirstName = reader[$"{UserSQLTable.firstName}"].ToString(),
                                    LastName = reader[$"{UserSQLTable.lastName}"].ToString(),
                                    Email = reader[$"{UserSQLTable.email}"].ToString(),
                                    Phone = reader[$"{UserSQLTable.phone}"].ToString()
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
