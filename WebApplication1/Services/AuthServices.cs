
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
            } catch (HttpResponseException) {
                throw;
            } catch (Exception ex) {
                throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
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
                } catch (HttpResponseException) {
                    throw;
                } catch (Exception ex) {
                    throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
                }
            }
            return false;
        }

        public static User? GetUserByToken(string? token) {
            if (token != null) {
                try {
                    using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                        using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {AuthTokenSQLTable.tableName} WHERE {AuthTokenSQLTable.token} = '{token.Split(" ").LastOrDefault()}'",dbContext)) {
                            using (SQLiteDataReader reader = command.ExecuteReader()) {
                                while (reader.Read()) {
                                    return UsersServices.GetSingle(new WhereSQL {
                                        SQLClauses = new string[] {
                                            $"{UserSQLTable.id} = {Convert.ToInt64(reader[$"{AuthTokenSQLTable.idUser}"].ToString())}"
                                        }
                                    });
                                }
                            }
                        }
                    }
                } catch (HttpResponseException) {
                    throw;
                } catch (Exception ex) {
                    throw new HttpResponseException(statusCode: StatusCodes.Status400BadRequest,error: ex.Message);
                }
            }
            return null;
        }

        public static UserRegistrer? GetUserRegistrerByToken(string? token) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {AuthTokenSQLTable.tableName} WHERE {AuthTokenSQLTable.token} = '{token?.Split(" ").LastOrDefault()}'",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return UsersServices.GetUserRegistrer(new WhereSQL {
                                    SQLClauses = new string[] {
                                            $"{UserSQLTable.id} = {Convert.ToInt64(reader[$"{AuthTokenSQLTable.idUser}"].ToString())}"
                                        }
                                });
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
    }
}
