using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class UsersServices {
        public static User Create(UserLogin newItem) {
            try {
                string encryptGUID = Guid.NewGuid().ToString();
                string? passwordEncrypted = EncryptionServices.Encrypt(newItem.Password,encryptGUID);
                if (passwordEncrypted == null) { throw new HttpResponseException(error: "Fallo en contraseña"); }
                newItem.Password = passwordEncrypted;
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {UserSQLTable.tableName} ({UserSQLTable.firstName}, {UserSQLTable.lastName}, {UserSQLTable.email}, {UserSQLTable.phone}, {UserSQLTable.encryptGUID}, {UserSQLTable.passwordEncrypted}) VALUES (?, ?, ?, ?, ?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.firstName}",newItem.FirstName));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.lastName}",newItem.LastName));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.email}",newItem.Email));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.phone}",newItem.Phone));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.encryptGUID}",encryptGUID));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.passwordEncrypted}",newItem.Password));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new HttpResponseException(StatusCodes.Status400BadRequest);
                        newItem.ID = dbContext.LastInsertRowId;
                        return newItem.getUser();
                    }
                }
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
            }
        }

        public static User? GetSingle(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {UserSQLTable.tableName} {whereSQL.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new User(
                                    id: Convert.ToInt64(reader[$"{UserSQLTable.id}"].ToString()),
                                    firstname: reader[$"{UserSQLTable.firstName}"].ToString()!,
                                    lastname: reader[$"{UserSQLTable.lastName}"].ToString()!,
                                    email: reader[$"{UserSQLTable.email}"].ToString()!
                                ) {
                                    Phone = reader[$"{UserSQLTable.phone}"].ToString()
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

        public static UserRegistrer? GetUserRegistrer(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {UserSQLTable.tableName}  {whereSQL?.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new UserRegistrer(
                                    id: Convert.ToInt64(reader[$"{UserSQLTable.id}"].ToString()),
                                    firstname: reader[$"{UserSQLTable.firstName}"].ToString()!,
                                    lastname: reader[$"{UserSQLTable.lastName}"].ToString()!,
                                    email: reader[$"{UserSQLTable.email}"].ToString()!,
                                    encryptGUID: reader[$"{UserSQLTable.encryptGUID}"].ToString()!,
                                    password: reader[$"{UserSQLTable.passwordEncrypted}"].ToString()!
                                ) {
                                    Phone = reader[$"{UserSQLTable.phone}"].ToString()
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

        public static void ChangePassword(WhereSQL whereSQL,string newPassword) {
            try {
                string encryptGUID = Guid.NewGuid().ToString();
                string? passwordEncrypted = EncryptionServices.Encrypt(newPassword,encryptGUID);
                if (passwordEncrypted == null) { throw new HttpResponseException(error: "Fallo en contraseña"); }
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"UPDATE {UserSQLTable.tableName} SET {UserSQLTable.encryptGUID} = ?, {UserSQLTable.passwordEncrypted} = ? {whereSQL.GetClausule()}",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.encryptGUID}",encryptGUID));
                        command.Parameters.Add(new SQLiteParameter($"{UserSQLTable.passwordEncrypted}",passwordEncrypted));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new HttpResponseException(StatusCodes.Status400BadRequest);
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
