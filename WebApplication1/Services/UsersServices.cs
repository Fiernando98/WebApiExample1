using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class UsersServices {
        public static User Create(UserRegistrer newItem) {
            try {
                newItem.EncryptGUID = Guid.NewGuid().ToString();
                newItem.Password = EncryptionServices.Encrypt(newItem.Password!,newItem.EncryptGUID);
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {UserSQLTable.tableName} ({UserSQLTable.firstName}, {UserSQLTable.lastName}, {UserSQLTable.email}, {UserSQLTable.phone}, {UserSQLTable.encryptGUID}, {UserSQLTable.passwordEncrypted}) VALUES (?, ?, ?, ?, ?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.firstName}",newItem.FirstName));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.lastName}",newItem.LastName));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.email}",newItem.Email));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.phone}",newItem.Phone));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.encryptGUID}",newItem.EncryptGUID));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.passwordEncrypted}",newItem.Password));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new Exception("Error en base de datos");
                        newItem.ID = dbContext.LastInsertRowId;
                        return newItem.toPrivatedUser();
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static UserRegistrer? GetSingle(string email) {
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
